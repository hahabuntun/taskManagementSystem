using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskTypeConfiguration : IEntityTypeConfiguration<TaskType>
{
    public void Configure(EntityTypeBuilder<TaskType> builder)
    {
        // Set the table name and primary key for the TaskType entity
        builder.ToTable("task_types").HasKey(tt => tt.Id);

        // Configure the Name property
        // Name is required and limited to 100 characters
        builder.Property(tt => tt.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Configure relationship with tasks
        // A task type can be associated with many tasks
        builder.HasMany(tt => tt.TasksList)
            .WithOne(t => t.TaskType)
            .HasForeignKey(t => t.TaskTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of type if tasks exist

        // Ensure unique task type names
        builder.HasIndex(tt => tt.Name)
            .IsUnique();

        // Seed data for task types
        builder.HasData(
            new TaskType { Id = 1, Name = "Задача" }, // Regular task type
            new TaskType { Id = 2, Name = "Веха" },   // Milestone task type
             new TaskType { Id = 3, Name = "Сводная задача" }
        );
    }
}