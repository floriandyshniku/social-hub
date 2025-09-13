using Microsoft.EntityFrameworkCore;
using newProject.Domain.Users;
using newProject.Domain.Users.ValueObjects;
using newProject.Infrastructure.Data;

namespace newProject.Infrastructure.Repositories;

/// <summary>
/// Repository for User entities with IQueryable support for database-level querying.
/// 
/// Usage examples:
/// - var activeUsers = await _userRepository.GetAllAsync().Where(u => u.IsActive).ToListAsync();
/// - var paginatedUsers = await _userRepository.GetAllAsync().Skip(10).Take(5).ToListAsync();
/// - var count = await _userRepository.GetAllAsync().CountAsync();
/// - var searchResults = await _userRepository.GetUsersByUsernameAsync("john").Take(10).ToListAsync();
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(UserId userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetByUsernameAsync(Username username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(Email email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<UserId> userIds)
    {
        return await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
    }

    public IQueryable<User> GetAll()
    {
        return _context.Users.AsQueryable();
    }

    public IQueryable<User> GetUsersByUsernameAsync(string usernameSearch)
    {
        return _context.Users
            .Where(u => u.Username.Value.Contains(usernameSearch));
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserId userId)
    {
        var user = await GetByIdAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(UserId userId)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<bool> ExistsByUsernameAsync(Username username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
} 