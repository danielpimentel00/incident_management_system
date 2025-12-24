using incident_management_system.API.Models;

namespace incident_management_system.API.Infrastructure;

public class IncidentInMemoryDb
{
    public List<Incident> Incidents { get; } =
    [
        new Incident
        {
            Id = 1,
            Title = "Sample Incident 1",
            Description = "This is a sample incident.",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            Status = "Open"
        },
        new Incident
        {
            Id = 2,
            Title = "Sample Incident 2",
            Description = "This is another sample incident.",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            ResolvedAt = DateTime.UtcNow,
            Status = "Resolved"
        }
    ];
}
