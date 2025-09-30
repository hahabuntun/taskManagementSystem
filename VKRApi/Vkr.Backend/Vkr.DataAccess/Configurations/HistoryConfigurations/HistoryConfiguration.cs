using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.History;

namespace Vkr.DataAccess.Configurations.HistoryConfigurations;

public class HistoryConfiguration : IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> builder)
    {
        builder.ToTable("history").HasKey(n => n.Id);

        builder.Property(n => n.Text).IsRequired().HasMaxLength(1000);
        builder.Property(n => n.RelatedEntityId).IsRequired();
        builder.Property(n => n.RelatedEntityType).IsRequired().HasConversion<string>();
        builder.Property(n => n.CreatedOn).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(n => n.CreatorId).IsRequired();

        builder.HasOne(n => n.Creator)
            .WithMany()
            .HasForeignKey(n => n.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(n => n.RelatedEntityId);
        builder.HasIndex(n => n.RelatedEntityType);
        builder.HasIndex(n => n.CreatorId);
    }
}