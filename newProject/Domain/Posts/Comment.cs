using newProject.Domain.Common;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Posts;

public class Comment : Entity<CommentId>
{
    public PostId PostId { get; private set; }
    public UserId AuthorId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<UserId> _likes = new();

    public IReadOnlyCollection<UserId> Likes => _likes.AsReadOnly();

    private Comment(CommentId id, PostId postId, UserId authorId, string content) : base(id)
    {
        PostId = postId;
        AuthorId = authorId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    private Comment() { }

    public static Comment Create(PostId postId, UserId authorId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Comment content cannot be empty", nameof(content));

        if (content.Length > 500)
            throw new ArgumentException("Comment content cannot exceed 500 characters", nameof(content));

        return new Comment(CommentId.Create(), postId, authorId, content.Trim());
    }

    public void UpdateContent(string newContent)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot update a deleted comment");

        if (string.IsNullOrWhiteSpace(newContent))
            throw new ArgumentException("Comment content cannot be empty", nameof(newContent));

        if (newContent.Length > 500)
            throw new ArgumentException("Comment content cannot exceed 500 characters", nameof(newContent));

        Content = newContent.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Like(UserId userId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot like a deleted comment");

        if (_likes.Contains(userId))
            throw new InvalidOperationException("User has already liked this comment");

        _likes.Add(userId);
    }

    public void Unlike(UserId userId)
    {
        if (!_likes.Contains(userId))
            throw new InvalidOperationException("User has not liked this comment");

        _likes.Remove(userId);
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Comment is already deleted");

        IsDeleted = true;
    }

    public bool IsLikedBy(UserId userId)
    {
        return _likes.Contains(userId);
    }

    public int GetLikesCount()
    {
        return _likes.Count;
    }

    public bool CanBeEditedBy(UserId userId)
    {
        return AuthorId == userId && !IsDeleted;
    }

    public bool CanBeDeletedBy(UserId userId)
    {
        return AuthorId == userId && !IsDeleted;
    }
} 