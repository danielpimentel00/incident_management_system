using IMS.Application.Interfaces.Persistance;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncidentStatus;

public class UpdateIncidentStatusCommandHandler : IRequestHandler<UpdateIncidentStatusCommand, bool>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public UpdateIncidentStatusCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }

    public async Task<bool> Handle(UpdateIncidentStatusCommand request, CancellationToken cancellationToken)
    {
        await _incidentsRepository.UpdateIncidentStatusAsync(request.Id, request.Status);

        return true;
    }
}
