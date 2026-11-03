namespace Schedule.Domain.Entities;

public class ScheduleSlot
{
    public Guid Id { get; private set; }
    public Guid CreatorId { get; private set; }
    public DateTimeOffset Start { get; private set; } = default!;
    public DateTimeOffset End { get; private set; } = default!;
    public string? Note { get; private set; }

    private ScheduleSlot() { }

    public ScheduleSlot(
        Guid id,
        Guid creatorId,
        DateTimeOffset start,
        DateTimeOffset end, 
        string? note = null
    )
    {
        if (start > end)
            throw new DomainLevelException("Start date/time cannot be later than end date/time.");
        
        Id = id;
        CreatorId = creatorId;
        Start = start; 
        End = end; 
        Note = note;
    }
}