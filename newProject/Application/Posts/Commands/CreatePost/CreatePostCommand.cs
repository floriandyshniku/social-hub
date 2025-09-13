using MediatR;
using newProject.Domain.Posts.ValueObjects;

namespace newProject.Application.Posts.Commands.CreatePost;

public class CreatePostCommand : IRequest<PostId>
{
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
} 