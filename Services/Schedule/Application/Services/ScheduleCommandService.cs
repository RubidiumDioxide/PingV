namespace Schedule.Application.Services; 

public class ScheduleCommandService(
    IScheduleRepository slotRepository,
    IDistributedCache cache,
    string dbPrefix,
    ICurrentUser currentUser 
) : IScheduleCommandService 
{
    private readonly IScheduleRepository _scheduleRepository = slotRepository; 
    private readonly IDistributedCache _cache = cache;
    private readonly string _dbPrefix = dbPrefix;
    private readonly ICurrentUser _currentUser = currentUser; 
    
    public async Task CreateAsync(CreateScheduleSlotRequest request, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var newSlot = new ScheduleSlot(Guid.NewGuid(), _currentUser.Id, request.Start, request.End, request.Note);

        await _scheduleRepository.AddAsync(newSlot, ct);

        var monthStart = new DateTimeOffset(request.Start.Year, request.Start.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var monthEnd = new DateTimeOffset(request.End.Year, request.End.Month, 1, 0, 0, 0, TimeSpan.Zero);

        for (var month = monthStart; month <= monthEnd; month = month.AddMonths(1))
        {
            await _cache.RemoveAsync(GetCacheKey(_dbPrefix, month.Year, month.Month), ct);
        }

        return;
    }

    public async Task UpdateAsync(Guid id, UpdateScheduleSlotRequest request, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var updatedSlot = new ScheduleSlot(id, _currentUser.Id, request.Start, request.End, request.Note);

        await _scheduleRepository.UpdateAsync(updatedSlot, ct);

        var monthStart = new DateTimeOffset(request.Start.Year, request.Start.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var monthEnd = new DateTimeOffset(request.End.Year, request.End.Month, 1, 0, 0, 0, TimeSpan.Zero);

        for (var month = monthStart; month <= monthEnd; month = month.AddMonths(1))
        {
            await _cache.RemoveAsync(GetCacheKey(_dbPrefix, month.Year, month.Month), ct);
        }

        return;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var deletedSlot = await _scheduleRepository.GetByIdAsync(id, ct) ?? throw new ApplicationLevelException($"Slot not found {id}");

        await _scheduleRepository.RemoveAsync(id, ct);

        var monthStart = new DateTimeOffset(deletedSlot.Start.Year, deletedSlot.Start.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var monthEnd = new DateTimeOffset(deletedSlot.End.Year, deletedSlot.End.Month, 1, 0, 0, 0, TimeSpan.Zero);

        for (var month = monthStart; month <= monthEnd; month = month.AddMonths(1))
        {
            await _cache.RemoveAsync(GetCacheKey(_dbPrefix, month.Year, month.Month), ct);
        }
    }
}
