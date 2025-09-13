using MediatR;
using newProject.Domain.Posts;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Application.Posts.Commands.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand>
{
    private readonly IPostRepository _postRepository;

    public AddCommentCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var postId = PostId.Create(request.PostId);
        var authorId = UserId.Create(request.AuthorId);
        
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new InvalidOperationException("Post not found");
            
        post.AddComment(authorId, request.Content);
        await _postRepository.UpdateAsync(post);
    }
} 