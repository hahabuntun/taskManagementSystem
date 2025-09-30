using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskPriorityConfiguration : IEntityTypeConfiguration<TaskPriority>
{
    public void Configure(EntityTypeBuilder<TaskPriority> builder)
    {
        builder.ToTable("task_priority");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Color)
            .IsRequired()
            .HasMaxLength(20);


        builder.HasData(
                new TaskPriority { Id = 1, Name = "Низкий", Color = "blue" },
                new TaskPriority { Id = 2, Name = "Обычный", Color = "blue" },
                new TaskPriority { Id = 3, Name = "Высокий", Color = "blue" },
                new TaskPriority { Id = 4, Name = "Критичный", Color = "blue" }
            );
    }
}