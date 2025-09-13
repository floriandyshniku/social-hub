using MediatR;
using newProject.Domain.Posts;
using newProject.Domain.Posts.ValueObjects;

namespace newProject.Application.Posts.Commands.PublishPost;

public class PublishPostCommandHandler : IRequestHandler<PublishPostCommand>
{
    private readonly IPostRepository _postRepository;

    public PublishPostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task Handle(PublishPostCommand request, CancellationToken cancellationToken)
    {
        var postId = PostId.Create(request.PostId);
        var post = await _postRepository.GetByIdAsync(postId);
        
        if (post == null)
            throw new InvalidOperationException("Post not found");
            
        post.Publish();
        await _postRepository.UpdateAsync(post);
    }
} 