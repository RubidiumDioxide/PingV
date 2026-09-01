using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using PingV.Infrastructure.Data.Models;
using PingV.Infrastructure.Data;


namespace PingV.Infrastructure.Initialization; 

public class DatabaseInitializationService(
    IDbContextFactory<PingVDbContext> contextFactory,
    string[]? testUserLogins,   
    ILogger<DatabaseInitializationService> logger
) : IHostedService 
{
    private readonly IDbContextFactory<PingVDbContext> _contextFactory = contextFactory; 
    private readonly string[]? _testUserLogins = testUserLogins;
    private readonly ILogger<DatabaseInitializationService> _logger = logger;

    public async Task StartAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting database initialization ");

        using var _context = await _contextFactory.CreateDbContextAsync(ct);

        try
        {
            // check connection to db 
            if (!await _context.Database.CanConnectAsync(ct))
            {
                _logger.LogWarning("Unable to connect to a database. Creating a new db ");
            }

            // apply migrations while creating a new db if necessary 
            await _context.Database.MigrateAsync(ct);

            if(_testUserLogins != null)
            {
                foreach (var userLogin in _testUserLogins)
                {
                    var existiingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin, ct);

                    if (existiingUser == null)
                    {
                        await _context.Users.AddAsync(new UserEfModel() { Login = userLogin }, ct); 
                    }
                }

                await _context.SaveChangesAsync(ct); 
            }

            _logger.LogInformation("Database initialized successfully ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured during database initialization ");
            throw;
        }
    }

    public Task StopAsync(CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}
