using IMS.Application.Interfaces.Infrastructure;
using IMS.Application.Interfaces.Persistance;
using IMS.Application.Shared;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsQueryHandler : IRequestHandler<GetAllIncidentsQuery, GetAllIncidentsResponse>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly ICacheService _cache;

    public GetAllIncidentsQueryHandler(IIncidentsRepository incidentsRepository, ICacheService cache)
    {
        _incidentsRepository = incidentsRepository;
        _cache = cache;
    }

    public async Task<GetAllIncidentsResponse> Handle(GetAllIncidentsQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        int pageCount = request.PageCount < 1 ? 5 : request.PageCount;

        var cacheKey = CacheKeys.IncidentsList(pageNumber, pageCount);

        var cachedItems = await _cache.GetAsync<(List<IncidentListItem>, int)>(cacheKey);

        if (cachedItems.Item1 != null)
        {
            return new GetAllIncidentsResponse
            {
                PageNumber = pageNumber,
                PageCount = cachedItems.Item2,
                Incidents = cachedItems.Item1
            };
        }

        var (items, itemsCount) = await _incidentsRepository.GetAllIncidentsAsync(pageNumber, pageCount);

        var incidentListItems = items.Select(x => new IncidentListItem
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            ResolvedAt = x.ResolvedAt,
            Status = x.Status,
            CreatedByUserId = x.CreatedByUserId,
            CreatedByUserName = x.CreatedByUser.Username,
            Comments = x.Comments.Select(c => c.Content).ToList()
        }).ToList();

        await _cache.SetAsync(
            cacheKey,
            (incidentListItems, itemsCount),
            TimeSpan.FromMinutes(5));

        return new GetAllIncidentsResponse
        {
            PageNumber = pageNumber,
            PageCount = itemsCount,
            Incidents = incidentListItems
        };
    }
}