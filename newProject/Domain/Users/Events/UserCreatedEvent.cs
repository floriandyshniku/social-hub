using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Events;

public class UserCreatedEvent : IDomainEvent
{
    public UserId UserId { get; }
    public Username Username { get; }
    public Email Email { get; }
    public DateTime OccurredOn { get; }

    public UserCreatedEvent(UserId userId, Username username, Email email)
    {
        UserId = userId;
        Username = username;
        Email = email;
        OccurredOn = DateTime.UtcNow;
    }
} 