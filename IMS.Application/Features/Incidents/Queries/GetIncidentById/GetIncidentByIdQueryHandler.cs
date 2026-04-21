using IMS.Application.Interfaces.Infrastructure;
using IMS.Application.Interfaces.Persistance;
using IMS.Application.Shared;
using MediatR;

namespace IMS.Application.Features.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQueryHandler : IRequestHandler<GetIncidentByIdQuery, IncidentDetails?>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly ICacheService _cache;

    public GetIncidentByIdQueryHandler(IIncidentsRepository incidentsRepository, ICacheService cache)
    {
        _incidentsRepository = incidentsRepository;
        _cache = cache;
    }

    public async Task<IncidentDetails?> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.IncidentById(request.Id);

        var cachedIncident = await _cache.GetAsync<IncidentDetails>(cacheKey);

        if (cachedIncident != null)
        {
            return cachedIncident;
        }

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

        await _cache.SetAsync(
            cacheKey,
            incidentDto,
            TimeSpan.FromMinutes(5));

        return incidentDto;
    }
}
