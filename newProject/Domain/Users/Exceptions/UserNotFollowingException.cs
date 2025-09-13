using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Exceptions;

public class UserNotFollowingException : DomainException
{
    public UserNotFollowingException(UserId followerId, UserId targetId)
        : base($"User {followerId} is not following {targetId}") { }
} 