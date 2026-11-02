namespace Schedule.Application.Services.Interfaces; 

public interface IScheduleCommandService
{
    Task CreateAsync(CreateScheduleSlotRequest request, CancellationToken ct = default);

    Task UpdateAsync(Guid id, UpdateScheduleSlotRequest request, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
