using MediatR;

namespace newProject.Application.Posts.Commands.PublishPost;

public class PublishPostCommand : IRequest
{
    public Guid PostId { get; set; }
} 