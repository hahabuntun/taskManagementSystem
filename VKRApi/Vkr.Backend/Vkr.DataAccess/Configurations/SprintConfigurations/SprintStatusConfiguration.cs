using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Sprint;

namespace Vkr.DataAccess.Configurations.SprintConfigurations;

public class SprintStatusConfiguration : IEntityTypeConfiguration<SprintStatus>
{
    public void Configure(EntityTypeBuilder<SprintStatus> builder)
    {
        builder.ToTable("sprint_statuses").HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Color)
            .IsRequired()
            .HasMaxLength(7);

        builder.HasData(
            new SprintStatus
            {
                Id = 1,
                Name = "PLANNED",
                Color = "#ffd666"
            },
            new SprintStatus
            {
                Id = 2,
                Name = "ACTIVE",
                Color = "#95de64"
            },
            new SprintStatus
            {
                Id = 3,
                Name = "FINISHED",
                Color = "#69b1ff"
            }
        );
    }
}