using MediatR;
using newProject.Domain.Users;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new InvalidOperationException("User not found");

        if (request.HardDelete)
        {
            await _userRepository.DeleteAsync(userId);
        }
        else
        {
            user.Deactivate();
            await _userRepository.UpdateAsync(user);
        }
    }
} 