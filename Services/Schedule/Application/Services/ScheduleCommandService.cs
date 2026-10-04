namespace Schedule.Application.Services; 

public class ScheduleCommandService(
    IScheduleRepository slotRepository,
    IDistributedCache cache,
    string dbPrefix,
    ILogger<ScheduleCommandService> logger, 
    ICurrentUser currentUser 
) : IScheduleCommandService 
{
    private readonly IScheduleRepository _scheduleRepository = slotRepository; 
    private readonly IDistributedCache _cache = cache;
    private readonly string _dbPrefix = dbPrefix;
    private readonly ILogger<ScheduleCommandService> _logger = logger;
    private readonly ICurrentUser _currentUser = currentUser;
    
    public async Task CreateAsync(CreateScheduleSlotRequest request, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var newSlot = new ScheduleSlot(Guid.NewGuid(), _currentUser.Id, new TimeRange(request.Start, request.End), request.Note);

            await _scheduleRepository.AddAsync(newSlot, ct);

            var monthStart = new DateTimeOffset(request.Start.Year, request.Start.Month, 1, 0, 0, 0, TimeSpan.Zero);
            var monthEnd = new DateTimeOffset(request.End.Year, request.End.Month, 1, 0, 0, 0, TimeSpan.Zero);

            for (var month = monthStart; month <= monthEnd; month = month.AddMonths(1))
            {
                await _cache.RemoveAsync(GetCacheKey(_dbPrefix, _currentUser.Id, month.Year, month.Month), ct);
            }

            return;
        }
        catch (OperationCanceledException) { throw; }
        catch (ApplicationException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create a schedule slot ");
            throw new ApplicationLevelException("Failed to create a schedule slot ", ex);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var deletedSlot = await _scheduleRepository.GetByIdAsync(id, ct); 
            
            if(deletedSlot == null)
                throw new ApplicationLevelException($"Slot not found {id}"); 
            
            await _scheduleRepository.RemoveAsync(id, ct);

            var monthStart = new DateTimeOffset(deletedSlot.TimeRange.Start.Year, deletedSlot.TimeRange.Start.Month, 1, 0, 0, 0, TimeSpan.Zero);
            var monthEnd = new DateTimeOffset(deletedSlot.TimeRange.End.Year, deletedSlot.TimeRange.End.Month, 1, 0, 0, 0, TimeSpan.Zero);

            for (var month = monthStart; month <= monthEnd; month = month.AddMonths(1))
            {
                await _cache.RemoveAsync(GetCacheKey(_dbPrefix, _currentUser.Id, month.Year, month.Month), ct);
            }
        }
        catch (OperationCanceledException) { throw; }
        catch (InfrastructureLevelException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete a schedule slot {id}", id);
            throw new ApplicationLevelException("Failed to delete a schedule slot ", ex);
        }
    }
}
