namespace IMS.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Agent";

    public ICollection<Incident> CreatedIncidents { get; set; } = [];
    public ICollection<IncidentComment> Comments { get; set; } = [];
}