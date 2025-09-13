using MediatR;

namespace newProject.Application.Users.Commands.FollowUser;

public class FollowUserCommand : IRequest
{
    public Guid FollowerId { get; set; }
    public Guid UserToFollowId { get; set; }
}
