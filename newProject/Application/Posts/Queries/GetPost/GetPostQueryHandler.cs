using MediatR;
using newProject.Domain.Posts;
using newProject.Domain.Posts.ValueObjects;

namespace newProject.Application.Posts.Queries.GetPost;

public class GetPostQueryHandler : IRequestHandler<GetPostQuery, Post?>
{
    private readonly IPostRepository _postRepository;

    public GetPostQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Post?> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        var postId = PostId.Create(request.PostId);
        return await _postRepository.GetByIdAsync(postId);
    }
} 