using incident_management_system.API.Infrastructure;
using incident_management_system.API.Interfaces;
using incident_management_system.API.Models;

namespace incident_management_system.API.Services;

public class IncidentService : IIncidentService
{
    private readonly IncidentInMemoryDb _incidentInMemoryDb;

    public IncidentService(IncidentInMemoryDb incidentInMemoryDb)
    {
        _incidentInMemoryDb = incidentInMemoryDb;
    }

    public Task<Incident> CreateIncidentAsync(Incident incident)
    {
        _incidentInMemoryDb.Incidents.Add(incident);
        return Task.FromResult(incident);
    }

    public Task<bool> DeleteIncidentAsync(int id)
    {
        _incidentInMemoryDb.Incidents.RemoveAll(i => i.Id == id);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Incident>> GetAllIncidentsAsync()
    {
        return Task.FromResult(_incidentInMemoryDb.Incidents.AsEnumerable());
    }

    public Task<Incident?> GetIncidentByIdAsync(int id)
    {
        var incident = _incidentInMemoryDb.Incidents.FirstOrDefault(i => i.Id == id);
        return Task.FromResult(incident);
    }

    public Task<bool> UpdateIncidentAsync(int id, Incident updatedIncident)
    {
        var index = _incidentInMemoryDb.Incidents.FindIndex(i => i.Id == id);
        if (index == -1)
        {
            return Task.FromResult(false);
        }

        _incidentInMemoryDb.Incidents[index] = updatedIncident;
        return Task.FromResult(true);
    }
}
