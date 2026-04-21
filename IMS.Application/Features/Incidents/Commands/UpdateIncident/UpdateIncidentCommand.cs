using IMS.Domain.Enums;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
}
