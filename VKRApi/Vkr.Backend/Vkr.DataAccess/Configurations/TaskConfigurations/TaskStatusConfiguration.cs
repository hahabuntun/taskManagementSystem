using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskStatus = Vkr.Domain.Entities.Task.TaskStatus;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

internal class TaskStatusConfiguration : IEntityTypeConfiguration<TaskStatus>
{
    public void Configure(EntityTypeBuilder<TaskStatus> builder)
    {
        builder.ToTable("task_statuses");
        builder.HasKey(x => x.Id);


        builder
            .Property(s => s.Name)
            .IsRequired();

        builder.HasData(
            new TaskStatus { Id = 1, Name = "В ожидании", Color = "blue" },
            new TaskStatus { Id = 2, Name = "В работе",  Color = "blue" },
            new TaskStatus { Id = 3, Name = "На проверке",  Color = "blue" },
            new TaskStatus { Id = 4, Name = "Завершена",  Color = "blue" },
            new TaskStatus { Id = 5, Name = "Приостановлен",  Color = "blue" }
            );
    }
}