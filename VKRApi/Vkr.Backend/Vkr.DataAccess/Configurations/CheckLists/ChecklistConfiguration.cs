using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.CheckLists;

namespace Vkr.DataAccess.Configurations.CheckLists;

public class ChecklistConfiguration : IEntityTypeConfiguration<Checklist>
{
    public void Configure(EntityTypeBuilder<Checklist> b)
    {
        b.ToTable("checklists");
        b.HasKey(x => x.Id);
        b.Property(x => x.OwnerType).IsRequired();
        b.Property(x => x.OwnerId).IsRequired();
        b.Property(x => x.Title).IsRequired().HasMaxLength(200);
        b.HasMany(x => x.Items)
            .WithOne(x => x.Checklist!)
            .HasForeignKey(x => x.ChecklistId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(x => new { x.OwnerType, x.OwnerId });
    }
}