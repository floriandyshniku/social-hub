using MediatR;

namespace newProject.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public Guid UserId { get; set; }
    public bool HardDelete { get; set; } = false;
} 