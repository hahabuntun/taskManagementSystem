using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskConfiguration : IEntityTypeConfiguration<Tasks>
{
    public void Configure(EntityTypeBuilder<Tasks> builder)
    {
        // Set the table name and primary key for the Tasks entity
        builder.ToTable("tasks").HasKey(t => t.Id);

        // Configure basic properties
        // Task name is required and limited to 100 characters for storage efficiency
        builder.Property(t => t.ShortName)
            .IsRequired()
            .HasMaxLength(100);

        // Description is optional and can hold up to 1000 characters for detailed task info
        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        // Progress defaults to 0 for new tasks
        builder.Property(t => t.Progress)
            .HasDefaultValue(0);

        // Creation date is required and defaults to the current timestamp
        builder.Property(t => t.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Configure relationship with creator
        // Each task must have a creator (Worker), and a worker can create many tasks
        builder.HasOne(t => t.Creator)
            .WithMany(w => w.CreatorTasks)
            .HasForeignKey(t => t.CreatorId)
            .IsRequired();


        // Configure many-to-many relationship with executors via TaskExecutor
        // A task can have multiple executors, managed through the TaskExecutor join entity
        builder.HasMany(t => t.TaskExecutors)
            .WithOne(te => te.Task)
            .HasForeignKey(te => te.TaskId)
            .OnDelete(DeleteBehavior.Cascade); // Delete executor assignments when task is deleted

        // Configure relationship with task type
        // Each task must have a type, and a type can be associated with many tasks
        builder.HasOne(t => t.TaskType)
            .WithMany(t => t.TasksList)
            .HasForeignKey(t => t.TaskTypeId)
            .IsRequired();

        // Configure relationship with task status
        // Each task must have a status, and a status can be associated with many tasks
        builder.HasOne(t => t.TaskStatus)
            .WithMany(t => t.TasksList)
            .HasForeignKey(t => t.TaskStatusId)
            .IsRequired();

        // Configure relationship with project
        // Each task must belong to a project, and a project can have many tasks
        builder.HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .IsRequired();

        // Configure relationship with sprint
        // A task can optionally belong to a sprint, and a sprint can have many tasks
        builder.HasOne(t => t.Sprint)
             .WithMany(s => s.TasksList)
             .HasForeignKey(t => t.SprintId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.SetNull);

        // Configure relationship with task priority
        // A task can have an optional priority, and a priority can be associated with many tasks
        builder.HasOne(t => t.TaskPriority)
            .WithMany(p => p.TasksList)
            .HasForeignKey(t => t.TaskPriorityId);

        // Configure many-to-many relationship with observers
        // A task can have multiple observers (Workers), and a worker can observe many tasks
        builder.HasMany(t => t.TaskObservers)
            .WithOne(to => to.Task)
            .HasForeignKey(to => to.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship with task links
        // A task can have multiple external links, which are deleted if the task is deleted
        builder.HasMany(t => t.TaskLinks)
            .WithOne(tl => tl.Task)
            .HasForeignKey(tl => tl.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship with task messages
        // A task can have multiple messages, which are deleted if the task is deleted
        builder.HasMany(t => t.TaskMessages)
            .WithOne(m => m.RelatedTask)
            .HasForeignKey(tm => tm.RelatedTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure task relationships (dependencies)
        // A task can have multiple outgoing dependencies (TaskRelationships)
        builder.HasMany(t => t.TaskRelationships)
            .WithOne(tr => tr.Task)
            .HasForeignKey(tr => tr.TaskId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if task is referenced

        // A task can have multiple incoming dependencies (RelatedTaskRelationships)
        builder.HasMany(t => t.RelatedTaskRelationships)
            .WithOne(tr => tr.RelatedTask)
            .HasForeignKey(tr => tr.RelatedTaskId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion if task is referenced

        // Configure indexes for performance
        builder.HasIndex(t => t.ShortName); // Index for quick name lookups
        builder.HasIndex(t => t.ExpireOn); // Index for deadline queries
        builder.HasIndex(t => t.TaskStatusId); // Index for status-based filtering



         builder.HasData(
        // Sprint 1: Analysis, Design, Initial Development
        new Tasks { Id = 1, ShortName = "Анализ требований", Description = "Сбор и анализ требований для CRM", ProjectId = 1, SprintId = 1, CreatorId = 1, TaskTypeId = 3, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 15, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 2, ShortName = "Сбор требований", Description = "Интервью с заказчиком", ProjectId = 1, SprintId = 1, CreatorId = 1, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 5, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 3, ShortName = "Анализ конкурентов", Description = "Изучение аналогичных CRM систем", ProjectId = 1, SprintId = 1, CreatorId = 2, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 1, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 7, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 4, ShortName = "Составление ТЗ", Description = "Написание технического задания", ProjectId = 1, SprintId = 1, CreatorId = 1, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 6, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 12, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 5, ShortName = "Утверждение ТЗ", Description = "Подтверждение ТЗ заказчиком", ProjectId = 1, SprintId = 1, CreatorId = 1, TaskTypeId = 2, TaskStatusId = 1, TaskPriorityId = 3, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 13, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 13, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 6, ShortName = "Дизайн приложения", Description = "Создание UI/UX дизайна CRM", ProjectId = 1, SprintId = 1, CreatorId = 9, TaskTypeId = 3, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 14, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 31, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 7, ShortName = "Прототипирование UI", Description = "Создание прототипов интерфейса", ProjectId = 1, SprintId = 1, CreatorId = 9, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 14, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 20, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 8, ShortName = "Дизайн логотипа", Description = "Разработка логотипа CRM", ProjectId = 1, SprintId = 1, CreatorId = 9, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 1, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 14, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 18, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 9, ShortName = "Финальный дизайн UI", Description = "Окончательный дизайн интерфейса", ProjectId = 1, SprintId = 1, CreatorId = 9, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 21, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 31, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 10, ShortName = "Утверждение дизайна", Description = "Подтверждение дизайна заказчиком", ProjectId = 1, SprintId = 1, CreatorId = 9, TaskTypeId = 2, TaskStatusId = 1, TaskPriorityId = 3, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 31, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 31, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 11, ShortName = "Sprint 1 Completion", Description = "Complete Sprint 1 deliverables", ProjectId = 1, SprintId = 1, CreatorId = 1, TaskTypeId = 2, TaskStatusId = 1, TaskPriorityId = 3, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 5, 31, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 5, 31, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        // Sprint 2: Development, Testing, Deployment
        new Tasks { Id = 12, ShortName = "Разработка приложения", Description = "Кодинг основного функционала CRM", ProjectId = 1, SprintId = 2, CreatorId = 1, TaskTypeId = 3, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 15, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 13, ShortName = "Настройка окружения", Description = "Подготовка dev-окружения", ProjectId = 1, SprintId = 2, CreatorId = 11, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 1, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 6, 5, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 14, ShortName = "Разработка API", Description = "Создание REST API для CRM", ProjectId = 1, SprintId = 2, CreatorId = 11, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 6, 6, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 6, 20, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 15, ShortName = "Интеграция фронтенда", Description = "Подключение UI к API", ProjectId = 1, SprintId = 2, CreatorId = 12, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 6, 21, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 5, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 16, ShortName = "Реализация авторизации", Description = "Настройка логина и регистрации", ProjectId = 1, SprintId = 2, CreatorId = 11, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 6, 11, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 6, 25, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 17, ShortName = "Завершение разработки", Description = "Готовность базового функционала", ProjectId = 1, SprintId = 2, CreatorId = 1, TaskTypeId = 2, TaskStatusId = 1, TaskPriorityId = 3, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 5, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 5, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 18, ShortName = "Тестирование приложения", Description = "Проверка качества CRM", ProjectId = 1, SprintId = 2, CreatorId = 2, TaskTypeId = 3, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 6, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 20, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 19, ShortName = "Unit-тесты", Description = "Написание модульных тестов", ProjectId = 1, SprintId = 2, CreatorId = 2, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 1, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 6, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 12, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 20, ShortName = "Интеграционное тестирование", Description = "Тестирование интеграции модулей", ProjectId = 1, SprintId = 2, CreatorId = 2, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 13, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 18, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 21, ShortName = "Нагрузочное тестирование", Description = "Проверка производительности", ProjectId = 1, SprintId = 2, CreatorId = 2, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 19, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 23, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 22, ShortName = "Релиз бета-версии", Description = "Выпуск бета-версии CRM", ProjectId = 1, SprintId = 2, CreatorId = 1, TaskTypeId = 2, TaskStatusId = 1, TaskPriorityId = 3, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 24, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 24, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 23, ShortName = "Запуск приложения", Description = "Подготовка и выпуск финальной версии", ProjectId = 1, SprintId = 2, CreatorId = 1, TaskTypeId = 3, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 25, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 31, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 24, ShortName = "Подготовка документации", Description = "Создание пользовательской документации", ProjectId = 1, SprintId = 2, CreatorId = 12, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 1, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 25, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 29, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 25, ShortName = "Маркетинговая кампания", Description = "Запуск рекламы CRM", ProjectId = 1, SprintId = 2, CreatorId = 9, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 25, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 30, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 26, ShortName = "Публикация CRM", Description = "Размещение CRM в продакшен", ProjectId = 1, SprintId = 2, CreatorId = 11, TaskTypeId = 1, TaskStatusId = 1, TaskPriorityId = 2, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 30, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 31, 23, 59, 59, DateTimeKind.Utc), Progress = 0 },
        new Tasks { Id = 27, ShortName = "Официальный релиз", Description = "Выпуск финальной версии CRM", ProjectId = 1, SprintId = 2, CreatorId = 1, TaskTypeId = 2, TaskStatusId = 1, TaskPriorityId = 3, CreatedOn = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc), StartOn = new DateTime(2025, 7, 31, 0, 0, 0, DateTimeKind.Utc), ExpireOn = new DateTime(2025, 7, 31, 23, 59, 59, DateTimeKind.Utc), Progress = 0 }
    );
}
}