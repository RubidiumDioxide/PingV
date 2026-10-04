namespace Schedule.Domain.Entities;

public class ScheduleSlot
{
    public Guid Id { get; private set; }
    public Guid CreatorId { get; private set; }
    public TimeRange TimeRange { get; private set; } = default!;
    public string? Note { get; private set; }

    private ScheduleSlot() { }

    public ScheduleSlot(
        Guid id,
        Guid creatorId,
        TimeRange timeRange,
        string? note = null
    )
    {
        Id = id;
        CreatorId = creatorId;
        TimeRange = timeRange;
        Note = note;
    }
}