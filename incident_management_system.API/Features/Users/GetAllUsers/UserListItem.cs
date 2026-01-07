namespace incident_management_system.API.Features.Users.GetAllUsers;

public class UserListItem
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
