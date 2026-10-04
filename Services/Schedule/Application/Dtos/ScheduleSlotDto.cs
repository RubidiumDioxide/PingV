namespace Schedule.Application.Dtos;

public sealed record ScheduleSlotDto(Guid Id, Guid CreatorId, string CreatorLogin, DateTimeOffset Start, DateTimeOffset End, string? Note);