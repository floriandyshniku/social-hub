using Microsoft.EntityFrameworkCore;
using newProject.Domain.Posts;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;
using newProject.Infrastructure.Data;

namespace newProject.Infrastructure.Repositories;

/// <summary>
/// Repository for Post entities with IQueryable support for database-level querying.
/// 
/// Usage examples:
/// - var publishedPosts = await _postRepository.GetAllAsync().Where(p => p.IsPublished).ToListAsync();
/// - var userPosts = await _postRepository.GetAllAsync().Where(p => p.AuthorId == userId).ToListAsync();
/// - var count = await _postRepository.GetAllAsync().CountAsync();
/// </summary>
public class PostRepository : IPostRepository
{
    private readonly ApplicationDbContext _context;

    public PostRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Post?> GetByIdAsync(PostId postId)
    {
        return await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<IEnumerable<Post>> GetByIdsAsync(IEnumerable<PostId> postIds)
    {
        return await _context.Posts
            .Where(p => postIds.Contains(p.Id))
            .Include(p => p.Comments)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetByAuthorIdAsync(UserId authorId)
    {
        return await _context.Posts
            .Where(p => p.AuthorId == authorId)
            .Include(p => p.Comments)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public IQueryable<Post> GetAllAsync()
    {
        return _context.Posts
            .Include(p => p.Comments);
    }

    public async Task<IEnumerable<Post>> GetPublishedPostsAsync(int skip = 0, int take = 20)
    {
        return await _context.Posts
            .Where(p => p.IsPublished && !p.IsDeleted)
            .Include(p => p.Comments)
            .OrderByDescending(p => p.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetPostsByHashtagAsync(string hashtag, int skip = 0, int take = 20)
    {
        return await _context.Posts
            .Where(p => p.IsPublished && !p.IsDeleted && p.Hashtags.Any(h => h.Value.Contains(hashtag)))
            .Include(p => p.Comments)
            .OrderByDescending(p => p.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> SearchPostsAsync(string searchTerm, int skip = 0, int take = 20)
    {
        return await _context.Posts
            .Where(p => p.IsPublished && !p.IsDeleted && 
                       (p.Content.Value.Contains(searchTerm) || 
                        p.Hashtags.Any(h => h.Value.Contains(searchTerm))))
            .Include(p => p.Comments)
            .OrderByDescending(p => p.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task AddAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PostId postId)
    {
        var post = await GetByIdAsync(postId);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(PostId postId)
    {
        return await _context.Posts.AnyAsync(p => p.Id == postId);
    }

    public async Task<bool> ExistsByAuthorAndContentAsync(UserId authorId, string content)
    {
        return await _context.Posts.AnyAsync(p => p.AuthorId == authorId && p.Content.Value == content);
    }
} 