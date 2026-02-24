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
        var incident = await _incidentsRepository.GetIncidentByIdAsync(request.Id);
        
        if (incident == null)
            throw new KeyNotFoundException($"Incident with ID {request.Id} not found.");

        incident.UpdateStatus(request.Status);

        await _incidentsRepository.UpdateIncidentAsync(incident);

        return true;
    }
}
