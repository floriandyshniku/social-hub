using MediatR;

namespace newProject.Application.Posts.Commands.LikePost;

public class LikePostCommand : IRequest
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
} 