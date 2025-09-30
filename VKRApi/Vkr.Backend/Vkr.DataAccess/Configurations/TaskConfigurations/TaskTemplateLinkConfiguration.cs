using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskTemplateLinkConfiguration : IEntityTypeConfiguration<TaskTemplateLink>
{
    public void Configure(EntityTypeBuilder<TaskTemplateLink> builder)
    {
        builder.ToTable("taskTemplate_links").HasKey(pl => pl.Id);

        // Настройка свойств
        builder.Property(ttl => ttl.Link)
            .IsRequired()
            .HasMaxLength(500); // Ограничение длины ссылки

        builder.Property(ttl => ttl.Description)
            .HasMaxLength(1000); // Ограничение длины описания

        builder.Property(ttl => ttl.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()"); // Текущая дата по умолчанию


        // Связь с проектом
        builder.HasOne(ttl => ttl.TaskTemplate)
            .WithMany(tt => tt.TaskTempateLinks)
            .HasForeignKey(ttl => ttl.TaskTemplateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Индексы
        builder.HasIndex(ttl => ttl.TaskTemplateId);

        builder.HasData(
            new TaskTemplateLink
            {
                Id = 1,
                TaskTemplateId = 1,
                Link = "https://docs.example.com/requirements",
                Description = "Ссылка на документ с требованиями для анализа.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc)
            },
            new TaskTemplateLink
            {
                Id = 2,
                TaskTemplateId = 1,
                Link = "https://collaboration.example.com/requirements-tool",
                Description = "Инструмент для совместной работы над требованиями.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc)
            }
        );
    }
}