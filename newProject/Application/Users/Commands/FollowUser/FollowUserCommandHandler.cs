using MediatR;
using newProject.Domain.Users;
using newProject.Domain.Users.Exceptions;
using newProject.Domain.Users.Services;
using newProject.Domain.Users.ValueObjects;
using newProject.Infrastructure.Data;
using newProject.Infrastructure.RealTime;

namespace newProject.Application.Users.Commands.FollowUser;

public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserDomainService _userDomainService;
    private readonly INotificationService _notificationService;

    public FollowUserCommandHandler(
        IUnitOfWork unitOfWork, 
        IUserDomainService userDomainService,
        INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _userDomainService = userDomainService;
        _notificationService = notificationService;
    }

    public async Task Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        var followerId = UserId.Create(request.FollowerId);
        var currentUser = await _unitOfWork.Users.GetByIdAsync(followerId);

        if (currentUser is null)
        {
            throw new UserNotFoundException(followerId);
        }

        var userToFollowId = UserId.Create(request.UserToFollowId);
        var userToFollow = await _unitOfWork.Users.GetByIdAsync(userToFollowId);

        if (userToFollow is null)
        {
            throw new UserNotFoundException(userToFollowId);
        }

        var canUserFollow = await _userDomainService.CanUserFollowAsync(followerId, userToFollowId);

        if (canUserFollow)
        {
            currentUser.FollowUser(userToFollowId);
            userToFollow.AddFollower(followerId);

            // Send real-time notification to the user being followed
            await _notificationService.NotifyUserAsync(
                userToFollowId.Value.ToString(),
                $"{currentUser.Username.Value} started following you!",
                "follow",
                new
                {
                    FollowerId = currentUser.Id.Value,
                    FollowerUsername = currentUser.Username.Value,
                    FollowerDisplayName = currentUser.DisplayName
                });
        }

        // Use Unit of Work for transaction management
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
