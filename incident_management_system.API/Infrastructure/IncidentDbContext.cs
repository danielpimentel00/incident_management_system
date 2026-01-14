using incident_management_system.API.Models;
using Microsoft.EntityFrameworkCore;

namespace incident_management_system.API.Infrastructure;

public class IncidentDbContext : DbContext
{
    public IncidentDbContext(DbContextOptions<IncidentDbContext> options) : base(options)
    {
    }

    public DbSet<Incident> Incidents { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

            entity.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
            .IsRequired();

            entity.Property(e => e.ResolvedAt)
            .IsRequired(false);

            entity.Property(e => e.Status)
            .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd();

            entity.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(100);

            entity.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

            entity.Property(e => e.Role)
            .IsRequired();
        });
    }
}
