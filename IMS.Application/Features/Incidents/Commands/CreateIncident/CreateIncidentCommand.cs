using MediatR;

namespace IMS.Application.Features.Incidents.Commands.CreateIncident;

public class CreateIncidentCommand : IRequest<CreatedIncident>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
}
