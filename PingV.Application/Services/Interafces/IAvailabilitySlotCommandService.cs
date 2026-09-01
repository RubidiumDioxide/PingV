using PingV.Application.Dtos;

namespace PingV.Application.Services.Interafces;

public interface IAvailabilitySlotCommandService
{
    Task AddAvailabilitySlotAsync(CreateAvailabilitySlotRequest request, CancellationToken ct = default);

    Task DeleteAvailabilitySlotAsync(Guid id, CancellationToken ct = default);
}
