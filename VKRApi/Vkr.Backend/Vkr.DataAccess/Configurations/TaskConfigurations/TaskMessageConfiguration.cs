using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskMessageConfiguration : IEntityTypeConfiguration<TaskMessage>
{
    public void Configure(EntityTypeBuilder<TaskMessage> builder)
    {
        builder.ToTable("task_messages").HasKey(tm => tm.Id);

        // Настройка свойств
        builder.Property(tm => tm.Id)
            .ValueGeneratedOnAdd(); // Автогенерация ID

        builder.Property(tm => tm.MessageText)
            .IsRequired()
            .HasMaxLength(2000); // Ограничение длины сообщения

        builder.Property(tm => tm.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()"); // Текущая дата по умолчанию

        // Связь с отправителем (работником)
        builder.HasOne(tm => tm.Sender)
            .WithMany(w => w.WorkerMessages)
            .HasForeignKey(tm => tm.SenderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Запрет удаления если есть сообщения

        // Связь с задачей
        builder.HasOne(tm => tm.RelatedTask)
            .WithMany(t => t.TaskMessages)
            .HasForeignKey(tm => tm.RelatedTaskId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление при удалении задачи

        // Индексы для оптимизации запросов
        builder.HasIndex(tm => tm.RelatedTaskId);
        builder.HasIndex(tm => tm.SenderId);
    }
}