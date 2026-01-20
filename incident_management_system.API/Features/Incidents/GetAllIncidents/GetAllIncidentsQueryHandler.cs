using incident_management_system.API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace incident_management_system.API.Features.Incidents.GetAllIncidents;

public class GetAllIncidentsQueryHandler : IRequestHandler<GetAllIncidentsQuery, List<IncidentListItem>>
{
    private readonly IncidentDbContext _dbContext;

    public GetAllIncidentsQueryHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<IncidentListItem>> Handle(GetAllIncidentsQuery request, CancellationToken cancellationToken)
    {
        var items = await _dbContext.Incidents
            .AsNoTracking()
            .AsSplitQuery()
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
            .ToListAsync(cancellationToken);

        return items;
    }
}