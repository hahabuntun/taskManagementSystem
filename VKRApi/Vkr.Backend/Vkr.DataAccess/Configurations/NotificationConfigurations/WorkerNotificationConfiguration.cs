using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Notification;

namespace Vkr.DataAccess.Configurations.NotificationConfigurations;

public class WorkerNotificationConfiguration : IEntityTypeConfiguration<WorkerNotification>
{
    public void Configure(EntityTypeBuilder<WorkerNotification> builder)
    {
        builder.ToTable("worker_notifications")
            .HasKey(wn => new { wn.NotificationId, wn.WorkerId });

        builder.Property(wn => wn.NotificationId)
            .IsRequired();

        builder.Property(wn => wn.WorkerId)
            .IsRequired();

        builder.Property(wn => wn.IsRead)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(wn => wn.Notification)
            .WithMany(n => n.WorkerNotifications)
            .HasForeignKey(wn => wn.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wn => wn.Worker)
            .WithMany()
            .HasForeignKey(wn => wn.WorkerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(wn => new { wn.WorkerId, wn.NotificationId })
            .IsUnique();
    }
}