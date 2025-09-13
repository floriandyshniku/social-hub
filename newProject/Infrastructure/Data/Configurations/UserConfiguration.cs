using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using newProject.Domain.Users;
using System.Text.Json;

namespace newProject.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        
        // Value object conversions
        builder.Property(e => e.Id).HasConversion(
            userId => userId.Value,
            value => Domain.Users.ValueObjects.UserId.Create(value)
        );
        
        builder.Property(e => e.Username)
            .HasConversion(
                username => username.Value,
                value => Domain.Users.ValueObjects.Username.Create(value)
            )
            .HasMaxLength(30);
        
        builder.Property(e => e.Email)
            .HasConversion(
                email => email.Value,
                value => Domain.Users.ValueObjects.Email.Create(value)
            )
            .HasMaxLength(255);

        // JSON arrays
        builder.Property(e => e.Following)
            .HasConversion(
                following => JsonSerializer.Serialize(following.Select(f => f.Value), (JsonSerializerOptions?)null),
                json => SafeDeserializeUserIds(json)
            );

        builder.Property(e => e.Followers)
            .HasConversion(
                followers => JsonSerializer.Serialize(followers.Select(f => f.Value), (JsonSerializerOptions?)null),
                json => SafeDeserializeUserIds(json)
            );

        // Indexes
        builder.HasIndex(e => e.Username).IsUnique();
        builder.HasIndex(e => e.Email).IsUnique();
    }

    private static List<Domain.Users.ValueObjects.UserId> SafeDeserializeUserIds(string json)
    {
        try
        {
            var userIds = JsonSerializer.Deserialize<List<Guid>>(json);
            return userIds?.Select(id => Domain.Users.ValueObjects.UserId.Create(id)).ToList() ?? new List<Domain.Users.ValueObjects.UserId>();
        }
        catch
        {
            return new List<Domain.Users.ValueObjects.UserId>();
        }
    }
} 