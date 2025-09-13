using newProject.Domain.Common;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;
using newProject.Domain.Posts.Events;

namespace newProject.Domain.Posts;

public class Post : AggregateRoot<PostId>
{
    public UserId AuthorId { get; private set; }
    public PostContent Content { get; private set; }
    public string? ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsPublished { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<Hashtag> _hashtags = new();
    private readonly List<UserId> _likes = new();
    private readonly List<Comment> _comments = new();

    public IReadOnlyCollection<Hashtag> Hashtags => _hashtags.AsReadOnly();
    public IReadOnlyCollection<UserId> Likes => _likes.AsReadOnly();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    private Post(PostId id, UserId authorId, PostContent content, string? imageUrl) : base(id)
    {
        AuthorId = authorId;
        Content = content;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;
        IsPublished = false;
        IsDeleted = false;
    }

    private Post() { }

    // Factory method - Factory Pattern
    public static Post Create(UserId authorId, PostContent content, string? imageUrl = null)
    {
        var post = new Post(PostId.Create(), authorId, content, imageUrl);
        post.AddDomainEvent(new PostCreatedEvent(post.Id, post.AuthorId, post.Content));
        return post;
    }

    // Domain methods
    public void Publish()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot publish a deleted post");

        if (IsPublished)
            throw new InvalidOperationException("Post is already published");

        IsPublished = true;
        AddDomainEvent(new PostPublishedEvent(Id, AuthorId, Content));
    }

    public void UpdateContent(PostContent newContent)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot update a deleted post");

        if (!IsPublished)
            throw new InvalidOperationException("Cannot update an unpublished post");

        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new PostContentUpdatedEvent(Id, AuthorId, Content));
    }

    public void AddHashtag(Hashtag hashtag)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot add hashtag to a deleted post");

        if (_hashtags.Count >= 10)
            throw new InvalidOperationException("Cannot add more than 10 hashtags to a post");

        if (!_hashtags.Contains(hashtag))
        {
            _hashtags.Add(hashtag);
            AddDomainEvent(new HashtagAddedEvent(Id, hashtag));
        }
    }

    public void RemoveHashtag(Hashtag hashtag)
    {
        if (_hashtags.Contains(hashtag))
        {
            _hashtags.Remove(hashtag);
            AddDomainEvent(new HashtagRemovedEvent(Id, hashtag));
        }
    }

    public void Like(UserId userId)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot like a deleted post");

        if (!IsPublished)
            throw new InvalidOperationException("Cannot like an unpublished post");

        if (_likes.Contains(userId))
            throw new InvalidOperationException("User has already liked this post");

        _likes.Add(userId);
        AddDomainEvent(new PostLikedEvent(Id, AuthorId, userId));
    }

    public void Unlike(UserId userId)
    {
        if (!_likes.Contains(userId))
            throw new InvalidOperationException("User has not liked this post");

        _likes.Remove(userId);
        AddDomainEvent(new PostUnlikedEvent(Id, AuthorId, userId));
    }

    public void AddComment(UserId authorId, string commentContent)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot add comment to a deleted post");

        if (!IsPublished)
            throw new InvalidOperationException("Cannot add comment to an unpublished post");

        var comment = Comment.Create(Id, authorId, commentContent);
        _comments.Add(comment);
        AddDomainEvent(new CommentAddedEvent(Id, comment.Id, authorId, commentContent));
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Post is already deleted");

        IsDeleted = true;
        AddDomainEvent(new PostDeletedEvent(Id, AuthorId));
    }

    // Business rules
    public bool IsLikedBy(UserId userId)
    {
        return _likes.Contains(userId);
    }

    public int GetLikesCount()
    {
        return _likes.Count;
    }

    public int GetCommentsCount()
    {
        return _comments.Count;
    }

    public bool HasHashtag(Hashtag hashtag)
    {
        return _hashtags.Contains(hashtag);
    }

    public bool CanBeEditedBy(UserId userId)
    {
        return AuthorId == userId && IsPublished && !IsDeleted;
    }

    public bool CanBeDeletedBy(UserId userId)
    {
        return AuthorId == userId && !IsDeleted;
    }
} 