namespace PingV.Infrastructure.Data.Models;

public class WishlistItemEfModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public virtual UserEfModel Creator { get; set; } = default!; 
}
