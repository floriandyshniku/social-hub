using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Exceptions;

public class UserCannotFollowSelfException : DomainException
{
    public UserCannotFollowSelfException(UserId userId) 
        : base($"User {userId} cannot follow themselves") { }
} 