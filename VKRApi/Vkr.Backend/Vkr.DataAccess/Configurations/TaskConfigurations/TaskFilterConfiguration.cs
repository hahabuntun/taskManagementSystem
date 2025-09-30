using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities;

namespace Vkr.DataAccess.Configurations;

public class TaskFilterConfiguration : IEntityTypeConfiguration<TaskFilter>
{
    public void Configure(EntityTypeBuilder<TaskFilter> builder)
    {
        builder.ToTable("task_filters").HasKey(tf => tf.Name);

        builder.Property(tf => tf.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tf => tf.OptionsJson)
            .IsRequired()
            .HasColumnType("jsonb"); // Use jsonb for PostgreSQL; adjust for other databases (e.g., "nvarchar(max)" for SQL Server)

        builder.HasIndex(tf => tf.Name).IsUnique();
    }
}