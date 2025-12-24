using incident_management_system.API.Models;

namespace incident_management_system.API.Interfaces;

public interface IIncidentService
{
    Task<IEnumerable<Incident>> GetAllIncidentsAsync();
    Task<Incident?> GetIncidentByIdAsync(int id);
    Task<Incident> CreateIncidentAsync(Incident incident);
    Task<bool> UpdateIncidentAsync(int id, Incident updatedIncident);
    Task<bool> DeleteIncidentAsync(int id);
}
