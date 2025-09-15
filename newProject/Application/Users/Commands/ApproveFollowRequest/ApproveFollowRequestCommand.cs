using MediatR;

namespace SocialHub.Application.Users.Commands.FollowRequest
{
    public record ApproveFollowRequestCommand(Guid FollowerId, Guid TargetUserId) : IRequest;

}
