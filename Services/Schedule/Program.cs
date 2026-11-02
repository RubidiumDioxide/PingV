var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();  

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
    var dbPrefix = $"{dbProvider}:Schedule";

    return new ScheduleQueryService(context, cache, dbPrefix, userMock);
});
builder.Services.AddScoped<IScheduleCommandService, ScheduleCommandService>(sp =>
{
    var scheduleRepository = sp.GetRequiredService<DbScheduleRepository>();
    var cache = sp.GetRequiredService<IDistributedCache>();
    var dbPrefix = $"{dbProvider}:Schedule"; 

    return new ScheduleCommandService(scheduleRepository, cache, dbPrefix, userMock);
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseSwagger(); 
app.UseExceptionHandler();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run(); 