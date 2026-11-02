namespace Schedule.Application.Services.Interfaces; 

public interface IScheduleQueryService 
{
    Task<ScheduleSlotDto> GetByIdAsync(Guid id, CancellationToken ct = default); 

    Task<IEnumerable<ScheduleSlotDto>> GetByMonthAsync(int year, int month, CancellationToken ct = default); 
}
