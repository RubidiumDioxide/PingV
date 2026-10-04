namespace Schedule.Infrastructure.Repositories; 

public class DbScheduleRepository(
    ScheduleDbContext context,
    ILogger<DbScheduleRepository> logger
) : IScheduleRepository 
{
    private readonly ScheduleDbContext _context = context;
    private readonly ILogger<DbScheduleRepository> _logger = logger;

    public async Task AddAsync(ScheduleSlot slot, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var newSlot = slot.DomainToDb();

            await _context.ScheduleSlots.AddAsync(newSlot, ct);
            await _context.SaveChangesAsync(ct);
        }
        catch (OperationCanceledException) { throw; }
        catch (DomainLevelException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add a schedule slot ");
            throw new DomainLevelException("Failed to add a schedule slot ", ex);
        }
    }

    public async Task<ScheduleSlot?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var slot = await _context.ScheduleSlots
                .FirstOrDefaultAsync(slot => slot.Id == id); 

            return slot.DbToDomain();
        }
        catch (OperationCanceledException) { throw; }
        catch (DomainLevelException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get a schedule slot {id}", id); 
            throw new DomainLevelException("Failed to get a schedule slot ", ex);
        }
    }

    public async Task RemoveAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            await _context.ScheduleSlots
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(ct);

            return; 
        }
        catch (OperationCanceledException) { throw; } 
        catch (DomainLevelException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove a schedule slot {id}", id);
            throw new DomainLevelException("Failed to remove a schedule slot ", ex);
        }
    }
}
