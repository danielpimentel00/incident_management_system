using IMS.Application.Interfaces.Persistance;
using IMS.Domain.Entities;
using IMS.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IMS.Persistance.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IncidentDbContext _context;

    public UsersRepository(IncidentDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
