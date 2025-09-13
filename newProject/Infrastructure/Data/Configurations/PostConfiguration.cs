using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using newProject.Domain.Posts;
using System.Text.Json;

namespace newProject.Infrastructure.Data.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(e => e.Id);
        
        // Value object conversions
        builder.Property(e => e.Id).HasConversion(
            postId => postId.Value,
            value => Domain.Posts.ValueObjects.PostId.Create(value)
        );
        
        builder.Property(e => e.AuthorId).HasConversion(
            userId => userId.Value,
            value => Domain.Users.ValueObjects.UserId.Create(value)
        );
        
        builder.Property(e => e.Content)
            .HasConversion(
                content => content.Value,
                value => Domain.Posts.ValueObjects.PostContent.Create(value)
            )
            .HasMaxLength(280);

        // JSON arrays
        builder.Property(e => e.Hashtags)
            .HasConversion(
                hashtags => JsonSerializer.Serialize(hashtags.Select(h => h.Value), (JsonSerializerOptions?)null),
                json => SafeDeserializeHashtags(json)
            );

        builder.Property(e => e.Likes)
            .HasConversion(
                likes => JsonSerializer.Serialize(likes.Select(l => l.Value), (JsonSerializerOptions?)null),
                json => SafeDeserializeUserIds(json)
            );

        // Foreign key relationship
        builder.HasOne<Domain.Users.User>()
            .WithMany()
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.AuthorId);
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.IsPublished);
    }

    private static List<Domain.Posts.ValueObjects.Hashtag> SafeDeserializeHashtags(string json)
    {
        try
        {
            var hashtags = JsonSerializer.Deserialize<List<string>>(json);
            return hashtags?.Select(h => Domain.Posts.ValueObjects.Hashtag.Create(h)).ToList() ?? new List<Domain.Posts.ValueObjects.Hashtag>();
        }
        catch
        {
            return new List<Domain.Posts.ValueObjects.Hashtag>();
        }
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