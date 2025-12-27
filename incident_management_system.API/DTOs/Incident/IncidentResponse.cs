namespace incident_management_system.API.DTOs.Incident;

public class IncidentResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? ResolvedAt { get; set; }
    public string Status { get; set; } = "Open";
}