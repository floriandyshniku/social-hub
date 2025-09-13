using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Exceptions;

public class UserInactiveException : DomainException
{
    public UserInactiveException(UserId userId)
        : base($"User {userId} is inactive and cannot perform this operation") { }
} 