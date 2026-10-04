namespace Schedule.Infrastructure.Database; 

public class ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : DbContext(options)
{
    public DbSet<UserDbModel> Users { get; set; }
    public DbSet<ScheduleSlotDbModel> ScheduleSlots { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new ScheduleSlotEntityTypeConfiguration());
    }
}
