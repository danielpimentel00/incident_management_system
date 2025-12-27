using incident_management_system.API.Infrastructure;
using incident_management_system.API.Interfaces;
using incident_management_system.API.Models;

namespace incident_management_system.API.Services;

public class UserService : IUserService
{
    private readonly UserInMemoryDb _userInMemoryDb;

    public UserService(UserInMemoryDb userInMemoryDb)
    {
        _userInMemoryDb = userInMemoryDb;
    }

    public Task<User> CreateUserAsync(User user)
    {
        _userInMemoryDb.Users.Add(user);
        return Task.FromResult(user);
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return Task.FromResult(_userInMemoryDb.Users.AsEnumerable());
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        var user = _userInMemoryDb.Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }
}