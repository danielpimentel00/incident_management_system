using IMS.Application.Interfaces.Persistance;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommandHandler : IRequestHandler<UpdateIncidentCommand, bool>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public UpdateIncidentCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }

    public async Task<bool> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = new Incident
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status
        };

        await _incidentsRepository.UpdateIncidentAsync(incident);

        return true;
    }
}