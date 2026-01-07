using incident_management_system.API.Infrastructure;
using MediatR;

namespace incident_management_system.API.Features.Incidents.DeleteIncident;

public class DeleteIncidentCommandHandler : IRequestHandler<DeleteIncidentCommand, bool>
{
    private readonly IncidentInMemoryDb _incidentInMemoryDb;

    public DeleteIncidentCommandHandler(IncidentInMemoryDb incidentInMemoryDb)
    {
        _incidentInMemoryDb = incidentInMemoryDb;
    }

    public Task<bool> Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = _incidentInMemoryDb.Incidents.FirstOrDefault(i => i.Id == request.Id);
        if (incident is null)
        {
            return Task.FromResult(false);
        }

        _incidentInMemoryDb.Incidents.Remove(incident);
        return Task.FromResult(true);
    }
}
