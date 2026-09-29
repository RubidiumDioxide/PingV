using PingV.Application.Dtos;


namespace PingV.Application.Services.Interafces; 

public interface IAvailabilitySlotQueryService
{
    Task<IEnumerable<AvailabilitySlotDto>> GetByCreatorIdByMonthAsync(Guid creatorId, int year, int month, CancellationToken ct = default);
}
