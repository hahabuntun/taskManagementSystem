using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.CheckLists;

namespace Vkr.DataAccess.Configurations.CheckLists;

public class CheclistItemConfiguration : IEntityTypeConfiguration<ChecklistItem>
{
    public void Configure(EntityTypeBuilder<ChecklistItem> b)
    {
        b.ToTable("checklist_items");
        b.HasKey(x => x.Id);
        b.Property(x => x.Title).IsRequired().HasMaxLength(200);
        b.Property(x => x.IsCompleted).IsRequired();
    }
}