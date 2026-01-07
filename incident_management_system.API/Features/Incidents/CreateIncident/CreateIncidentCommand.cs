using MediatR;

namespace incident_management_system.API.Features.Incidents.CreateIncident;

public class CreateIncidentCommand : IRequest<CreatedIncident>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
