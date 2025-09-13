using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Events;

public class UserFollowedEvent : IDomainEvent
{
    public UserId FollowerId { get; }
    public UserId FollowedUserId { get; }
    public DateTime OccurredOn { get; }

    public UserFollowedEvent(UserId followerId, UserId followedUserId)
    {
        FollowerId = followerId;
        FollowedUserId = followedUserId;
        OccurredOn = DateTime.UtcNow;
    }
} 