using incident_management_system.API.Models;

namespace incident_management_system.API.Infrastructure;

public class UserInMemoryDb
{
    public List<User> Users { get; } =
    [
        new User
        {
            Id = 1,
            Username = "bossman24",
            Email = "bossman24@test-gmail.com",
            Role = "Agent"
        },
        new User 
        {
            Id = 2,
            Username = "marie00",
            Email = "marie00@test-gmail.com",
            Role = "Admin"
        }
    ];
}