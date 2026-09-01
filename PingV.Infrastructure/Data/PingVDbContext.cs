using Microsoft.EntityFrameworkCore;
using PingV.Infrastructure.Data.Models;

namespace PingV.Infrastructure.Data;

public sealed class PingVDbContext(DbContextOptions<PingVDbContext> options) : DbContext(options)
{
    public DbSet<AvailabilitySlotEfModel> AvailabilitySlots { get; set; } = null!;
    public DbSet<UserEfModel> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailabilitySlotEfModel>(builder =>
        {
            builder.HasKey(slot => slot.Id);
            builder.Property(slot => slot.CreatorId).IsRequired(); 
            builder.Property(slot => slot.Start).IsRequired();
            builder.Property(slot => slot.End).IsRequired();
            builder.Property(slot => slot.Note);

            builder.HasOne(slot => slot.Creator)
                .WithMany(user => user.CreatedAvailibilitySlots)
                .HasForeignKey(slot => slot.CreatorId);
        }); 

        modelBuilder.Entity<UserEfModel>(builder =>
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Login).IsRequired(); 
        });
    }
}
