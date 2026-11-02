namespace Schedule.Infrastructure.Services; 

public class ScheduleQueryService(
    ScheduleDbContext context,
    IDistributedCache cache,
    string dbPrefix,
    ICurrentUser currentUser 
) : IScheduleQueryService
{
    private readonly ScheduleDbContext _context = context;
    private readonly IDistributedCache _cache = cache;
    private readonly string _dbPrefix = dbPrefix;
    private readonly ICurrentUser _currentUser = currentUser; 
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task<ScheduleSlotDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        ScheduleSlotDto? slot = null;

        var cacheKey = GetCacheKey(_dbPrefix, id);
        var cachedValue = await _cache.GetStringAsync(cacheKey, ct);
        if (cachedValue != null)
        {
            slot = JsonSerializer.Deserialize<ScheduleSlotDto>(cachedValue, JsonOptions);
        }
        else
        {
            var slotDb = await _context.ScheduleSlots.FindAsync([id], ct);

            if (slotDb != null)
            {
                slot = slotDb.DbToDto();

                var payload = JsonSerializer.Serialize(slot, JsonOptions);
                await _cache.SetStringAsync(cacheKey, payload, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                }, ct);
            }
        }

        return slot ?? throw new DomainLevelException("Schedule slot not found ");
    }

    public async Task<IEnumerable<ScheduleSlotDto>> GetByMonthAsync(int year, int month, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var slots = Enumerable.Empty<ScheduleSlotDto>();

        var cacheKey = GetCacheKey(_dbPrefix, year, month);
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
                .Select(DbToDtoExpression).ToListAsync(ct);

            var payload = JsonSerializer.Serialize(slots, JsonOptions);
            await _cache.SetStringAsync(cacheKey, payload, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            }, ct);
        }

        return slots;
    }
}
