using MediatR;

namespace IMS.Application.Features.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommand(int id) : IRequest<bool>
{
    public int Id { get; } = id;
}
