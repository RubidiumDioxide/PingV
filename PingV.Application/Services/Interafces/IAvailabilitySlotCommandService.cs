using PingV.Application.Dtos;


namespace PingV.Application.Services.Interafces;

public interface IAvailabilitySlotCommandService
{
    Task AddAsync(CreateAvailabilitySlotRequest request, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
