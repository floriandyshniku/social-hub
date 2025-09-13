using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Events;

public class UserActivatedEvent : IDomainEvent
{
    public UserId UserId { get; }
    public DateTime OccurredOn { get; }

    public UserActivatedEvent(UserId userId)
    {
        UserId = userId;
        OccurredOn = DateTime.UtcNow;
    }
} 