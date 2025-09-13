using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users;

public interface IUserRepository
{
    // Basic CRUD operations
    Task<User?> GetByIdAsync(UserId userId);
    Task<User?> GetByUsernameAsync(Username username);
    Task<User?> GetByEmailAsync(Email email);
    Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<UserId> userIds);
    
    // Query operations - returns IQueryable for flexible database-level querying
    IQueryable<User> GetAll();
    IQueryable<User> GetUsersByUsernameAsync(string usernameSearch);
    
    // Persistence operations
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(UserId userId);
    
    // Existence checks
    Task<bool> ExistsAsync(UserId userId);
    Task<bool> ExistsByUsernameAsync(Username username);
    Task<bool> ExistsByEmailAsync(Email email);
} 