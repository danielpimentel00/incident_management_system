using incident_management_system.API.Infrastructure;
using MediatR;

namespace incident_management_system.API.Features.Incidents.GetAllIncidents;

public class GetAllIncidentsQueryHandler : IRequestHandler<GetAllIncidentsQuery, List<IncidentListItem>>
{
    private readonly IncidentInMemoryDb _incidentInMemoryDb;

    public GetAllIncidentsQueryHandler(IncidentInMemoryDb incidentInMemoryDb)
    {
        _incidentInMemoryDb = incidentInMemoryDb;
    }

    public Task<List<IncidentListItem>> Handle(GetAllIncidentsQuery request, CancellationToken cancellationToken)
    {
        var items = _incidentInMemoryDb.Incidents.AsEnumerable();

        var response = new List<IncidentListItem>();
        foreach (var item in items)
        {
            response.Add(new IncidentListItem
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                ResolvedAt = item.ResolvedAt,
                Status = item.Status
            });
        }

        return Task.FromResult(response);
    }
}