using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Events;

public class UserDeactivatedEvent : IDomainEvent
{
    public UserId UserId { get; }
    public DateTime OccurredOn { get; }

    public UserDeactivatedEvent(UserId userId)
    {
        UserId = userId;
        OccurredOn = DateTime.UtcNow;
    }
} 