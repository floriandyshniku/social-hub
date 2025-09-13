using newProject.Domain.Common;

namespace newProject.Domain.Users.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId(Guid value)
    {
        Value = value;
    }

    // Parameterless constructor for EF Core
    private UserId() { }

    public static UserId Create()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Create(Guid value)
    {
        return new UserId(value);
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