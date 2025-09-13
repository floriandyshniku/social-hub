using newProject.Domain.Users;
using newProject.Domain.Posts;

namespace newProject.Infrastructure.Data;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IPostRepository Posts { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
} 