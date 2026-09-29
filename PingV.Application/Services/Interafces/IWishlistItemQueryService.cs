using PingV.Application.Dtos;  


namespace PingV.Application.Services.Interafces; 

public interface IWishlistItemQueryService
{
    Task<IEnumerable<WishlistItemDto>> GetByCreatorId(Guid creatorId, CancellationToken ct = default); 
}
