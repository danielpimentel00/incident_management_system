using incident_management_system.API.Infrastructure;
using MediatR;

namespace incident_management_system.API.Features.Incidents.GetIncidentById;

public class GetIncidentByIdQueryHandler : IRequestHandler<GetIncidentByIdQuery, IncidentDetails?>
{
    private readonly IncidentInMemoryDb _incidentInMemoryDb;

    public GetIncidentByIdQueryHandler(IncidentInMemoryDb incidentInMemoryDb)
    {
        _incidentInMemoryDb = incidentInMemoryDb;
    }

    public Task<IncidentDetails?> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        var incident = _incidentInMemoryDb.Incidents.FirstOrDefault(i => i.Id == request.Id);

        IncidentDetails? incidentDetails = null;
        if (incident is not null)
        {
            incidentDetails = new IncidentDetails
            {
                Id = incident.Id,
                Title = incident.Title,
                Description = incident.Description,
                ResolvedAt = incident.ResolvedAt,
                Status = incident.Status
            };
        }
        
        return Task.FromResult(incidentDetails);
    }
}
