using IMS.Domain.Enums;
using MediatR;

namespace incident_management_system.API.Features.Incidents.UpdateIncidentStatus;

public class UpdateIncidentStatusCommand : IRequest<bool>
{
    public int Id { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
}
