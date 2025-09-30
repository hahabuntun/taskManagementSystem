using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Progect;

namespace Vkr.DataAccess.Configurations.ProjectConfigurations
{
    public class ProjectStatusConfiguration : IEntityTypeConfiguration<ProjectStatus>
    {
        public void Configure(EntityTypeBuilder<ProjectStatus> builder)
        {
            builder.ToTable("project_statuses");

            builder.HasKey(s => s.Id);
            
            builder
                .HasOne(s => s.RelatedColor)
                .WithMany();

            builder.Property(s => s.Name).IsRequired();

            builder.HasData(
                    new ProjectStatus
                    {
                        Id = 1,
                        Name = "Инициализируется",
                        RelatedColorId = 15
                    },
                    new ProjectStatus
                    {
                        Id = 2,
                        Name = "В работе",
                        RelatedColorId = 20
                    },
                    new ProjectStatus
                    {
                        Id = 3,
                        Name = "На проверке",
                        RelatedColorId = 11
                    },
                    new ProjectStatus
                    {
                        Id = 4,
                        Name = "Завершен",
                        RelatedColorId = 14
                    },
                    new ProjectStatus
                    {
                        Id = 5,
                        Name = "В архиве",
                        RelatedColorId = 14
                    }
                );
        }
    }
}
