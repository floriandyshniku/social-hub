using MediatR;
using newProject.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace newProject.Application.Posts.Queries.GetAllPosts;

public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, IEnumerable<Post>>
{
    private readonly IPostRepository _postRepository;

    public GetAllPostsQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<Post>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _postRepository.GetAllAsync();

        if (request.PublishedOnly)
        {
            queryable = queryable.Where(p => p.IsPublished && !p.IsDeleted);
        }

        var posts = await queryable
            .Skip(request.Skip)
            .Take(request.Take)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        return posts;
    }
} 