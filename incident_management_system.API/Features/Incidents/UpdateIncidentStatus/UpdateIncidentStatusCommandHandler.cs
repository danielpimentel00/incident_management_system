using incident_management_system.API.Enums;
using incident_management_system.API.Infrastructure;
using MediatR;

namespace incident_management_system.API.Features.Incidents.UpdateIncidentStatus;

public class UpdateIncidentStatusCommandHandler : IRequestHandler<UpdateIncidentStatusCommand, bool>
{
    private readonly IncidentInMemoryDb _incidentInMemoryDb;

    public UpdateIncidentStatusCommandHandler(IncidentInMemoryDb incidentInMemoryDb)
    {
        _incidentInMemoryDb = incidentInMemoryDb;
    }

    public Task<bool> Handle(UpdateIncidentStatusCommand request, CancellationToken cancellationToken)
    {
        var incident = _incidentInMemoryDb.Incidents.FirstOrDefault(i => i.Id == request.Id);
        if (incident is null)
        {
            return Task.FromResult(false);
        }

        incident.Status = request.Status;
        if (incident.Status == IncidentStatus.Resolved) incident.ResolvedAt = DateTime.UtcNow;

        return Task.FromResult(true);
    }
}
