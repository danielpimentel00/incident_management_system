namespace incident_management_system.API.Features.Users.CreateUser;

public class CreatedUser
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
