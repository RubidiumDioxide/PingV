namespace Schedule.Application.Services.Interfaces; 

public interface IScheduleQueryService 
{
    Task<IEnumerable<ScheduleSlotDto>> GetByMonthAsync(int year, int month, CancellationToken ct = default); 
}
