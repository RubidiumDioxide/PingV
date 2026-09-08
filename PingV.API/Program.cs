using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PingV.Application.Services;
using PingV.Application.Services.Interafces;
using PingV.Domain.Interfaces;
using PingV.Infrastructure.Data;
using PingV.Infrastructure.Initialization;
using PingV.Infrastructure.Repositories;
using PingV.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbProvider = builder.Configuration.GetValue<string>("DbProvider");
var connectionString = builder.Configuration.GetConnectionString(dbProvider ?? "PostgreSQL"); 

switch (dbProvider)
{
    case "PostgreSQL":
        builder.Services.AddDbContextFactory<PingVDbContext>(
        options => options
            .UseNpgsql(
                connectionString,
                postgreSqlOptions =>
                {
                    postgreSqlOptions.MigrationsAssembly("PingV.Infrastructure.PostgreSQL");
                }
            )
            .UseLazyLoadingProxies()
        );
        
        break;
    case "SQLServer":
        builder.Services.AddDbContextFactory<PingVDbContext>(
        options => options
            .UseSqlServer(
                connectionString,
                sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly("PingV.Infrastructure.SQLServer");
                }
            )
            .UseLazyLoadingProxies()
        );
        
        break;
    default:
        throw new InvalidOperationException($"Unsupported database provider: {dbProvider}");
}

builder.Services.AddScoped<PingVDbContext>(
    sp => sp.GetRequiredService<IDbContextFactory<PingVDbContext>>().CreateDbContext()
);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "PingVCache:";
});

// repositories 
builder.Services.AddScoped<IAvailabilitySlotRepository, EfAvailabilitySlotRepository>(); 
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

// services 
builder.Services.AddScoped<IAvailabilitySlotQueryService, AvailabilitySlotQueryService>(sp =>
{
    var context = sp.GetRequiredService<PingVDbContext>();
    var cache = sp.GetRequiredService<IDistributedCache>(); 
    var logger = sp.GetRequiredService<ILogger<AvailabilitySlotQueryService>>();

    return new AvailabilitySlotQueryService(context, cache, dbProvider, logger); 
});
builder.Services.AddScoped<IAvailabilitySlotCommandService, AvailabilitySlotCommandService>(sp =>
{
    var availabilitySlotRepository = sp.GetRequiredService<IAvailabilitySlotRepository>(); 
    var userRepository = sp.GetRequiredService<IUserRepository>(); 
    var cache = sp.GetRequiredService<IDistributedCache>();
    var logger = sp.GetRequiredService<ILogger<AvailabilitySlotCommandService>>();

    return new AvailabilitySlotCommandService(availabilitySlotRepository, userRepository, cache, dbProvider, logger);
});
builder.Services.AddScoped<IUserQueryService, UserQueryService>(sp =>
{
    var context = sp.GetRequiredService<PingVDbContext>();
    var cache = sp.GetRequiredService<IDistributedCache>();
    var logger = sp.GetRequiredService<ILogger<UserQueryService>>();

    return new UserQueryService(context, cache, dbProvider, logger);
});

// hosted services 
builder.Services.AddHostedService<DatabaseInitializationService>(sp =>
{
    var contextFactory = sp.GetRequiredService<IDbContextFactory<PingVDbContext>>(); 
    var testUserLogins = builder.Configuration
        .GetSection("TestUserLogins")
        .Get<string[]>();
    var logger = sp.GetRequiredService<ILogger<DatabaseInitializationService>>(); 

    return new DatabaseInitializationService(contextFactory, testUserLogins, logger);
});

var app = builder.Build();

// Configure the HTTP request pipeline. 
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PingV API v1");
});
app.UseAuthorization();
app.MapControllers();

app.Run();
