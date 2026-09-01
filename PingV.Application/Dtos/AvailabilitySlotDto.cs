namespace PingV.Application.Dtos;

public sealed record AvailabilitySlotDto(Guid Id, Guid CreatorId, string CreatorLogin, DateTimeOffset Start, DateTimeOffset End, string? Note);
