using MediatR;
using newProject.Domain.Users.Exceptions;
using newProject.Domain.Users.Services;
using newProject.Domain.Users.ValueObjects;
using newProject.Infrastructure.Data;
using newProject.Infrastructure.RealTime;

namespace SocialHub.Application.Users.Commands.FollowRequest
{
    public class ApproveFollowRequestCommandHandler : IRequestHandler<ApproveFollowRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public ApproveFollowRequestCommandHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task Handle(ApproveFollowRequestCommand request, CancellationToken cancellationToken)
        {
            var targetUserId = UserId.Create(request.TargetUserId);
            var targetUser = await _unitOfWork.Users.GetByIdAsync(targetUserId);

            if (targetUser is null)
                throw new UserNotFoundException(targetUserId);

            var followerId = UserId.Create(request.FollowerId);
            var follower = await _unitOfWork.Users.GetByIdAsync(followerId);

            if (follower is null)
                throw new UserNotFoundException(followerId);

            if (!targetUser.HasPendingFollowRequest(followerId))
                throw new InvalidOperationException("No follow request found.");

            targetUser.AcceptFollowRequest(followerId);
            follower.FollowUser(targetUserId);

            await _notificationService.NotifyUserAsync(
                followerId.Value.ToString(),
                $"{targetUser.Username.Value} accepted your follow request!",
                "follow-approved",
                new
                {
                    TargetId = targetUser.Id.Value,
                    TargetUsername = targetUser.Username.Value,
                    TargetDisplayName = targetUser.DisplayName
                });

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
