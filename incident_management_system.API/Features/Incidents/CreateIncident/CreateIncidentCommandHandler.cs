using incident_management_system.API.Infrastructure;
using incident_management_system.API.Models;
using MediatR;

namespace incident_management_system.API.Features.Incidents.CreateIncident;

public class CreateIncidentCommandHandler : IRequestHandler<CreateIncidentCommand, CreatedIncident>
{
    private readonly IncidentInMemoryDb _incidentInMemoryDb;

    public CreateIncidentCommandHandler(IncidentInMemoryDb incidentInMemoryDb)
    {
        _incidentInMemoryDb = incidentInMemoryDb;
    }

    public Task<CreatedIncident> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        var newIncidentId = _incidentInMemoryDb.Incidents.Count > 0
            ? _incidentInMemoryDb.Incidents.Max(i => i.Id) + 1
            : 1;
        var newIncident = new Incident
        {
            Id = newIncidentId,
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };
        _incidentInMemoryDb.Incidents.Add(newIncident);

        var createdIncident = new CreatedIncident
        {
            Id = newIncident.Id,
            Title = newIncident.Title,
            Description = newIncident.Description,
            ResolvedAt = newIncident.ResolvedAt,
            Status = newIncident.Status
        };

        return Task.FromResult(createdIncident);
    }
}
