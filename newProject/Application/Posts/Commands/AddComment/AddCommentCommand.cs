using MediatR;

namespace newProject.Application.Posts.Commands.AddComment;

public class AddCommentCommand : IRequest
{
    public Guid PostId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
} 