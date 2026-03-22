using IMS.Application.Interfaces.Infrastructure;
using IMS.Application.Interfaces.Persistance;
using IMS.Application.Shared;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommandHandler : IRequestHandler<UpdateIncidentCommand, bool>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly ICacheService _cache;

    public UpdateIncidentCommandHandler(IIncidentsRepository incidentsRepository, ICacheService cache)
    {
        _incidentsRepository = incidentsRepository;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = new Incident
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status
        };

        await _incidentsRepository.UpdateIncidentAsync(incident);

        await _cache.RemoveAsync(CacheKeys.IncidentById(request.Id));
        await _cache.RemoveByPrefixAsync(CacheKeys.IncidentsListPrefix);
        await _cache.RemoveAsync(CacheKeys.OpenIncidents);

        return true;
    }
}