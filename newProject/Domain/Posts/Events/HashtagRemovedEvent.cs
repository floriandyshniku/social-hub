using newProject.Domain.Common;
using newProject.Domain.Posts.ValueObjects;

namespace newProject.Domain.Posts.Events;

public class HashtagRemovedEvent : IDomainEvent
{
    public PostId PostId { get; }
    public Hashtag Hashtag { get; }
    public DateTime OccurredOn { get; }

    public HashtagRemovedEvent(PostId postId, Hashtag hashtag)
    {
        PostId = postId;
        Hashtag = hashtag;
        OccurredOn = DateTime.UtcNow;
    }
} 