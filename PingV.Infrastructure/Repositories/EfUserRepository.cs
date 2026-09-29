using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PingV.Domain.Entities;
using PingV.Domain.Exceptions;
using PingV.Domain.Interfaces;
using PingV.Infrastructure.Data;
using PingV.Infrastructure.Mappers;
using PingV.Application.Resources;


namespace PingV.Infrastructure.Repositories; 

public sealed class EfUserRepository(
    PingVDbContext context, 
    ILogger<EfUserRepository> logger 
) : IUserRepository 
{
    private readonly PingVDbContext _context = context;
    private readonly ILogger<EfUserRepository> _logger = logger;

    public async Task<IEnumerable<User>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var users = await _context.Users.ToListAsync(ct); 

            return users.Select(user => user.EfToDomain());
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
        }
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var user = await _context.Users.FindAsync([id], ct); 

            if(user == null)
            {
                throw new ServerSideException(ServerSideErrorMessages.NotFound);
            }

            return user.EfToDomain();
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
        }
    }

    public async Task<User> GetByLoginAsync(string login, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == login, ct);

            if (user == null)
            {
                throw new ServerSideException(ServerSideErrorMessages.NotFound);
            }

            return user.EfToDomain();
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
        }
    }
}
