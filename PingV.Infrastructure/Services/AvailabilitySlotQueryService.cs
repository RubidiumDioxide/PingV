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

public class AvailabilitySlotQueryService(
    PingVDbContext context,
    IDistributedCache cache, 
    string dbProviderPrefix, 
    ILogger<AvailabilitySlotQueryService> logger
) : IAvailabilitySlotQueryService
{
    private readonly PingVDbContext _context = context;
    private readonly IDistributedCache _cache = cache;
    private readonly string _dbProviderPrefix = dbProviderPrefix; 
    private readonly ILogger<AvailabilitySlotQueryService> _logger = logger;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ServerSideException"></exception>
    public async Task<IEnumerable<AvailabilitySlotDto>> GetMonthlyScheduleAsync(Guid userId, int year, int month, CancellationToken ct = default)
    {
        try
        {
            var availabilitySlots = Enumerable.Empty<AvailabilitySlotDto>();

            var cacheKey = GetCacheKey(year, month);
            var cachedValue = await _cache.GetStringAsync(cacheKey, ct);
            if (cachedValue != null)
            {
                availabilitySlots = JsonSerializer.Deserialize<IEnumerable<AvailabilitySlotDto>>(cachedValue, JsonOptions) ?? [];
            }
            else
            {
                var monthStart = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
                var monthEnd = monthStart.AddMonths(1);

                availabilitySlots = await _context.AvailabilitySlots
                    .Where(slot => slot.CreatorId == userId && slot.Start < monthEnd && slot.End > monthStart)
                    .Select(AvailabilitySlotEfToDto).ToListAsync(ct);                
                
                var payload = JsonSerializer.Serialize(availabilitySlots, JsonOptions);
                await _cache.SetStringAsync(cacheKey, payload, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                }, ct);
            }

            return availabilitySlots;
        }
        catch (ServerSideException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new ServerSideException(ServerSideErrorMessages.GetGenericError);
        }
    }


    // --- HELPERS --- 
    private string GetCacheKey(int year, int month) => $"{_dbProviderPrefix}:monthlySchedule:{year}:{month}";
}
