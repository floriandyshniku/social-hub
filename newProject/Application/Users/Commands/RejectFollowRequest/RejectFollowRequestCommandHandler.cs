using MediatR;
using newProject.Domain.Users.Exceptions;
using newProject.Domain.Users.ValueObjects;
using newProject.Infrastructure.Data;
using newProject.Infrastructure.RealTime;

namespace SocialHub.Application.Users.Commands.RejectFollowRequest
{
    public class RejectFollowRequestCommandHandler : IRequestHandler<RejectFollowRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public RejectFollowRequestCommandHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task Handle(RejectFollowRequestCommand request, CancellationToken cancellationToken)
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

            targetUser.RejectFollowRequest(followerId);

            await _notificationService.NotifyUserAsync(
                followerId.Value.ToString(),
                $"{targetUser.Username.Value} rejected your follow request.",
                "follow-rejected",
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
