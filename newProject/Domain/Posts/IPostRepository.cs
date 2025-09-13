using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Posts;

public interface IPostRepository
{
    // Basic CRUD operations
    Task<Post?> GetByIdAsync(PostId postId);
    Task<IEnumerable<Post>> GetByIdsAsync(IEnumerable<PostId> postIds);
    Task<IEnumerable<Post>> GetByAuthorIdAsync(UserId authorId);
    
    // Query operations - returns IQueryable for flexible database-level querying
    IQueryable<Post> GetAllAsync();
    Task<IEnumerable<Post>> GetPublishedPostsAsync(int skip = 0, int take = 20);
    Task<IEnumerable<Post>> GetPostsByHashtagAsync(string hashtag, int skip = 0, int take = 20);
    Task<IEnumerable<Post>> SearchPostsAsync(string searchTerm, int skip = 0, int take = 20);
    
    // Persistence operations
    Task AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(PostId postId);
    
    // Existence checks
    Task<bool> ExistsAsync(PostId postId);
    Task<bool> ExistsByAuthorAndContentAsync(UserId authorId, string content);
} 