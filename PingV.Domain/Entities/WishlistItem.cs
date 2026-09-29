namespace PingV.Domain.Entities; 

public class WishlistItem
{
    public Guid Id { get; private set; } 
    public Guid CreatorId { get; private set; }
    public string Title { get; private set; } = default!; 
    public string? Description { get; private set; }

    private WishlistItem() { } 

    public WishlistItem( 
        Guid id, 
        Guid creatorId, 
        string title, 
        string? description
    )
    {
        Id = id;
        CreatorId = creatorId; 
        Title = title; 
        Description = description; 
    }
}
