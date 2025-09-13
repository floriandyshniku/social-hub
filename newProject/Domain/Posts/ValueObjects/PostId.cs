using newProject.Domain.Common;

namespace newProject.Domain.Posts.ValueObjects;

public class PostId : ValueObject
{
    public Guid Value { get; }

    private PostId(Guid value)
    {
        Value = value;
    }

    // Parameterless constructor for EF Core
    private PostId() { }

    public static PostId Create()
    {
        return new PostId(Guid.NewGuid());
    }

    public static PostId Create(Guid value)
    {
        return new PostId(value);
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