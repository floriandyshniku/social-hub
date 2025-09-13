using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using newProject.Domain.Posts;
using System.Text.Json;

namespace newProject.Infrastructure.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(e => e.Id);
        
        // Value object conversions
        builder.Property(e => e.Id).HasConversion(
            commentId => commentId.Value,
            value => Domain.Posts.ValueObjects.CommentId.Create(value)
        );
        
        builder.Property(e => e.PostId).HasConversion(
            postId => postId.Value,
            value => Domain.Posts.ValueObjects.PostId.Create(value)
        );
        
        builder.Property(e => e.AuthorId).HasConversion(
            userId => userId.Value,
            value => Domain.Users.ValueObjects.UserId.Create(value)
        );

        builder.Property(e => e.Content).HasMaxLength(500);

        // JSON array
        builder.Property(e => e.Likes)
            .HasConversion(
                likes => JsonSerializer.Serialize(likes.Select(l => l.Value), (JsonSerializerOptions?)null),
                json => SafeDeserializeUserIds(json)
            );

        // Foreign key relationships
        builder.HasOne<Post>()
            .WithMany(p => p.Comments)
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Domain.Users.User>()
            .WithMany()
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.PostId);
        builder.HasIndex(e => e.AuthorId);
        builder.HasIndex(e => e.CreatedAt);
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