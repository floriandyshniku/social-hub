using MediatR;
using newProject.Domain.Posts;

namespace newProject.Application.Posts.Queries.GetPost;

public class GetPostQuery : IRequest<Post?>
{
    public Guid PostId { get; set; }
} 