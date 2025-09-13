using MediatR;

namespace newProject.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public Guid UserId { get; set; }
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
} 