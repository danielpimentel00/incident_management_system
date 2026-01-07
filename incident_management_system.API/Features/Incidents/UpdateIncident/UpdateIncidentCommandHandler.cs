using incident_management_system.API.Enums;
using incident_management_system.API.Infrastructure;
using MediatR;

namespace incident_management_system.API.Features.Incidents.UpdateIncident;

public class UpdateIncidentCommandHandler : IRequestHandler<UpdateIncidentCommand, bool>
{
    private readonly IncidentInMemoryDb _incidentInMemoryDb;

    public UpdateIncidentCommandHandler(IncidentInMemoryDb incidentInMemoryDb)
    {
        _incidentInMemoryDb = incidentInMemoryDb;
    }

    public Task<bool> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
    {
        var index = _incidentInMemoryDb.Incidents.FindIndex(i => i.Id == request.Id);
        if (index == -1)
        {
            return Task.FromResult(false);
        }

        var incident = _incidentInMemoryDb.Incidents[index];
        incident.Title = request.Title;
        incident.Description = request.Description;
        incident.Status = request.Status;

        if (incident.Status == IncidentStatus.Resolved) incident.ResolvedAt = DateTime.UtcNow;

        return Task.FromResult(true);
    }
}
