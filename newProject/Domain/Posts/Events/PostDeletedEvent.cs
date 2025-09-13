using newProject.Domain.Common;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Posts.Events;

public class PostDeletedEvent : IDomainEvent
{
    public PostId PostId { get; }
    public UserId AuthorId { get; }
    public DateTime OccurredOn { get; }

    public PostDeletedEvent(PostId postId, UserId authorId)
    {
        PostId = postId;
        AuthorId = authorId;
        OccurredOn = DateTime.UtcNow;
    }
} 