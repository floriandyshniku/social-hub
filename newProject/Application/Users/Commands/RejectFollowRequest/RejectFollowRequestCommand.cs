using MediatR;

namespace SocialHub.Application.Users.Commands.RejectFollowRequest
{
    public record RejectFollowRequestCommand(Guid TargetUserId,  Guid FollowerId) : IRequest;
}
