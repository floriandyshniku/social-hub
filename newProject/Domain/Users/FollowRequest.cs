using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;
using SocialHub.Domain.Users.ValueObjects;

public class FollowRequest : AggregateRoot<FollowRequestId>
{
    public UserId FromUserId { get; private set; }   // who sent the request
    public UserId UserId { get; private set; }       // who will receive it
    public DateTime RequestedAt { get; private set; }
    public bool IsAccepted { get; private set; }
    public bool IsRejected { get; private set; }

    private FollowRequest() { } // EF Core

    // Constructor for creating a new follow request
    public FollowRequest(UserId fromUserId, UserId userId)
    {
        if (fromUserId == userId)
            throw new InvalidOperationException("Cannot follow yourself");

        Id = FollowRequestId.Create();
        FromUserId = fromUserId;
        UserId = userId;
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
