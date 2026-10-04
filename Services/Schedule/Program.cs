using Schedule.Infrastructure.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

var dbProvider = builder.Configuration.GetValue<string>("DbProvider"); 
var connectionString = builder.Configuration.GetConnectionString(dbProvider ?? "PostgreSQL");

switch (dbProvider)
{
    case "PostgreSQL":
        builder.Services.AddDbContextFactory<ScheduleDbContext>(
        options => options
            .UseNpgsql(connectionString)
            .UseLazyLoadingProxies()
        );

        break;
    default:
        throw new InvalidOperationException($"Unsupported database provider: {dbProvider}");
}

// repositories  
builder.Services.AddScoped<IScheduleRepository, DbScheduleRepository>();

var userMock = new CurrentUserMock();

// services 
builder.Services.AddScoped<IScheduleQueryService, ScheduleQueryService>(sp =>
{
    var context = sp.GetRequiredService<ScheduleDbContext>();
    var cache = sp.GetRequiredService<IDistributedCache>();
    var logger = sp.GetRequiredService<ILogger<ScheduleQueryService>>();
    var dbPrefix = $"{dbProvider}:Schedule";

    return new ScheduleQueryService(context, cache, dbPrefix, logger, userMock);
});
builder.Services.AddScoped<IScheduleCommandService, ScheduleCommandService>(sp =>
{
    var scheduleRepository = sp.GetRequiredService<DbScheduleRepository>();
    var cache = sp.GetRequiredService<IDistributedCache>();
    var logger = sp.GetRequiredService<ILogger<ScheduleCommandService>>();
    var dbPrefix = $"{dbProvider}:Schedule"; 

    return new ScheduleCommandService(scheduleRepository, cache, dbPrefix, logger, userMock);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PingV API v1");
}); 
app.UseAuthorization();
app.MapControllers();

app.Run(); 