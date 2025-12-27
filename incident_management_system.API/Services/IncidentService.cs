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
        incident.Id = _incidentInMemoryDb.Incidents.Count > 0
            ? _incidentInMemoryDb.Incidents.Max(i => i.Id) + 1
            : 1;
        incident.CreatedAt = DateTime.UtcNow;

        _incidentInMemoryDb.Incidents.Add(incident);
        return Task.FromResult(incident);
    }

    public Task<bool> DeleteIncidentAsync(int id)
    {
        var incident = _incidentInMemoryDb.Incidents.FirstOrDefault(i => i.Id == id);
        if (incident is null)
        {
            return Task.FromResult(false);
        }

        _incidentInMemoryDb.Incidents.Remove(incident);
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

        var incident = _incidentInMemoryDb.Incidents[index];
        incident.Title = updatedIncident.Title;
        incident.Description = updatedIncident.Description;
        incident.Status = updatedIncident.Status;

        if (incident.Status == "Resolved") incident.ResolvedAt = DateTime.UtcNow;

        return Task.FromResult(true);
    }

    public Task<bool> UpdateIncidentStatusAsync(int id, string status)
    {
        var incident = _incidentInMemoryDb.Incidents.FirstOrDefault(i => i.Id == id);
        if (incident is null)
        {
            return Task.FromResult(false);
        }

        incident.Status = status;
        if (status == "Resolved") incident.ResolvedAt = DateTime.UtcNow;

        return Task.FromResult(true);
    }
}
