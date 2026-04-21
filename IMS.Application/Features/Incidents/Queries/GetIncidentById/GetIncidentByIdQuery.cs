using MediatR;

namespace IMS.Application.Features.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQuery(int id) : IRequest<IncidentDetails?>
{
    public int Id { get; set; } = id;
}
