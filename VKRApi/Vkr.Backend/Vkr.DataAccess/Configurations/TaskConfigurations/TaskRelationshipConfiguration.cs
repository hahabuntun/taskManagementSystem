using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskRelationshipConfiguration : IEntityTypeConfiguration<TaskRelationship>
{
    public void Configure(EntityTypeBuilder<TaskRelationship> builder)
    {
        // Set the table name and primary key for the TaskRelationship entity
        builder.ToTable("task_relationships").HasKey(tr => tr.Id);

         builder.Property(tr => tr.TaskId)
        .IsRequired();

    // RelatedTaskId references the target task of the relationship
    builder.Property(tr => tr.RelatedTaskId)
        .IsRequired();

    // TaskRelationshipTypeId specifies the type of dependency (FS, FF, SS, SF)
    builder.Property(tr => tr.TaskRelationshipTypeId)
        .IsRequired();

    // Configure relationship with source task
    // Each relationship is associated with one source task
    builder.HasOne(tr => tr.Task)
        .WithMany(t => t.TaskRelationships)
        .HasForeignKey(tr => tr.TaskId)
        .OnDelete(DeleteBehavior.Cascade); // Delete relationships when source task is deleted

    // Configure relationship with target task
    // Each relationship is associated with one target task
    builder.HasOne(tr => tr.RelatedTask)
        .WithMany(t => t.RelatedTaskRelationships)
        .HasForeignKey(tr => tr.RelatedTaskId)
        .OnDelete(DeleteBehavior.Cascade); // Delete relationships when target task is deleted

    // Configure relationship with TaskRelationshipType
    // Each relationship has one type (FS, FF, SS, SF)
    builder.HasOne(tr => tr.RelationshipType)
        .WithMany()
        .HasForeignKey(tr => tr.TaskRelationshipTypeId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if type is referenced

    // Configure index for performance
    // Index on TaskId and RelatedTaskId for efficient dependency queries
    builder.HasIndex(tr => new { tr.TaskId, tr.RelatedTaskId });

    // Seed data for task relationships
    builder.HasData(
        // Summary Task Relationships (ParentChild)
        new TaskRelationship { Id = 1, TaskId = 1, RelatedTaskId = 2, TaskRelationshipTypeId = 5 }, // Analysis → Сбор требований
        new TaskRelationship { Id = 2, TaskId = 1, RelatedTaskId = 3, TaskRelationshipTypeId = 5 }, // Analysis → Анализ конкурентов
        new TaskRelationship { Id = 3, TaskId = 1, RelatedTaskId = 4, TaskRelationshipTypeId = 5 }, // Analysis → Составление ТЗ
        new TaskRelationship { Id = 4, TaskId = 1, RelatedTaskId = 5, TaskRelationshipTypeId = 5 }, // Analysis → Утверждение ТЗ
        new TaskRelationship { Id = 5, TaskId = 6, RelatedTaskId = 7, TaskRelationshipTypeId = 5 }, // Design → Прототипирование UI
        new TaskRelationship { Id = 6, TaskId = 6, RelatedTaskId = 8, TaskRelationshipTypeId = 5 }, // Design → Дизайн логотипа
        new TaskRelationship { Id = 7, TaskId = 6, RelatedTaskId = 9, TaskRelationshipTypeId = 5 }, // Design → Финальный дизайн UI
        new TaskRelationship { Id = 8, TaskId = 6, RelatedTaskId = 10, TaskRelationshipTypeId = 5 }, // Design → Утверждение дизайна
        new TaskRelationship { Id = 9, TaskId = 12, RelatedTaskId = 13, TaskRelationshipTypeId = 5 }, // Development → Настройка окружения
        new TaskRelationship { Id = 10, TaskId = 12, RelatedTaskId = 14, TaskRelationshipTypeId = 5 }, // Development → Разработка API
        new TaskRelationship { Id = 11, TaskId = 12, RelatedTaskId = 15, TaskRelationshipTypeId = 5 }, // Development → Интеграция фронтенда
        new TaskRelationship { Id = 12, TaskId = 12, RelatedTaskId = 16, TaskRelationshipTypeId = 5 }, // Development → Реализация авторизации
        new TaskRelationship { Id = 13, TaskId = 12, RelatedTaskId = 17, TaskRelationshipTypeId = 5 }, // Development → Завершение разработки
        new TaskRelationship { Id = 14, TaskId = 18, RelatedTaskId = 19, TaskRelationshipTypeId = 5 }, // Testing → Unit-тесты
        new TaskRelationship { Id = 15, TaskId = 18, RelatedTaskId = 20, TaskRelationshipTypeId = 5 }, // Testing → Интеграционное тестирование
        new TaskRelationship { Id = 16, TaskId = 18, RelatedTaskId = 21, TaskRelationshipTypeId = 5 }, // Testing → Нагрузочное тестирование
        new TaskRelationship { Id = 17, TaskId = 18, RelatedTaskId = 22, TaskRelationshipTypeId = 5 }, // Testing → Релиз бета-версии
        new TaskRelationship { Id = 18, TaskId = 23, RelatedTaskId = 24, TaskRelationshipTypeId = 5 }, // Deployment → Подготовка документации
        new TaskRelationship { Id = 19, TaskId = 23, RelatedTaskId = 25, TaskRelationshipTypeId = 5 }, // Deployment → Маркетинговая кампания
        new TaskRelationship { Id = 20, TaskId = 23, RelatedTaskId = 26, TaskRelationshipTypeId = 5 }, // Deployment → Публикация CRM
        new TaskRelationship { Id = 21, TaskId = 23, RelatedTaskId = 27, TaskRelationshipTypeId = 5 }, // Deployment → Официальный релиз
        // Temporal Dependencies
        new TaskRelationship { Id = 22, TaskId = 3, RelatedTaskId = 2, TaskRelationshipTypeId = 3 }, // Анализ конкурентов → Сбор требований (StartToStart)
        new TaskRelationship { Id = 23, TaskId = 4, RelatedTaskId = 2, TaskRelationshipTypeId = 1 }, // Составление ТЗ → Сбор требований (FinishToStart)
        new TaskRelationship { Id = 24, TaskId = 5, RelatedTaskId = 4, TaskRelationshipTypeId = 1 }, // Утверждение ТЗ → Составление ТЗ (FinishToStart)
        new TaskRelationship { Id = 25, TaskId = 6, RelatedTaskId = 5, TaskRelationshipTypeId = 1 }, // Дизайн приложения → Утверждение ТЗ (FinishToStart)
        new TaskRelationship { Id = 26, TaskId = 7, RelatedTaskId = 5, TaskRelationshipTypeId = 1 }, // Прототипирование UI → Утверждение ТЗ (FinishToStart)
        new TaskRelationship { Id = 27, TaskId = 8, RelatedTaskId = 5, TaskRelationshipTypeId = 1 }, // Дизайн логотипа → Утверждение ТЗ (FinishToStart)
        new TaskRelationship { Id = 28, TaskId = 9, RelatedTaskId = 7, TaskRelationshipTypeId = 1 }, // Финальный дизайн UI → Прототипирование UI (FinishToStart)
        new TaskRelationship { Id = 29, TaskId = 10, RelatedTaskId = 9, TaskRelationshipTypeId = 1 }, // Утверждение дизайна → Финальный дизайн UI (FinishToStart)
        new TaskRelationship { Id = 30, TaskId = 11, RelatedTaskId = 10, TaskRelationshipTypeId = 1 }, // Sprint 1 Completion → Утверждение дизайна (FinishToStart)
        new TaskRelationship { Id = 31, TaskId = 12, RelatedTaskId = 11, TaskRelationshipTypeId = 1 }, // Разработка приложения → Sprint 1 Completion (FinishToStart)
        new TaskRelationship { Id = 32, TaskId = 13, RelatedTaskId = 11, TaskRelationshipTypeId = 1 }, // Настройка окружения → Sprint 1 Completion (FinishToStart)
        new TaskRelationship { Id = 33, TaskId = 14, RelatedTaskId = 13, TaskRelationshipTypeId = 1 }, // Разработка API → Настройка окружения (FinishToStart)
        new TaskRelationship { Id = 34, TaskId = 15, RelatedTaskId = 14, TaskRelationshipTypeId = 1 }, // Интеграция фронтенда → Разработка API (FinishToStart)
        new TaskRelationship { Id = 35, TaskId = 16, RelatedTaskId = 14, TaskRelationshipTypeId = 3 }, // Реализация авторизации → Разработка API (StartToStart)
        new TaskRelationship { Id = 36, TaskId = 17, RelatedTaskId = 15, TaskRelationshipTypeId = 1 }, // Завершение разработки → Интеграция фронтенда (FinishToStart)
        new TaskRelationship { Id = 37, TaskId = 17, RelatedTaskId = 16, TaskRelationshipTypeId = 1 }, // Завершение разработки → Реализация авторизации (FinishToStart)
        new TaskRelationship { Id = 38, TaskId = 18, RelatedTaskId = 17, TaskRelationshipTypeId = 1 }, // Тестирование приложения → Завершение разработки (FinishToStart)
        new TaskRelationship { Id = 39, TaskId = 19, RelatedTaskId = 17, TaskRelationshipTypeId = 1 }, // Unit-тесты → Завершение разработки (FinishToStart)
        new TaskRelationship { Id = 40, TaskId = 20, RelatedTaskId = 19, TaskRelationshipTypeId = 1 }, // Интеграционное тестирование → Unit-тесты (FinishToStart)
        new TaskRelationship { Id = 41, TaskId = 21, RelatedTaskId = 20, TaskRelationshipTypeId = 1 }, // Нагрузочное тестирование → Интеграционное тестирование (FinishToStart)
        new TaskRelationship { Id = 42, TaskId = 22, RelatedTaskId = 21, TaskRelationshipTypeId = 1 }, // Релиз бета-версии → Нагрузочное тестирование (FinishToStart)
        new TaskRelationship { Id = 43, TaskId = 23, RelatedTaskId = 22, TaskRelationshipTypeId = 1 }, // Запуск приложения → Релиз бета-версии (FinishToStart)
        new TaskRelationship { Id = 44, TaskId = 24, RelatedTaskId = 22, TaskRelationshipTypeId = 1 }, // Подготовка документации → Релиз бета-версии (FinishToStart)
        new TaskRelationship { Id = 45, TaskId = 25, RelatedTaskId = 22, TaskRelationshipTypeId = 1 }, // Маркетинговая кампания → Релиз бета-версии (FinishToStart)
        new TaskRelationship { Id = 46, TaskId = 26, RelatedTaskId = 24, TaskRelationshipTypeId = 1 }, // Публикация CRM → Подготовка документации (FinishToStart)
        new TaskRelationship { Id = 47, TaskId = 27, RelatedTaskId = 26, TaskRelationshipTypeId = 1 }, // Официальный релиз → Публикация CRM (FinishToStart)
        new TaskRelationship { Id = 48, TaskId = 27, RelatedTaskId = 25, TaskRelationshipTypeId = 1 }  // Официальный релиз → Маркетинговая кампания (FinishToStart)
    );
    }
}