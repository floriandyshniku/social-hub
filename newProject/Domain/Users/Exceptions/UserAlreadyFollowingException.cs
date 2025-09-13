using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Exceptions;

public class UserAlreadyFollowingException : DomainException
{
    public UserAlreadyFollowingException(UserId followerId, UserId targetId)
        : base($"User {followerId} is already following {targetId}") { }
} 