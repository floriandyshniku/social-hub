using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Events;

public class UserProfileUpdatedEvent : IDomainEvent
{
    public UserId UserId { get; }
    public string DisplayName { get; }
    public string Bio { get; }
    public string ProfilePictureUrl { get; }
    public DateTime OccurredOn { get; }

    public UserProfileUpdatedEvent(UserId userId, string displayName, string bio, string profilePictureUrl)
    {
        UserId = userId;
        DisplayName = displayName;
        Bio = bio;
        ProfilePictureUrl = profilePictureUrl;
        OccurredOn = DateTime.UtcNow;
    }
} 