using Microsoft.EntityFrameworkCore;
using CalendarApp.Models;
using System.Text.Json;

namespace CalendarApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // AppUser Configuration
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Ignore(e => e.AvatarInitials);
        });

        // Appointment Configuration
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Ignore(e => e.Duration);

            // SQLite doesn't support collections natively, so we serialize Attendees to JSON
            entity.Property(e => e.Attendees)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
                );
        });
    }
}
