using MediatR;
using newProject.Domain.Posts;
using newProject.Domain.Posts.ValueObjects;

namespace SocialHub.Application.Posts.Commands.EditPost
{
    public class EditPostCommandHandler : IRequestHandler<EditPostCommand, PostId>
    {
        private readonly IPostRepository _postRepository;

        public EditPostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<PostId> Handle(EditPostCommand request, CancellationToken cancellationToken)
        {
            var postId = PostId.Create(request.PostId);
            var contentToUpdate = PostContent.Create(request.Content);

            var post = await _postRepository.GetByIdAsync(postId) ?? throw new Exception("Post not found");

            post.UpdateContent(contentToUpdate);
            await _postRepository.UpdateAsync(post);
            return post.Id;

        }
    }
}
