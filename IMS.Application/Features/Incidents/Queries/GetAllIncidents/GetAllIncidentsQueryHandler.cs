using IMS.Application.Interfaces.Persistance;
using MediatR;

namespace IMS.Application.Features.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsQueryHandler : IRequestHandler<GetAllIncidentsQuery, GetAllIncidentsResponse>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public GetAllIncidentsQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }

    public async Task<GetAllIncidentsResponse> Handle(GetAllIncidentsQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        int pageCount = request.PageCount < 1 ? 5 : request.PageCount;

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

        return new GetAllIncidentsResponse
        {
            PageNumber = pageNumber,
            PageCount = itemsCount,
            Incidents = incidentListItems
        };
    }
}