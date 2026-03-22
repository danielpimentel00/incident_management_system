using IMS.Application.Interfaces.Infrastructure;
using IMS.Application.Interfaces.Persistance;
using IMS.Application.Shared;
using MediatR;

namespace IMS.Application.Features.Incidents.Queries.GetOpenIncidents;

public class GetOpenIncidentsQueryHandler : IRequestHandler<GetOpenIncidentsQuery, GetOpenIncidentsResponse>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly ICacheService _cache;

    public GetOpenIncidentsQueryHandler(IIncidentsRepository incidentsRepository, ICacheService cache)
    {
        _incidentsRepository = incidentsRepository;
        _cache = cache;
    }

    public async Task<GetOpenIncidentsResponse> Handle(GetOpenIncidentsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.OpenIncidents;

        var cachedIncidents = await _cache.GetAsync<List<OpenIncidentItem>>(cacheKey);

        if (cachedIncidents != null)
        {
            return new GetOpenIncidentsResponse
            {
                Incidents = cachedIncidents
            };
        }

        var incidents = await _incidentsRepository.GetOpenIncidentsAsync();

        var openIncidentItems = incidents.Select(x => new OpenIncidentItem
        {
            Id = x.Id,
            Title = x.Title,
            CreatedAt = x.CreatedAt
        }).ToList();

        await _cache.SetAsync(
            cacheKey, 
            openIncidentItems, 
            TimeSpan.FromMinutes(5));

        return new GetOpenIncidentsResponse
        {
            Incidents = openIncidentItems
        };
    }
}
