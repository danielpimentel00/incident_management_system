using IMS.Application.Interfaces.Infrastructure;
using IMS.Application.Interfaces.Persistance;
using IMS.Application.Shared;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommandHandler : IRequestHandler<DeleteIncidentCommand, bool>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly ICacheService _cache;

    public DeleteIncidentCommandHandler(IIncidentsRepository incidentsRepository, ICacheService cache)
    {
        _incidentsRepository = incidentsRepository;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
    {
        await _incidentsRepository.DeleteIncidentAsync(request.Id);

        await _cache.RemoveAsync(CacheKeys.IncidentById(request.Id));
        await _cache.RemoveByPrefixAsync(CacheKeys.IncidentsListPrefix);
        await _cache.RemoveAsync(CacheKeys.OpenIncidents);

        return true;
    }
}