using newProject.Domain.Common;

namespace newProject.Domain.Posts.ValueObjects;

public class CommentId : ValueObject
{
    public Guid Value { get; }

    private CommentId(Guid value)
    {
        Value = value;
    }

    // Parameterless constructor for EF Core
    private CommentId() { }

    public static CommentId Create()
    {
        return new CommentId(Guid.NewGuid());
    }

    public static CommentId Create(Guid value)
    {
        return new CommentId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
} 