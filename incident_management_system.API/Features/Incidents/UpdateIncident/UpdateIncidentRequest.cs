using IMS.Domain.Enums;

namespace incident_management_system.API.Features.Incidents.UpdateIncident;

public class UpdateIncidentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
}
