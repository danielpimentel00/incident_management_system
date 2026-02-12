using IMS.Domain.Enums;

namespace incident_management_system.API.Features.Incidents.UpdateIncidentStatus;

public class UpdateIncidentStatusRequest
{
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
}
