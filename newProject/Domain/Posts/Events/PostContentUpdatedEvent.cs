using newProject.Domain.Common;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Posts.Events;

public class PostContentUpdatedEvent : IDomainEvent
{
    public PostId PostId { get; }
    public UserId AuthorId { get; }
    public PostContent Content { get; }
    public DateTime OccurredOn { get; }

    public PostContentUpdatedEvent(PostId postId, UserId authorId, PostContent content)
    {
        PostId = postId;
        AuthorId = authorId;
        Content = content;
        OccurredOn = DateTime.UtcNow;
    }
} 