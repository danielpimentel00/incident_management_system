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
            .Include(x => x.Comments)
            .Include(x => x.CreatedByUser)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        var response = new List<IncidentListItem>();
        foreach (var item in items)
        {
            response.Add(new IncidentListItem
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                ResolvedAt = item.ResolvedAt,
                Status = item.Status,
                CreatedByUserId = item.CreatedByUserId,
                CreatedByUserName = item.CreatedByUser.Username,
                Comments = item.Comments.Select(c => c.Content).ToList()
            });
        }

        return response;
    }
}