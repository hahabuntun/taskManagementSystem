using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskRelationshipTypeConfiguration : IEntityTypeConfiguration<TaskRelationshipType>
{
    public void Configure(EntityTypeBuilder<TaskRelationshipType> builder)
    {
        builder.ToTable("task_relationship_types").HasKey(trt => trt.Id);
        builder.Property(trt => trt.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(trt => trt.Name).IsUnique();
        builder.HasData(
            new TaskRelationshipType { Id = 1, Name = "Финиш-Старт" },
            new TaskRelationshipType { Id = 2, Name = "Финиш-Финиш" },
            new TaskRelationshipType { Id = 3, Name = "Старт-Старт" },
            new TaskRelationshipType { Id = 4, Name = "Старт-Финиш" },
            new TaskRelationshipType { Id = 5, Name = "Подзадача" } // Parent -> Child (subtask)
        );
    }
}