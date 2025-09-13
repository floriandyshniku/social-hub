using MediatR;
using newProject.Domain.Posts;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Application.Posts.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostId>
{
    private readonly IPostRepository _postRepository;

    public CreatePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<PostId> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var authorId = UserId.Create(request.AuthorId);
        var content = PostContent.Create(request.Content);
        
        var post = Post.Create(authorId, content, request.ImageUrl);
        
        await _postRepository.AddAsync(post);
        
        return post.Id;
    }
} 