using MediatR;
using newProject.Domain.Users;

namespace newProject.Application.Users.Queries.GetUser;

public class GetUserQuery : IRequest<User?>
{
    public Guid UserId { get; set; }
} 