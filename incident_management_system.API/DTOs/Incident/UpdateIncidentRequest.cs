using incident_management_system.API.Enums;

namespace incident_management_system.API.DTOs.Incident;

public class UpdateIncidentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
}
