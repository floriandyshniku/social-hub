using Microsoft.EntityFrameworkCore;
using newProject.Domain.Users;
using newProject.Domain.Posts;
using System.Text.Json;
using SocialHub.Domain.Users;

namespace newProject.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<FollowRequest> FollowRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all IEntityTypeConfiguration<> mappings in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Value object conversions
            entity.Property(e => e.Id).HasConversion(
                userId => userId.Value,
                value => Domain.Users.ValueObjects.UserId.Create(value)
            );
            
            entity.Property(e => e.Username)
                .HasConversion(
                    username => username.Value,
                    value => Domain.Users.ValueObjects.Username.Create(value)
                )
                .HasMaxLength(30);
            
            entity.Property(e => e.Email)
                .HasConversion(
                    email => email.Value,
                    value => Domain.Users.ValueObjects.Email.Create(value)
                )
                .HasMaxLength(255);

            // JSON arrays
            entity.Property(e => e.Following)
                .HasConversion(
                    following => JsonSerializer.Serialize(following.Select(f => f.Value), (JsonSerializerOptions?)null),
                    json => SafeDeserializeUserIds(json)
                );

            entity.Property(e => e.Followers)
                .HasConversion(
                    followers => JsonSerializer.Serialize(followers.Select(f => f.Value), (JsonSerializerOptions?)null),
                    json => SafeDeserializeUserIds(json)
                );

            // Indexes
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Post entity configuration
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Value object conversions
            entity.Property(e => e.Id).HasConversion(
                postId => postId.Value,
                value => Domain.Posts.ValueObjects.PostId.Create(value)
            );
            
            entity.Property(e => e.AuthorId).HasConversion(
                userId => userId.Value,
                value => Domain.Users.ValueObjects.UserId.Create(value)
            );
            
            entity.Property(e => e.Content)
                .HasConversion(
                    content => content.Value,
                    value => Domain.Posts.ValueObjects.PostContent.Create(value)
                )
                .HasMaxLength(280);

            // JSON arrays
            entity.Property(e => e.Hashtags)
                .HasConversion(
                    hashtags => JsonSerializer.Serialize(hashtags.Select(h => h.Value), (JsonSerializerOptions?)null),
                    json => SafeDeserializeHashtags(json)
                );

            entity.Property(e => e.Likes)
                .HasConversion(
                    likes => JsonSerializer.Serialize(likes.Select(l => l.Value), (JsonSerializerOptions?)null),
                    json => SafeDeserializeUserIds(json)
                );

            // Foreign key relationship
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.IsPublished);
        });

        // Comment entity configuration
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Value object conversions
            entity.Property(e => e.Id).HasConversion(
                commentId => commentId.Value,
                value => Domain.Posts.ValueObjects.CommentId.Create(value)
            );
            
            entity.Property(e => e.PostId).HasConversion(
                postId => postId.Value,
                value => Domain.Posts.ValueObjects.PostId.Create(value)
            );
            
            entity.Property(e => e.AuthorId).HasConversion(
                userId => userId.Value,
                value => Domain.Users.ValueObjects.UserId.Create(value)
            );

            entity.Property(e => e.Content).HasMaxLength(500);

            // JSON array
            entity.Property(e => e.Likes)
                .HasConversion(
                    likes => JsonSerializer.Serialize(likes.Select(l => l.Value), (JsonSerializerOptions?)null),
                    json => SafeDeserializeUserIds(json)
                );

            // Foreign key relationships
            entity.HasOne<Post>()
                .WithMany(p => p.Comments)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.PostId);
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.CreatedAt);
        });
    }

    // Helper methods for safe deserialization
    private static List<Domain.Users.ValueObjects.UserId> SafeDeserializeUserIds(string? json)
    {
        if (string.IsNullOrEmpty(json))
            return new List<Domain.Users.ValueObjects.UserId>();

        try
        {
            var guids = JsonSerializer.Deserialize<List<Guid>>(json, (JsonSerializerOptions?)null);
            if (guids == null)
                return new List<Domain.Users.ValueObjects.UserId>();

            var userIds = new List<Domain.Users.ValueObjects.UserId>();
            foreach (var guid in guids)
            {
                try
                {
                    userIds.Add(Domain.Users.ValueObjects.UserId.Create(guid));
                }
                catch
                {
                    // Skip invalid user IDs
                    continue;
                }
            }
            return userIds;
        }
        catch
        {
            return new List<Domain.Users.ValueObjects.UserId>();
        }
    }

    private static List<Domain.Posts.ValueObjects.Hashtag> SafeDeserializeHashtags(string? json)
    {
        if (string.IsNullOrEmpty(json))
            return new List<Domain.Posts.ValueObjects.Hashtag>();

        try
        {
            var hashtagStrings = JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null);
            if (hashtagStrings == null)
                return new List<Domain.Posts.ValueObjects.Hashtag>();

            var hashtags = new List<Domain.Posts.ValueObjects.Hashtag>();
            foreach (var hashtagString in hashtagStrings)
            {
                try
                {
                    hashtags.Add(Domain.Posts.ValueObjects.Hashtag.Create(hashtagString));
                }
                catch
                {
                    continue;
                }
            }
            return hashtags;
        }
        catch
        {
            return new List<Domain.Posts.ValueObjects.Hashtag>();
        }
    }
} 