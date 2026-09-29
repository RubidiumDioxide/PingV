namespace PingV.Application.Dtos;

public sealed record CreateWishlistItemRequest(Guid CreatorId, string Title, string? Description = null);
