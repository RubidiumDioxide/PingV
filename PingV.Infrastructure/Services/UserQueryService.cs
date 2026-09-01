using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PingV.Application.Dtos;
using PingV.Application.Services.Interafces;
using PingV.Domain.Exceptions;
using PingV.Infrastructure.Data;
using System.Text.Json;
using PingV.Application.Resources;
using static PingV.Infrastructure.Mappers.Mapper;


namespace PingV.Infrastructure.Services;

public sealed class UserQueryService( 
    PingVDbContext  context, 
    IDistributedCache cache, 
    string dbProviderPrefix, 
    ILogger<UserQueryService> logger 
) : IUserQueryService
{
    private readonly PingVDbContext _context = context;
    private readonly IDistributedCache _cache = cache; 
    private readonly string _dbProviderPrefix = dbProviderPrefix; 
    private readonly ILogger<UserQueryService> _logger = logger; 
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task<IEnumerable<UserDto>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            var cacheKey = GetCacheKey();
            var cachedValue = await _cache.GetStringAsync(cacheKey, ct);
            if (cachedValue != null)
            {
                var users = JsonSerializer.Deserialize<IEnumerable<UserDto>>(cachedValue, JsonOptions);

                if (users == null)
                {
                    throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
                }

                return users;
            }
            else
            {
                var users = await _context.Users.Select(UserEfToDto).ToListAsync(ct); 
                var payload = JsonSerializer.Serialize(users, JsonOptions);
                await _cache.SetStringAsync(cacheKey, payload, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                }, ct); 

                return users; 
            }
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
        }
    }


    public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var cacheKey = GetCacheKey(id);
            var cachedValue = await _cache.GetStringAsync(cacheKey, ct);
            if (cachedValue != null)
            {
                var user = JsonSerializer.Deserialize<UserDto>(cachedValue, JsonOptions);

                if (user == null)
                {
                    throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
                }

                return user;
            }
            else
            {
                var user = await _context.Users
                    .Where(u => u.Id == id)
                    .Select(UserEfToDto)
                    .FirstOrDefaultAsync(ct); 

                if (user == null)
                {
                    throw new ServerSideException(ServerSideErrorMessages.NotFound);
                }

                var payload = JsonSerializer.Serialize(user, JsonOptions);
                await _cache.SetStringAsync(cacheKey, payload, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                }, ct);

                return user;
            }
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
        }
    }


    // --- HELPERS --- 
    private string GetCacheKey() => $"{_dbProviderPrefix}:users:all";
    private string GetCacheKey(Guid id) => $"{_dbProviderPrefix}:userById:{id}"; 
}
