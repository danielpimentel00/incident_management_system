using IMS.Application.Interfaces.Persistance;
using IMS.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IMS.Persistance.Repositories;

public class IncidentsRepository : IIncidentsRepository
{
    private readonly IncidentDbContext _context;

    public IncidentsRepository(IncidentDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Domain.Entities.Incident>, int)> GetAllIncidentsAsync(int pageNumber, int pageCount)
    {
        var totalCount = await _context.Incidents.CountAsync();

        var incidents = await _context.Incidents
            .Include(i => i.CreatedByUser)
            .Include(i => i.Comments)
                .ThenInclude(c => c.User)
            .OrderByDescending(i => i.CreatedAt)
            .Skip((pageNumber - 1) * pageCount)
            .Take(pageCount)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();

        return (incidents, totalCount);
    }

    public async Task<List<Domain.Entities.Incident>> GetOpenIncidentsAsync()
    {
        return await _context.Incidents
            .Where(i => i.Status == Domain.Enums.IncidentStatus.Open)
            .OrderByDescending(i => i.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Domain.Entities.Incident?> GetIncidentByIdAsync(int id)
    {
        return await _context.Incidents
            .Include(i => i.CreatedByUser)
            .Include(i => i.Comments)
                .ThenInclude(c => c.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
    }
    
    public async Task<Domain.Entities.Incident> CreateIncidentAsync(Domain.Entities.Incident incident)
    {
        await _context.Incidents.AddAsync(incident);
        await _context.SaveChangesAsync();

        await _context.Entry(incident)
            .Reference(i => i.CreatedByUser)
            .LoadAsync();

        return incident;
    }
    
    public async Task<Domain.Entities.Incident> UpdateIncidentAsync(Domain.Entities.Incident incident)
    {
        var existingIncident = await _context.Incidents
            .FirstOrDefaultAsync(i => i.Id == incident.Id);

        if (existingIncident == null)
        {
            throw new KeyNotFoundException($"Incident with ID {incident.Id} not found.");
        }

        existingIncident.Title = incident.Title;
        existingIncident.Description = incident.Description;
        existingIncident.Status = incident.Status;
        existingIncident.ResolvedAt = incident.ResolvedAt;

        _context.Incidents.Update(existingIncident);
        await _context.SaveChangesAsync();

        return existingIncident;
    }
    
    public async Task UpdateIncidentStatusAsync(int id, Domain.Enums.IncidentStatus status)
    {
        var incident = await _context.Incidents.FindAsync(id);

        if (incident == null)
        {
            throw new KeyNotFoundException($"Incident with ID {id} not found.");
        }

        incident.UpdateStatus(status);

        _context.Incidents.Update(incident);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteIncidentAsync(int id)
    {
        var incident = await _context.Incidents.FindAsync(id);

        if (incident == null)
        {
            throw new KeyNotFoundException($"Incident with ID {id} not found.");
        }

        _context.Incidents.Remove(incident);
        await _context.SaveChangesAsync();
    }
}
