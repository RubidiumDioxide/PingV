namespace Schedule.Infrastructure.EntityTypeConfiguration; 

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserDbModel>
{
    public void Configure(EntityTypeBuilder<UserDbModel> builder)
    {
        builder.ToTable("User");

        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id)
            .UseHiLo("schedule_slot_hilo")
            .IsRequired();
        builder.Property(user => user.Login).IsRequired(); 
    }
}
