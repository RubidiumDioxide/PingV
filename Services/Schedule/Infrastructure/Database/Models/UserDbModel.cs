namespace Schedule.Infrastructure.Database.Models; 

public class UserDbModel
{ 
    public Guid Id { get; set; }
    public string Login { get; set; } = default!;

    public virtual IEnumerable<ScheduleSlotDbModel> CreatedScheduleSlots { get; set; } = default!; 
}