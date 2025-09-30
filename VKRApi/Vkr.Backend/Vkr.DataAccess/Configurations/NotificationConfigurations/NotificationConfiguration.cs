using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Notification;

namespace Vkr.DataAccess.Configurations.NotificationConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications").HasKey(n => n.Id);

        builder.Property(n => n.Text).IsRequired().HasMaxLength(1000);
        builder.Property(n => n.RelatedEntityName).IsRequired().HasMaxLength(100);
        builder.Property(n => n.RelatedEntityId).IsRequired();
        builder.Property(n => n.RelatedEntityType).IsRequired().HasConversion<string>();
        builder.Property(n => n.CreatedOn).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(n => n.CreatorId).IsRequired();
        builder.Property(n => n.IsRead).IsRequired().HasDefaultValue(false);

        builder.HasOne(n => n.Creator)
            .WithMany()
            .HasForeignKey(n => n.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(n => n.WorkerNotifications)
            .WithOne(wn => wn.Notification)
            .HasForeignKey(wn => wn.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(n => n.RelatedEntityId);
        builder.HasIndex(n => n.RelatedEntityType);
        builder.HasIndex(n => n.CreatorId);
    }
}