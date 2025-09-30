using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations;

public class TaskTemplateConfiguration : IEntityTypeConfiguration<TaskTemplates>
{
    public void Configure(EntityTypeBuilder<TaskTemplates> builder)
    {
        builder.ToTable("TaskTemplates");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.TemplateName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.TaskName)
            .HasMaxLength(100);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.TaskStatusId)
            .IsRequired(false);

        builder.Property(t => t.TaskPriorityId)
            .IsRequired(false);

        builder.Property(t => t.TaskTypeId)
            .IsRequired(false);

        builder.Property(t => t.Progress)
            .IsRequired(false)
            .HasDefaultValue(0);

        builder.Property(t => t.StoryPoints)
            .IsRequired(false)
            .HasDefaultValue(0);

        // Creation date is required and defaults to the current timestamp
        builder.Property(t => t.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Configure relationship with TaskStatus
        builder.HasOne(t => t.TaskStatus)
            .WithMany(s => s.TaskTemplates)
            .HasForeignKey(t => t.TaskStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure relationship with TaskPriority
        builder.HasOne(t => t.TaskPriority)
            .WithMany(p => p.TaskTemplates)
            .HasForeignKey(t => t.TaskPriorityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure relationship with TaskType
        builder.HasOne(t => t.TaskType)
            .WithMany() // No reverse navigation in TaskType for TaskTemplates
            .HasForeignKey(t => t.TaskTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure many-to-many relationship with Tags
        builder.HasMany(t => t.Tags)
            .WithMany(t => t.TaskTemplates)
            .UsingEntity<Dictionary<string, object>>(
                "TaskTemplateTags",
                j => j.HasOne<Tags>().WithMany().HasForeignKey("TagId"),
                j => j.HasOne<TaskTemplates>().WithMany().HasForeignKey("TaskTemplateId"),
                j =>
                {
                    j.HasKey("TagId", "TaskTemplateId");
                    j.ToTable("TaskTemplateTags");
                });

        // Configure one-to-many relationship with TaskTemplateLink
        builder.HasMany(t => t.TaskTempateLinks)
            .WithOne(tl => tl.TaskTemplate)
            .HasForeignKey(tl => tl.TaskTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed Task Templates with TaskTypeId
        builder.HasData(
            new TaskTemplates
            {
                Id = 1,
                TemplateName = "Requirement Analysis Template",
                TaskName = "Анализ требований",
                Description = "Сбор и анализ требований для проекта.",
                TaskTypeId = 3, // Epic
                TaskStatusId = 1,
                TaskPriorityId = 2,
                StoryPoints = 8,
                CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TaskTemplates
            {
                Id = 2,
                TemplateName = "UI Design Template",
                TaskName = "Дизайн интерфейса",
                Description = "Создание UI/UX дизайна для приложения.",
                TaskTypeId = 3, // Epic
                TaskStatusId = 1,
                TaskPriorityId = 2,
                StoryPoints = 13,
                CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TaskTemplates
            {
                Id = 3,
                TemplateName = "API Development Template",
                TaskName = "Разработка API",
                Description = "Создание REST API для приложения.",
                TaskTypeId = 1, // Task
                TaskStatusId = 1,
                TaskPriorityId = 2,
                StoryPoints = 5,
                CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TaskTemplates
            {
                Id = 4,
                TemplateName = "Testing Template",
                TaskName = "Тестирование",
                Description = "Проведение тестирования приложения (unit, интеграционное, нагрузочное).",
                TaskTypeId = 3, // Epic
                TaskStatusId = 1,
                TaskPriorityId = 2,
                StoryPoints = 8,
                CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new TaskTemplates
            {
                Id = 5,
                TemplateName = "Deployment Template",
                TaskName = "Развертывание",
                Description = "Подготовка и выпуск приложения в продакшен.",
                TaskTypeId = 3, // Epic
                TaskStatusId = 1,
                TaskPriorityId = 2,
                StoryPoints = 3,
                CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}