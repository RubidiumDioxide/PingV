namespace Schedule.Domain.Repositories.Interfaces; 

public interface IScheduleRepository
{
    Task AddAsync(ScheduleSlot slot, CancellationToken ct = default);

    Task<ScheduleSlot?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task RemoveAsync(Guid id, CancellationToken ct = default); 
}
