using newProject.Domain.Common;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Posts.Events;

public class PostLikedEvent : IDomainEvent
{
    public PostId PostId { get; }
    public UserId AuthorId { get; }
    public UserId LikedByUserId { get; }
    public DateTime OccurredOn { get; }

    public PostLikedEvent(PostId postId, UserId authorId, UserId likedByUserId)
    {
        PostId = postId;
        AuthorId = authorId;
        LikedByUserId = likedByUserId;
        OccurredOn = DateTime.UtcNow;
    }
} 