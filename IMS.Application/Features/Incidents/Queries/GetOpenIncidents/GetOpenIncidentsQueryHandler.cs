using IMS.Application.Interfaces.Persistance;
using MediatR;

namespace IMS.Application.Features.Incidents.Queries.GetOpenIncidents;

public class GetOpenIncidentsQueryHandler : IRequestHandler<GetOpenIncidentsQuery, GetOpenIncidentsResponse>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public GetOpenIncidentsQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }

    public async Task<GetOpenIncidentsResponse> Handle(GetOpenIncidentsQuery request, CancellationToken cancellationToken)
    {
        var incidents = await _incidentsRepository.GetOpenIncidentsAsync();

        var openIncidentItems = incidents.Select(x => new OpenIncidentItem
        {
            Id = x.Id,
            Title = x.Title,
            CreatedAt = x.CreatedAt
        }).ToList();

        return new GetOpenIncidentsResponse
        {
            Incidents = openIncidentItems
        };
    }
}
