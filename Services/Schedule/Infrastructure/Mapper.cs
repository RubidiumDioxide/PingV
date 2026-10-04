namespace Schedule.Infrastructure; 

public static class Mapper
{
    public static ScheduleSlotDbModel DomainToDb(this ScheduleSlot slot) => new()
    {
        Id = slot.Id,
        CreatorId = slot.CreatorId,
        Start = slot.TimeRange.Start,
        End = slot.TimeRange.End,
        Note = slot.Note
    };

    public static ScheduleSlot? DbToDomain(this ScheduleSlotDbModel? slot)
        => (slot == null)? null : new(slot.Id, slot.CreatorId, new TimeRange(slot.Start, slot.End), slot.Note);


    public static Expression<Func<ScheduleSlotDbModel, ScheduleSlotDto>> ScheduleSlotDbToDto
       => slot => new(slot.Id, slot.CreatorId, slot.Creator.Login, slot.Start, slot.End, slot.Note);
}