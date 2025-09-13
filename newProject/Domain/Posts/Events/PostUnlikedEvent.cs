using newProject.Domain.Common;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Posts.Events;

public class PostUnlikedEvent : IDomainEvent
{
    public PostId PostId { get; }
    public UserId AuthorId { get; }
    public UserId UnlikedByUserId { get; }
    public DateTime OccurredOn { get; }

    public PostUnlikedEvent(PostId postId, UserId authorId, UserId unlikedByUserId)
    {
        PostId = postId;
        AuthorId = authorId;
        UnlikedByUserId = unlikedByUserId;
        OccurredOn = DateTime.UtcNow;
    }
} 