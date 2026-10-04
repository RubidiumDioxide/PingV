namespace Schedule.Infrastructure.EntityTypeConfiguration; 

public class ScheduleSlotEntityTypeConfiguration : IEntityTypeConfiguration<ScheduleSlotDbModel>
{
    public void Configure(EntityTypeBuilder<ScheduleSlotDbModel> builder)
    {
        builder.ToTable("ScheduleSlot");

        builder.HasKey(slot => slot.Id);
        builder.Property(slot => slot.Id)
            .UseHiLo("schedule_slot_hilo")
            .IsRequired();
        builder.Property(slot => slot.CreatorId).IsRequired();
        builder.Property(slot => slot.Start).IsRequired();
        builder.Property(slot => slot.End).IsRequired();
        builder.Property(slot => slot.Note);

        builder.HasOne(slot => slot.Creator)
            .WithMany(user => user.CreatedScheduleSlots)
            .HasForeignKey(slot => slot.CreatorId);
    }
}
