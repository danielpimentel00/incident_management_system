using MediatR;

namespace incident_management_system.API.Features.Incidents.DeleteIncident;

public class DeleteIncidentCommand(int id) : IRequest<bool>
{
    public int Id { get; } = id;
}
