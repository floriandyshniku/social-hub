using newProject.Domain.Common;
using newProject.Domain.Users.ValueObjects;
using newProject.Domain.Users.Events;

namespace newProject.Domain.Users;

public class User : AggregateRoot<UserId>
{
    public Username Username { get; private set; }
    public Email Email { get; private set; }
    public string DisplayName { get; private set; }
    public string Bio { get; private set; }
    public string ProfilePictureUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<UserId> _following = new();
    private readonly List<UserId> _followers = new();

    public IReadOnlyCollection<UserId> Following => _following.AsReadOnly();
    public IReadOnlyCollection<UserId> Followers => _followers.AsReadOnly();

    // Constructor for new users
    private User(UserId id, Username username, Email email, string displayName) : base(id)
    {
        Username = username;
        Email = email;
        DisplayName = displayName;
        Bio = string.Empty;
        ProfilePictureUrl = string.Empty;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    // Constructor for EF Core
    private User() { }

    // Factory method - Factory Pattern
    public static User Create(Username username, Email email, string displayName)
    {
        var user = new User(UserId.Create(), username, email, displayName);
        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Username, user.Email));
        return user;
    }

    // Domain methods
    public void UpdateProfile(string displayName, string bio, string profilePictureUrl)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty", nameof(displayName));

        DisplayName = displayName;
        Bio = bio ?? string.Empty;
        ProfilePictureUrl = profilePictureUrl ?? string.Empty;

        AddDomainEvent(new UserProfileUpdatedEvent(Id, DisplayName, Bio, ProfilePictureUrl));
    }

    public void FollowUser(UserId userToFollow)
    {
        if (userToFollow == Id)
            throw new InvalidOperationException("User cannot follow themselves");

        if (_following.Contains(userToFollow))
            throw new InvalidOperationException("User is already being followed");

        _following.Add(userToFollow);
        AddDomainEvent(new UserFollowedEvent(Id, userToFollow));
    }

    public void UnfollowUser(UserId userToUnfollow)
    {
        if (!_following.Contains(userToUnfollow))
            throw new InvalidOperationException("User is not being followed");

        _following.Remove(userToUnfollow);
        AddDomainEvent(new UserUnfollowedEvent(Id, userToUnfollow));
    }

    public void AddFollower(UserId followerId)
    {
        if (!_followers.Contains(followerId))
        {
            _followers.Add(followerId);
        }
    }

    public void RemoveFollower(UserId followerId)
    {
        _followers.Remove(followerId);
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        AddDomainEvent(new UserDeactivatedEvent(Id));
    }

    public void Activate()
    {
        IsActive = true;
        AddDomainEvent(new UserActivatedEvent(Id));
    }

    // Business rules
    public bool IsFollowing(UserId userId)
    {
        return _following.Contains(userId);
    }

    public bool IsFollowedBy(UserId userId)
    {
        return _followers.Contains(userId);
    }

    public int GetFollowersCount()
    {
        return _followers.Count;
    }

    public int GetFollowingCount()
    {
        return _following.Count;
    }
} 