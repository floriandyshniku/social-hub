using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace SocialHub.Domain.Users.Events
{
    public class UserFollowRequestRejectedEvent : IDomainEvent
    {
        public UserId TargetUserId { get; }    
        public UserId FollowerId { get; }     
        public DateTime OccurredOn { get; }

        public UserFollowRequestRejectedEvent(UserId targetUserId, UserId followerId)
        {
            TargetUserId = targetUserId;
            FollowerId = followerId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
