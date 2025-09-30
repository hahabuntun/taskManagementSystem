using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Organization;

namespace Vkr.DataAccess.Configurations.OrganizationConfigurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organizations>
{
    public void Configure(EntityTypeBuilder<Organizations> builder)
    {
        builder.ToTable("organization").HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.CreatedOn);

        builder
            .Property(x => x.OwnerId)
            .IsRequired();

        builder
            .HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerId);

        builder.HasData(
            new Organizations
            {
                Id = 1,
                Name = "Инновации",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                OwnerId = 1 // Assuming Worker ID 1 (e.g., Иван Петров)
            }
        );
    }
}