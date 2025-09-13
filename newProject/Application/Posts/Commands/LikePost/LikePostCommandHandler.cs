using MediatR;
using newProject.Domain.Posts;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;
using newProject.Infrastructure.RealTime;

namespace newProject.Application.Posts.Commands.LikePost;

public class LikePostCommandHandler : IRequestHandler<LikePostCommand>
{
    private readonly IPostRepository _postRepository;
    private readonly INotificationService _notificationService;

    public LikePostCommandHandler(IPostRepository postRepository, INotificationService notificationService)
    {
        _postRepository = postRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(LikePostCommand request, CancellationToken cancellationToken)
    {
        var postId = PostId.Create(request.PostId);
        var userId = UserId.Create(request.UserId);
        
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new InvalidOperationException("Post not found");
            
        post.Like(userId);
        await _postRepository.UpdateAsync(post);

        // Send notification to post author
        await _notificationService.NotifyUserAsync(
            post.AuthorId.Value.ToString(),
            $"Someone liked your post: \"{post.Content.Value.Substring(0, Math.Min(50, post.Content.Value.Length))}...\"",
            "like",
            new
            {
                PostId = post.Id.Value,
                PostContent = post.Content.Value,
                LikedByUserId = userId.Value
            });

        // Send real-time update to all users viewing this post
        await _notificationService.NotifyPostGroupAsync(
            postId.Value.ToString(),
            $"Post received a new like!",
            "post_like",
            new
            {
                PostId = post.Id.Value,
                LikesCount = post.GetLikesCount(),
                LikedByUserId = userId.Value
            });
    }
} 