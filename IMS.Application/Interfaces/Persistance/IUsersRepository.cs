using IMS.Domain.Entities;

namespace IMS.Application.Interfaces.Persistance;

public interface IUsersRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
}