using PingV.Application.Dtos;

namespace PingV.Application.Services.Interafces; 

public interface IAvailabilitySlotQueryService
{
    Task<IEnumerable<AvailabilitySlotDto>> GetMonthlyScheduleAsync(Guid userId, int year, int month, CancellationToken ct = default);
}
