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

public class WishlistItemQueryService(
    PingVDbContext context,
    IDistributedCache cache, 
    string dbProviderPrefix, 
    ILogger<WishlistItemQueryService> logger
) : IWishlistItemQueryService
{
    private readonly PingVDbContext _context = context;
    private readonly IDistributedCache _cache = cache;
    private readonly string _dbProviderPrefix = dbProviderPrefix; 
    private readonly ILogger<WishlistItemQueryService> _logger = logger;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="creatorId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task<IEnumerable<WishlistItemDto>> GetByCreatorId(Guid creatorId, CancellationToken ct = default)
    {
        try
        {
            var items = Enumerable.Empty<WishlistItemDto>();

            var cacheKey = GetCacheKey(creatorId);
            var cachedValue = await _cache.GetStringAsync(cacheKey, ct);
            if (cachedValue != null)
            {
                items = JsonSerializer.Deserialize<IEnumerable<WishlistItemDto>>(cachedValue, JsonOptions) ?? [];
            }
            else 
            {
                items = await _context.WishlistItems
                    .Where(item => item.CreatorId == creatorId)
                    .Select(WishlistItemEfToDto).ToListAsync(ct);                
                
                var payload = JsonSerializer.Serialize(items, JsonOptions);
                await _cache.SetStringAsync(cacheKey, payload, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                }, ct);
            }

            return items;
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
        }
    }


    // --- HELPERS --- 
    private string GetCacheKey(Guid creatorId) => $"{_dbProviderPrefix}:wishlistItemsByCreatorId:{creatorId}";
}
