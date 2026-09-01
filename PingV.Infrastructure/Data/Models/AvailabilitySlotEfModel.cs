namespace PingV.Infrastructure.Data.Models;

public class AvailabilitySlotEfModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; } 
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public string? Note { get; set; }

    public virtual UserEfModel Creator { get; set; } = default!; 
}
