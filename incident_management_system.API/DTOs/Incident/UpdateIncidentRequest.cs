namespace incident_management_system.API.DTOs.Incident;

public class UpdateIncidentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
}
