using newProject.Domain.Users;
using newProject.Domain.Users.ValueObjects;
using newProject.Domain.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace newProject.Infrastructure.Services;

public class UserDomainService : IUserDomainService
{
    private readonly IUserRepository _userRepository;

    public UserDomainService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> CanUserFollowAsync(UserId followerId, UserId userToFollowId)
    {
        // Check if both users exist
        var follower = await _userRepository.GetByIdAsync(followerId);
        var userToFollow = await _userRepository.GetByIdAsync(userToFollowId);

        if (follower == null || userToFollow == null)
            return false;

        // Check if users are active
        if (!follower.IsActive || !userToFollow.IsActive)
            return false;

        // Check if already following
        if (follower.IsFollowing(userToFollowId))
            return false;

        return true;
    }

    public async Task<bool> CanUserUnfollowAsync(UserId followerId, UserId userToUnfollowId)
    {
        // Check if both users exist
        var follower = await _userRepository.GetByIdAsync(followerId);
        var userToUnfollow = await _userRepository.GetByIdAsync(userToUnfollowId);

        if (follower == null || userToUnfollow == null)
            return false;

        // Check if currently following
        if (!follower.IsFollowing(userToUnfollowId))
            return false;

        return true;
    }

    public async Task<bool> IsUsernameAvailableAsync(Username username)
    {
        return !await _userRepository.ExistsByUsernameAsync(username);
    }

    public async Task<bool> IsEmailAvailableAsync(Email email)
    {
        return !await _userRepository.ExistsByEmailAsync(email);
    }

    public async Task<IEnumerable<UserId>> GetRecommendedUsersToFollowAsync(UserId userId, int maxCount = 10)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return Enumerable.Empty<UserId>();

        // Simple recommendation: get users that the user's followers are following
        var allUsers = await _userRepository.GetAll()
            .Where(x => x.IsActive)
            .ToListAsync();

        var recommendations = new List<UserId>();

        foreach (var otherUser in allUsers)
        {
            if (otherUser.Id == userId) continue; // Don't recommend self
            if (user.IsFollowing(otherUser.Id)) continue; // Don't recommend already following

            // Count mutual connections
            var mutualConnections = user.Followers.Intersect(otherUser.Followers).Count();
            
            if (mutualConnections > 0)
            {
                recommendations.Add(otherUser.Id);
                if (recommendations.Count >= maxCount)
                    break;
            }
        }

        return recommendations;
    }

    public async Task<int> GetMutualFollowersCountAsync(UserId user1Id, UserId user2Id)
    {
        var user1 = await _userRepository.GetByIdAsync(user1Id);
        var user2 = await _userRepository.GetByIdAsync(user2Id);

        if (user1 == null || user2 == null)
            return 0;

        return user1.Followers.Intersect(user2.Followers).Count();
    }
} 