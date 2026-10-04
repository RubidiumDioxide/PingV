namespace Schedule.Infrastructure.Services; 

public class ScheduleQueryService(
    ScheduleDbContext context,
    IDistributedCache cache,
    string dbPrefix,
    ILogger<ScheduleQueryService> logger, 
    ICurrentUser currentUser 
) : IScheduleQueryService
{
    private readonly ScheduleDbContext _context = context;
    private readonly IDistributedCache _cache = cache;
    private readonly string _dbPrefix = dbPrefix;
    private readonly ILogger<ScheduleQueryService> _logger = logger;
    private readonly ICurrentUser _currentUser = currentUser; 
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task<IEnumerable<ScheduleSlotDto>> GetByMonthAsync(int year, int month, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var slots = Enumerable.Empty<ScheduleSlotDto>();

            var cacheKey = GetCacheKey(_dbPrefix, _currentUser.Id, year, month);
            var cachedValue = await _cache.GetStringAsync(cacheKey, ct);
            if (cachedValue != null)
            {
                slots = JsonSerializer.Deserialize<IEnumerable<ScheduleSlotDto>>(cachedValue, JsonOptions) ?? [];
            }
            else
            {
                var monthStart = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
                var monthEnd = monthStart.AddMonths(1);

                slots = await _context.ScheduleSlots
                    .Where(slot => slot.CreatorId == _currentUser.Id && slot.Start < monthEnd && slot.End > monthStart)
                    .Select(ScheduleSlotDbToDto).ToListAsync(ct);

                var payload = JsonSerializer.Serialize(slots, JsonOptions);
                await _cache.SetStringAsync(cacheKey, payload, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                }, ct);
            }

            return slots;
        }
        catch (OperationCanceledException) { throw; }
        catch (InfrastructureLevelException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get a monthly schedule ");
            throw new InfrastructureLevelException("Failed to get a monthly schedule ", ex);
        }
    }
}
