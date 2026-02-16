using IMS.Domain.Entities;
using IMS.Domain.Enums;

namespace IMS.Application.Interfaces.Persistance;

public interface IIncidentsRepository
{
    Task<(List<Incident>, int)> GetAllIncidentsAsync(int pageNumber, int pageCount);
    Task<Incident?> GetIncidentByIdAsync(int id);
    Task<Incident> CreateIncidentAsync(Incident incident);
    Task<Incident> UpdateIncidentAsync(Incident incident);
    Task UpdateIncidentStatusAsync(int id, IncidentStatus status);
    Task DeleteIncidentAsync(int id);
}
