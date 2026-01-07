using MediatR;

namespace incident_management_system.API.Features.Incidents.GetIncidentById;

public class GetIncidentByIdQuery(int id) : IRequest<IncidentDetails?>
{
    public int Id { get; set; } = id;
}
