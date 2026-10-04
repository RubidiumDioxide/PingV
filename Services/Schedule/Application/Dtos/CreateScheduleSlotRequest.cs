namespace Schedule.Application.Dtos;

public sealed record CreateScheduleSlotRequest(DateTimeOffset Start, DateTimeOffset End, string? Note = null);

