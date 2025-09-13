using newProject.Domain.Common;
using newProject.Domain.Posts.ValueObjects;
using newProject.Domain.Users.ValueObjects;

namespace newProject.Domain.Posts.Events;

public class CommentAddedEvent : IDomainEvent
{
    public PostId PostId { get; }
    public CommentId CommentId { get; }
    public UserId AuthorId { get; }
    public string CommentContent { get; }
    public DateTime OccurredOn { get; }

    public CommentAddedEvent(PostId postId, CommentId commentId, UserId authorId, string commentContent)
    {
        PostId = postId;
        CommentId = commentId;
        AuthorId = authorId;
        CommentContent = commentContent;
        OccurredOn = DateTime.UtcNow;
    }
} 