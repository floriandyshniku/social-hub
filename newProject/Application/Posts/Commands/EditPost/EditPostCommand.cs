using MediatR;
using newProject.Domain.Posts.ValueObjects;

namespace SocialHub.Application.Posts.Commands.EditPost
{
    public class EditPostCommand :  IRequest<PostId>
    {
        public Guid PostId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
