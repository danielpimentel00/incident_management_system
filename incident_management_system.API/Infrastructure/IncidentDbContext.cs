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

            entity.HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedIncidents)
            .HasForeignKey(e => e.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
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

        modelBuilder.Entity<IncidentComment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd();

            entity.Property(e => e.Content)
            .IsRequired()
            .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
            .IsRequired();

            entity.HasOne(e => e.Incident)
            .WithMany(i => i.Comments)
            .HasForeignKey(e => e.IncidentId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
