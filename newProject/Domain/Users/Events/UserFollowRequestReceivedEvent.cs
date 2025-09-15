using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace SocialHub.Domain.Users.Events
{
    public class UserFollowRequestReceivedEvent : IDomainEvent
    {
        public UserId TargetUserId { get; }   // The user who received the follow request
        public UserId FollowerId { get; }     // The user who sent the follow request
        public DateTime OccurredOn { get; }  

        public UserFollowRequestReceivedEvent(UserId targetUserId, UserId followerId)
        {
            TargetUserId = targetUserId;
            FollowerId = followerId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
