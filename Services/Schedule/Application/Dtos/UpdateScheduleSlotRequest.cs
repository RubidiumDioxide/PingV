namespace Schedule.Application.Dtos;

public sealed record UpdateScheduleSlotRequest(DateTimeOffset Start, DateTimeOffset End, string? Note = null); 