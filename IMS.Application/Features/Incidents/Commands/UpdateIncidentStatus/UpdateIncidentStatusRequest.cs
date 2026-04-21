using IMS.Domain.Enums;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncidentStatus;

public class UpdateIncidentStatusRequest
{
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
}
