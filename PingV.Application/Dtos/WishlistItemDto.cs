namespace PingV.Application.Dtos;

public sealed record WishlistItemDto(Guid Id, Guid CreatorId, string CreatorLogin, string Title, string? Description); 