using IMS.Application.Interfaces.Persistance;
using MediatR;

namespace IMS.Application.Features.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQueryHandler : IRequestHandler<GetIncidentByIdQuery, IncidentDetails?>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public GetIncidentByIdQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }

    public async Task<IncidentDetails?> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentByIdAsync(request.Id);

        if (incident == null)
        {
            return null;
        }

        var incidentDto = new IncidentDetails
        {
            Id = incident.Id,
            Title = incident.Title,
            Description = incident.Description,
            ResolvedAt = incident.ResolvedAt,
            Status = incident.Status,
            CreatedByUserId = incident.CreatedByUserId,
            CreatedByUserName = incident.CreatedByUser.Username,
            Comments = incident.Comments.Select(c => c.Content).ToList()
        };

        return incidentDto;
    }
}
