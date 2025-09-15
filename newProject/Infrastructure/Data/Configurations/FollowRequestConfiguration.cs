using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using newProject.Domain.Users;
using SocialHub.Domain.Users;
using SocialHub.Domain.Users.ValueObjects;

namespace SocialHub.Infrastructure.Data.Configurations
{
	public class FollowRequestConfiguration : IEntityTypeConfiguration<FollowRequest>
	{
		public void Configure(EntityTypeBuilder<FollowRequest> builder)
		{
			builder.ToTable("FollowRequests");

			// Primary key: FollowRequestId value object
			builder.HasKey(fr => fr.Id);
			builder.Property(fr => fr.Id)
				.HasConversion(id => id.Value, value => FollowRequestId.Create(value))
				.ValueGeneratedNever();

			// Value object conversions
			builder.Property(fr => fr.UserId)
				.HasConversion(uid => uid.Value, value => newProject.Domain.Users.ValueObjects.UserId.Create(value))
				.HasColumnName("UserId")
				.IsRequired();

			builder.Property(fr => fr.FromUserId)
				.HasConversion(uid => uid.Value, value => newProject.Domain.Users.ValueObjects.UserId.Create(value))
				.HasColumnName("FromUserId")
				.IsRequired();

			builder.Property(fr => fr.RequestedAt).IsRequired();
			builder.Property(fr => fr.IsAccepted).IsRequired();
			builder.Property(fr => fr.IsRejected).IsRequired();

			// Relationships
			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey(fr => fr.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Indexes
			builder.HasIndex(fr => fr.UserId);
			builder.HasIndex(fr => fr.FromUserId);
			builder.HasIndex(fr => fr.RequestedAt);
		}
	}
} 