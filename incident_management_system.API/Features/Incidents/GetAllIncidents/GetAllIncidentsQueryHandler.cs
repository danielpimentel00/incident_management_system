using incident_management_system.API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace incident_management_system.API.Features.Incidents.GetAllIncidents;

public class GetAllIncidentsQueryHandler : IRequestHandler<GetAllIncidentsQuery, GetAllIncidentsResponse>
{
    private readonly IncidentDbContext _dbContext;

    public GetAllIncidentsQueryHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAllIncidentsResponse> Handle(GetAllIncidentsQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        int pageCount = request.PageCount < 1 ? 5 : request.PageCount;
        int skip = (pageNumber - 1) * request.PageCount;

        var items = await _dbContext.Incidents
            .OrderByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Take(pageCount)
            .Select(x => new IncidentListItem
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ResolvedAt = x.ResolvedAt,
                Status = x.Status,
                CreatedByUserId = x.CreatedByUserId,
                CreatedByUserName = x.CreatedByUser.Username,
                Comments = x.Comments.Select(c => c.Content).ToList()
            })
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return new GetAllIncidentsResponse
        {
            PageNumber = pageNumber,
            PageCount = items.Count,
            Incidents = items
        };
    }
}