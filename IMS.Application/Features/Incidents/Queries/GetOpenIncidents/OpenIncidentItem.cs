namespace IMS.Application.Features.Incidents.Queries.GetOpenIncidents;

public class OpenIncidentItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
