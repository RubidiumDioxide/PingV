using PingV.Application.Dtos; 


namespace PingV.Application.Services.Interafces; 

public interface IWishlistItemCommandService
{
    Task AddAsync(CreateWishlistItemRequest request, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
