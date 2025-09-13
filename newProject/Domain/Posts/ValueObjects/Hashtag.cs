using newProject.Domain.Common;

namespace newProject.Domain.Posts.ValueObjects;

public class Hashtag : ValueObject
{
    public string Value { get; }

    private Hashtag(string value)
    {
        Value = value;
    }

    // Parameterless constructor for EF Core
    private Hashtag() { }

    public static Hashtag Create(string hashtag)
    {
        if (string.IsNullOrWhiteSpace(hashtag))
            throw new ArgumentException("Hashtag cannot be empty", nameof(hashtag));

        // Remove # if present and normalize
        var normalizedHashtag = hashtag.Trim().ToLowerInvariant();
        if (normalizedHashtag.StartsWith("#"))
        {
            normalizedHashtag = normalizedHashtag.Substring(1);
        }

        if (normalizedHashtag.Length < 2)
            throw new ArgumentException("Hashtag must be at least 2 characters long", nameof(hashtag));

        if (normalizedHashtag.Length > 50)
            throw new ArgumentException("Hashtag cannot exceed 50 characters", nameof(hashtag));

        if (!IsValidHashtag(normalizedHashtag))
            throw new ArgumentException("Hashtag can only contain letters, numbers, and underscores", nameof(hashtag));

        return new Hashtag(normalizedHashtag);
    }

    private static bool IsValidHashtag(string hashtag)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(hashtag, @"^[a-z0-9_]+$");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return $"#{Value}";
    }
} 