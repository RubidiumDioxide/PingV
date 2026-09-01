namespace PingV.Application.Dtos;

public sealed record CreateAvailabilitySlotRequest(Guid CreatorId, DateTimeOffset Start, DateTimeOffset End, string? Note = null);
