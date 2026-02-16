using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IMS.Persistance.Context;

public class IncidentDbContextFactory : IDesignTimeDbContextFactory<IncidentDbContext>
{
    public IncidentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IncidentDbContext>();

        // Hardcoded connection string for local migrations. In production, connection string should be provided via configuration.
        var connectionString = "Host=localhost;Port=5432;Database=incident_management;Username=incident_user;Password=incident_user";
        
        optionsBuilder.UseNpgsql(connectionString);

        return new IncidentDbContext(optionsBuilder.Options);
    }
}