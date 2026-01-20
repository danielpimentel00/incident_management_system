namespace incident_management_system.API.Features.Incidents.GetAllIncidents;

public class GetAllIncidentsResponse
{
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
    public List<IncidentListItem> Incidents { get; set; } = [];
}
