using incident_management_system.API.Enums;

namespace incident_management_system.API.Models;

public class Incident
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;

    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;

    public ICollection<IncidentComment> Comments { get; set; } = [];
}
