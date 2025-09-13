using newProject.Domain.Common;

namespace newProject.Domain.Posts.ValueObjects;

public class PostContent : ValueObject
{
    public string Value { get; }

    private PostContent(string value)
    {
        Value = value;
    }

    // Parameterless constructor for EF Core
    private PostContent() { }

    public static PostContent Create(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Post content cannot be empty", nameof(content));

        if (content.Length > 280)
            throw new ArgumentException("Post content cannot exceed 280 characters", nameof(content));

        return new PostContent(content.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
} 