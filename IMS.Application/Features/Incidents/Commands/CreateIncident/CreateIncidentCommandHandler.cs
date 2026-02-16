using IMS.Application.Interfaces.Persistance;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.CreateIncident;

public class CreateIncidentCommandHandler : IRequestHandler<CreateIncidentCommand, CreatedIncident>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public CreateIncidentCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }

    public async Task<CreatedIncident> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        var newIncident = new Incident
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = request.CreatedByUserId
        };
        var createdIncident = await _incidentsRepository.CreateIncidentAsync(newIncident);

        var response = new CreatedIncident
        {
            Id = createdIncident.Id,
            Title = createdIncident.Title,
            Description = createdIncident.Description,
            ResolvedAt = createdIncident.ResolvedAt,
            Status = createdIncident.Status,
            CreatedByUserId = createdIncident.CreatedByUserId,
            CreatedByUserName = createdIncident.CreatedByUser.Username
        };

        return response;
    }
}