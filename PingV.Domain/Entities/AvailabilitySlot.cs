using PingV.Domain.ValueObjects;

namespace PingV.Domain.Entities; 

public class AvailabilitySlot
{
    public Guid Id { get; private set; } 
    public Guid CreatorId { get; private set; } 
    public TimeRange TimeRange { get; private set; } = default!; 
    public string? Note { get; private set; }

    private AvailabilitySlot() { } 

    public AvailabilitySlot(
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
