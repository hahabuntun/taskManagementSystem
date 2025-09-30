using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskExecutorConfiguration : IEntityTypeConfiguration<TaskExecutor>
{
    public void Configure(EntityTypeBuilder<TaskExecutor> builder)
    {
        // Set the table name for the TaskExecutor join entity
        builder.ToTable("task_executors");

        // Configure composite primary key (TaskId, WorkerId)
        builder.HasKey(te => new { te.TaskId, te.WorkerId });

        // Configure relationship with Task
        // Each TaskExecutor is associated with one task
        builder.HasOne(te => te.Task)
            .WithMany(t => t.TaskExecutors)
            .HasForeignKey(te => te.TaskId)
            .OnDelete(DeleteBehavior.Cascade); // Delete executor assignments when task is deleted

        // Configure relationship with Worker
        // Each TaskExecutor is associated with one worker
        builder.HasOne(te => te.Worker)
            .WithMany(w => w.TaskExecutors)
            .HasForeignKey(te => te.WorkerId)
            .OnDelete(DeleteBehavior.Cascade); // Delete executor assignments when worker is deleted

        // Configure IsResponsible property
        // Indicates if the worker is the responsible executor for the task
        builder.Property(te => te.IsResponsible)
            .IsRequired()
            .HasDefaultValue(false); // Default to false, only one executor can be responsible


        builder.HasData(
        // Sprint 1: Analysis, Design
        // Иван Петров (Id: 1, Manager): Milestones and high-level tasks
        new TaskExecutor { TaskId = 1, WorkerId = 1, IsResponsible = true }, // Анализ требований
        new TaskExecutor { TaskId = 2, WorkerId = 1, IsResponsible = true }, // Сбор требований
        new TaskExecutor { TaskId = 4, WorkerId = 1, IsResponsible = true }, // Составление ТЗ
        new TaskExecutor { TaskId = 5, WorkerId = 1, IsResponsible = true }, // Утверждение ТЗ
        new TaskExecutor { TaskId = 11, WorkerId = 1, IsResponsible = true }, // Sprint 1 Completion
        // Анна Смирнова (Id: 2, Developer): Analysis tasks
        new TaskExecutor { TaskId = 3, WorkerId = 2, IsResponsible = true }, // Анализ конкурентов
        new TaskExecutor { TaskId = 2, WorkerId = 2, IsResponsible = false }, // Сбор требований (assisting)
        new TaskExecutor { TaskId = 4, WorkerId = 2, IsResponsible = false }, // Составление ТЗ (assisting)
        // Сергей Федоров (Id: 9, Developer): Design tasks
        new TaskExecutor { TaskId = 6, WorkerId = 9, IsResponsible = true }, // Дизайн приложения
        new TaskExecutor { TaskId = 7, WorkerId = 9, IsResponsible = true }, // Прототипирование UI
        new TaskExecutor { TaskId = 8, WorkerId = 9, IsResponsible = true }, // Дизайн логотипа
        new TaskExecutor { TaskId = 9, WorkerId = 9, IsResponsible = true }, // Финальный дизайн UI
        new TaskExecutor { TaskId = 10, WorkerId = 9, IsResponsible = true }, // Утверждение дизайна
        // Sprint 2: Development, Testing, Deployment
        // Иван Петров (Id: 1, Manager): Milestones
        new TaskExecutor { TaskId = 12, WorkerId = 1, IsResponsible = true }, // Разработка приложения
        new TaskExecutor { TaskId = 17, WorkerId = 1, IsResponsible = true }, // Завершение разработки
        new TaskExecutor { TaskId = 22, WorkerId = 1, IsResponsible = true }, // Релиз бета-версии
        new TaskExecutor { TaskId = 23, WorkerId = 1, IsResponsible = true }, // Запуск приложения
        new TaskExecutor { TaskId = 27, WorkerId = 1, IsResponsible = true }, // Официальный релиз
        // Анна Смирнова (Id: 2, Developer): Testing tasks
        new TaskExecutor { TaskId = 18, WorkerId = 2, IsResponsible = true }, // Тестирование приложения
        new TaskExecutor { TaskId = 19, WorkerId = 2, IsResponsible = true }, // Unit-тесты
        new TaskExecutor { TaskId = 20, WorkerId = 2, IsResponsible = true }, // Интеграционное тестирование
        new TaskExecutor { TaskId = 21, WorkerId = 2, IsResponsible = true }, // Нагрузочное тестирование
        // Андрей Воробьев (Id: 11, Developer): Backend/Frontend tasks
        new TaskExecutor { TaskId = 13, WorkerId = 11, IsResponsible = true }, // Настройка окружения
        new TaskExecutor { TaskId = 14, WorkerId = 11, IsResponsible = true }, // Разработка API
        new TaskExecutor { TaskId = 16, WorkerId = 11, IsResponsible = true }, // Реализация авторизации
        new TaskExecutor { TaskId = 15, WorkerId = 11, IsResponsible = false }, // Интеграция фронтенда (assisting)
        new TaskExecutor { TaskId = 26, WorkerId = 11, IsResponsible = true }, // Публикация CRM
        // Юлия Григорьева (Id: 12, Tester): Frontend/Deployment tasks
        new TaskExecutor { TaskId = 15, WorkerId = 12, IsResponsible = true }, // Интеграция фронтенда
        new TaskExecutor { TaskId = 24, WorkerId = 12, IsResponsible = true }, // Подготовка документации
        new TaskExecutor { TaskId = 25, WorkerId = 12, IsResponsible = true }, // Маркетинговая кампания
        new TaskExecutor { TaskId = 19, WorkerId = 12, IsResponsible = false }, // Unit-тесты (assisting)
        new TaskExecutor { TaskId = 20, WorkerId = 12, IsResponsible = false } // Интеграционное тестирование (assisting)
    );
    }
}