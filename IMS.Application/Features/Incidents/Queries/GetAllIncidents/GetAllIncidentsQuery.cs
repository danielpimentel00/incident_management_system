using MediatR;

namespace IMS.Application.Features.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsQuery : IRequest<GetAllIncidentsResponse>
{
    public int PageNumber { get; set; }
    public int PageCount { get; set; }

}
