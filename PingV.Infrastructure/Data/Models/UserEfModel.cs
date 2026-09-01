namespace PingV.Infrastructure.Data.Models;

public class UserEfModel
{
    public Guid Id { get; set; }
    public string Login { get; set; } = default!;

    public virtual IEnumerable<AvailabilitySlotEfModel> CreatedAvailibilitySlots { get; set; } = default!; 
}
