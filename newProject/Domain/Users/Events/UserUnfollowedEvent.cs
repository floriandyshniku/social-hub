using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Events;

public class UserUnfollowedEvent : IDomainEvent
{
    public UserId FollowerId { get; }
    public UserId UnfollowedUserId { get; }
    public DateTime OccurredOn { get; }

    public UserUnfollowedEvent(UserId followerId, UserId unfollowedUserId)
    {
        FollowerId = followerId;
        UnfollowedUserId = unfollowedUserId;
        OccurredOn = DateTime.UtcNow;
    }
} 