using PingV.Domain.Entities;

namespace PingV.Domain.Interfaces; 

public interface IAvailabilitySlotRepository
{
    Task AddAsync(AvailabilitySlot slot, CancellationToken ct = default);

    Task<IEnumerable<AvailabilitySlot>> GetSlotsByMonthAsync(Guid userId, int year, int month, CancellationToken ct = default);

    Task<AvailabilitySlot> DeleteAsync(Guid id, CancellationToken ct = default);
}
