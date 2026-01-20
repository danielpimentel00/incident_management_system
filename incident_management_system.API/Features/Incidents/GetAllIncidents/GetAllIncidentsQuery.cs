using MediatR;

namespace incident_management_system.API.Features.Incidents.GetAllIncidents;

public class GetAllIncidentsQuery : IRequest<GetAllIncidentsResponse>
{
    public int PageNumber { get; set; } 
    public int PageCount { get; set; }

}
