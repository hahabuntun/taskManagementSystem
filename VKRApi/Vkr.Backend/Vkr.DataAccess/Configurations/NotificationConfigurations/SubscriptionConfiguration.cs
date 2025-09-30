using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Notification;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription> 
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscriptions").HasKey(s => s.Id);
        builder.Property(s => s.WorkerId)
        .IsRequired();

    builder.Property(s => s.EntityId)
        .IsRequired();

    builder.Property(s => s.EntityType)
        .IsRequired()
        .HasConversion<int>();

    builder.HasOne(s => s.Worker)
        .WithMany()
        .HasForeignKey(s => s.WorkerId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(s => new { s.WorkerId, s.EntityId, s.EntityType })
        .IsUnique();
}

}