using MediatR;
using newProject.Domain.Users;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new InvalidOperationException("User not found");

        if (!string.IsNullOrWhiteSpace(request.DisplayName))
        {
            user.UpdateProfile(request.DisplayName, request.Bio, request.ProfilePictureUrl);
        }

        await _userRepository.UpdateAsync(user);
    }
} 