using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace incident_management_system.API.Infrastructure;

public class IncidentDbContextFactory : IDesignTimeDbContextFactory<IncidentDbContext>
{
    public IncidentDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        var optionsBuilder = new DbContextOptionsBuilder<IncidentDbContext>();
        var connectionString = configuration.GetConnectionString("PostgresDB");

        optionsBuilder.UseNpgsql(connectionString);

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        return new IncidentDbContext(optionsBuilder.Options);
    }
}