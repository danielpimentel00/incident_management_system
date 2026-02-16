namespace IMS.Application.Features.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsResponse
{
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
    public List<IncidentListItem> Incidents { get; set; } = [];
}
