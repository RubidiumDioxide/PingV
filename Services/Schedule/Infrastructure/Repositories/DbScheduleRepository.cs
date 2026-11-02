namespace Schedule.Infrastructure.Repositories; 

public class DbScheduleRepository(
    ScheduleDbContext context
) : IScheduleRepository 
{
    private readonly ScheduleDbContext _context = context; 

    public async Task AddAsync(ScheduleSlot slot, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var newSlot = slot.DomainToDb();

        await _context.ScheduleSlots.AddAsync(newSlot, ct);
        await _context.SaveChangesAsync(ct);
    }
    
    public async Task UpdateAsync(ScheduleSlot updatedSlot, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var existingSlot = await _context.ScheduleSlots.FindAsync([updatedSlot.Id], ct) ?? throw new DomainLevelException("Schedule slot not found ");

        existingSlot.Start = updatedSlot.Start;
        existingSlot.End = updatedSlot.End;
        existingSlot.Note = updatedSlot.Note;

        await _context.SaveChangesAsync(ct);
    }

    public async Task<ScheduleSlot> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var slot = await _context.ScheduleSlots.FindAsync([id], ct) ?? throw new DomainLevelException("Schedule slot not found ");

        return slot.DbToDomain();
    }

    public async Task RemoveAsync(Guid id, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        await _context.ScheduleSlots
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync(ct);

        return;
    }
}
