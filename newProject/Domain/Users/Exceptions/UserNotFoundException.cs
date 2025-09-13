using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Exceptions;

public class UserNotFoundException : DomainException
{
    public UserNotFoundException(UserId userId)
        : base($"User {userId} not found") { }
} 