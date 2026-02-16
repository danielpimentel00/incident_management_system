using IMS.Domain.Enums;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncidentStatus;

public class UpdateIncidentStatusCommand : IRequest<bool>
{
    public int Id { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
}
