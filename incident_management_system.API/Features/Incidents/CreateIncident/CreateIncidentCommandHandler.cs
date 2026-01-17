using incident_management_system.API.Infrastructure;
using incident_management_system.API.Models;
using MediatR;

namespace incident_management_system.API.Features.Incidents.CreateIncident;

public class CreateIncidentCommandHandler : IRequestHandler<CreateIncidentCommand, CreatedIncident>
{
    private readonly IncidentDbContext _dbContext;

    public CreateIncidentCommandHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreatedIncident> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        var newIncident = new Incident
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };
        var createdIncident = await _dbContext.Incidents.AddAsync(newIncident, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new CreatedIncident
        {
            Id = createdIncident.Entity.Id,
            Title = createdIncident.Entity.Title,
            Description = createdIncident.Entity.Description,
            ResolvedAt = createdIncident.Entity.ResolvedAt,
            Status = createdIncident.Entity.Status
        };

        return response;
    }
}
