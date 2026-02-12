using IMS.Domain.Enums;

namespace incident_management_system.API.Features.Incidents.GetIncidentById;

public class IncidentDetails
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? ResolvedAt { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
    public int CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public List<string> Comments { get; set; } = [];
}
