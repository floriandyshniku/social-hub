using MediatR;
using newProject.Domain.Posts;

namespace newProject.Application.Posts.Queries.GetAllPosts;

public class GetAllPostsQuery : IRequest<IEnumerable<Post>>
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 20;
    public bool PublishedOnly { get; set; } = true;
} 