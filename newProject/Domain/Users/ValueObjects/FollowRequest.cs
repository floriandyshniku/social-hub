using newProject.Domain.Users.ValueObjects;

namespace SocialHub.Domain.Users.ValueObjects
{
    public class FollowRequest
    {
        public UserId FromUserId { get; private set; }
        public DateTime RequestedAt { get; private set; }
        public bool IsAccepted { get; private set; }
        public bool IsRejected { get; private set; }

        private FollowRequest() { } // EF Core

        public FollowRequest(UserId fromUserId)
        {
            FromUserId = fromUserId;
            RequestedAt = DateTime.UtcNow;
        }

        public void Accept()
        {
            if (IsRejected) throw new InvalidOperationException("Request already rejected");
            IsAccepted = true;
        }

        public void Reject()
        {
            if (IsAccepted) throw new InvalidOperationException("Request already accepted");
            IsRejected = true;
        }
    }

}
