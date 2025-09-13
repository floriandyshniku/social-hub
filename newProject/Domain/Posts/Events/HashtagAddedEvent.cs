using newProject.Domain.Common;
using newProject.Domain.Posts.ValueObjects;

namespace newProject.Domain.Posts.Events;

public class HashtagAddedEvent : IDomainEvent
{
    public PostId PostId { get; }
    public Hashtag Hashtag { get; }
    public DateTime OccurredOn { get; }

    public HashtagAddedEvent(PostId postId, Hashtag hashtag)
    {
        PostId = postId;
        Hashtag = hashtag;
        OccurredOn = DateTime.UtcNow;
    }
} 