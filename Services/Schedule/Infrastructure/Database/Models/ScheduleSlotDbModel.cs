namespace Schedule.Infrastructure.Database.Models; 

public class ScheduleSlotDbModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public string? Note { get; set; }

    public virtual UserDbModel Creator { get; set; } = default!;
}