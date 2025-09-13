using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Users.Services;

public interface IUserDomainService
{
    // Business rules that don't belong to a single user
    Task<bool> CanUserFollowAsync(UserId followerId, UserId userToFollowId);
    Task<bool> CanUserUnfollowAsync(UserId followerId, UserId userToUnfollowId);
    Task<bool> IsUsernameAvailableAsync(Username username);
    Task<bool> IsEmailAvailableAsync(Email email);
    
    // Complex business logic
    Task<IEnumerable<UserId>> GetRecommendedUsersToFollowAsync(UserId userId, int maxCount = 10);
    Task<int> GetMutualFollowersCountAsync(UserId user1Id, UserId user2Id);
} 