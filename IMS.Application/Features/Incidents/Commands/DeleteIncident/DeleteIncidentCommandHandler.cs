using IMS.Application.Interfaces.Persistance;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommandHandler : IRequestHandler<DeleteIncidentCommand, bool>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public DeleteIncidentCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }

    public async Task<bool> Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
    {
        await _incidentsRepository.DeleteIncidentAsync(request.Id);

        return true;
    }
}