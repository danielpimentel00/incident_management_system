using MediatR;

namespace incident_management_system.API.Features.Incidents.GetAllIncidents;

public class GetAllIncidentsQuery : IRequest<List<IncidentListItem>>
{
}
