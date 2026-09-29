using PingV.Domain.Entities;


namespace PingV.Domain.Interfaces; 

public interface IWishlistItemRepository
{
    Task AddAsync(WishlistItem item, CancellationToken ct = default);

    Task<IEnumerable<WishlistItem>> GetByCreatorIdAsync(Guid creatorId, CancellationToken ct = default);

    Task<WishlistItem> DeleteAsync(Guid id, CancellationToken ct = default);
}
