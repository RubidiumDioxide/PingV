namespace Schedule.Infrastructure; 

public static class Mapper
{
    public static ScheduleSlotDbModel DomainToDb(this ScheduleSlot slot) => new()
    {
        Id = slot.Id,
        CreatorId = slot.CreatorId,
        Start = slot.Start,
        End = slot.End,
        Note = slot.Note
    };

    public static ScheduleSlot DbToDomain(this ScheduleSlotDbModel slot)
        => new(slot.Id, slot.CreatorId, slot.Start, slot.End, slot.Note);
    
    public static ScheduleSlotDto DbToDto(this ScheduleSlotDbModel slot)
       => new(slot.Id, slot.CreatorId, slot.Creator.Login, slot.Start, slot.End, slot.Note);

    public static Expression<Func<ScheduleSlotDbModel, ScheduleSlotDto>> DbToDtoExpression
       => slot => new(slot.Id, slot.CreatorId, slot.Creator.Login, slot.Start, slot.End, slot.Note);
}