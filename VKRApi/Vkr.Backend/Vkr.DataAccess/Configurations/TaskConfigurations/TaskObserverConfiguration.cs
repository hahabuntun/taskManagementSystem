using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskObserverConfiguration : IEntityTypeConfiguration<TaskObserver>
{
    public void Configure(EntityTypeBuilder<TaskObserver> builder)
    {
        // Set the table name for the TaskObserver join entity
        builder.ToTable("task_observers");

        // Configure composite primary key (TaskId, WorkerId)
        builder.HasKey(to => new { to.TaskId, to.WorkerId });

        // Configure relationship with Task
        // Each TaskObserver is associated with one task
        builder.HasOne(to => to.Task)
            .WithMany(t => t.TaskObservers)
            .HasForeignKey(to => to.TaskId)
            .OnDelete(DeleteBehavior.Cascade); // Delete observer assignments when task is deleted

        // Configure relationship with Worker
        // Each TaskObserver is associated with one worker
        builder.HasOne(to => to.Worker)
            .WithMany(w => w.TaskObservers)
            .HasForeignKey(to => to.WorkerId)
            .OnDelete(DeleteBehavior.Cascade); // Delete observer assignments when worker is deleted

        // Configure AssignedOn property
        // Date when the worker was assigned as an observer, defaults to current timestamp
        builder.Property(to => to.AssignedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Optional: Add index for performance
        builder.HasIndex(to => to.AssignedOn);
    }
}