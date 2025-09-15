using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace SocialHub.Domain.Users.Events
{
    public class UserFollowRequestAcceptedEvent : IDomainEvent
    {
        public UserId FollowerId { get; }
        public UserId TargetUserId { get; }
        public DateTime OccurredOn { get; }

        public UserFollowRequestAcceptedEvent(UserId followerId, UserId targetUserId)
        {
            FollowerId = followerId;
            TargetUserId = targetUserId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
