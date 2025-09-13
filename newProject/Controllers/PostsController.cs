using Microsoft.AspNetCore.Mvc;
using MediatR;
using newProject.Application.Posts.Commands.CreatePost;
using newProject.Application.Posts.Commands.PublishPost;
using newProject.Application.Posts.Commands.LikePost;
using newProject.Application.Posts.Commands.AddComment;
using newProject.Application.Posts.Queries.GetPost;
using newProject.Application.Posts.Queries.GetAllPosts;

namespace newProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
    {
        var postId = await _mediator.Send(command);
        return Ok(new { PostId = postId.Value });
    }

    [HttpGet("{postId:guid}")]
    public async Task<IActionResult> GetPost(Guid postId)
    {
        var query = new GetPostQuery { PostId = postId };
        var post = await _mediator.Send(query);

        if (post == null)
            return NotFound();

        return Ok(new
        {
            Id = post.Id.Value,
            AuthorId = post.AuthorId.Value,
            Content = post.Content.Value,
            ImageUrl = post.ImageUrl,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            IsPublished = post.IsPublished,
            IsDeleted = post.IsDeleted,
            LikesCount = post.GetLikesCount(),
            CommentsCount = post.GetCommentsCount(),
            Hashtags = post.Hashtags.Select(h => h.Value)
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts([FromQuery] GetAllPostsQuery query)
    {
        var posts = await _mediator.Send(query);

        var response = posts.Select(post => new
        {
            Id = post.Id.Value,
            AuthorId = post.AuthorId.Value,
            Content = post.Content.Value,
            ImageUrl = post.ImageUrl,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            IsPublished = post.IsPublished,
            IsDeleted = post.IsDeleted,
            LikesCount = post.GetLikesCount(),
            CommentsCount = post.GetCommentsCount(),
            Hashtags = post.Hashtags.Select(h => h.Value)
        });

        return Ok(response);
    }

    [HttpPost("{postId:guid}/publish")]
    public async Task<IActionResult> PublishPost(Guid postId)
    {
        var command = new PublishPostCommand { PostId = postId };
        await _mediator.Send(command);
        return Ok(new { Message = "Post published successfully" });
    }

    [HttpPost("{postId:guid}/like")]
    public async Task<IActionResult> LikePost(Guid postId, [FromBody] LikePostCommand command)
    {
        command.PostId = postId;
        await _mediator.Send(command);
        return Ok(new { Message = "Post liked successfully" });
    }

    [HttpPost("{postId:guid}/comments")]
    public async Task<IActionResult> AddComment(Guid postId, [FromBody] AddCommentCommand command)
    {
        command.PostId = postId;
        await _mediator.Send(command);
        return Ok(new { Message = "Comment added successfully" });
    }
} 