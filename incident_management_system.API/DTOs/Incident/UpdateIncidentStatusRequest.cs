using incident_management_system.API.Enums;

namespace incident_management_system.API.DTOs.Incident;

public class UpdateIncidentStatusRequest
{
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
}