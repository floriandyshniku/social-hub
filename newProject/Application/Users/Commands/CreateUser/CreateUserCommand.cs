using MediatR;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<UserId>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
} 