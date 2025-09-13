using newProject.Domain.Common;

namespace newProject.Domain.Users.ValueObjects;

public class Username : ValueObject
{
    public string Value { get; }

    private Username(string value)
    {
        Value = value;
    }

    // Parameterless constructor for EF Core
    private Username() { }

    public static Username Create(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));

        if (username.Length < 3 || username.Length > 30)
            throw new ArgumentException("Username must be between 3 and 30 characters", nameof(username));

        if (!IsValidUsername(username))
            throw new ArgumentException("Username can only contain letters, numbers, and underscores", nameof(username));

        return new Username(username.ToLowerInvariant());
    }

    public bool Contains(string substring)
    {
        if (substring == null) return false;
        return Value.Contains(substring, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsValidUsername(string username)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");
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