using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Board;

namespace Vkr.DataAccess.Configurations.BoardConfigurations;

public class BoardTaskConfiguration : IEntityTypeConfiguration<BoardTask>
{
    public void Configure(EntityTypeBuilder<BoardTask> builder)
    {
        builder.ToTable("board_tasks").HasKey(bt => new { bt.BoardId, bt.TaskId });

        builder.Property(bt => bt.CustomColumnName)
            .HasMaxLength(100);

        builder.HasOne(bt => bt.Board)
            .WithMany(b => b.BoardTasks) // Changed from TasksList to BoardTasks
            .HasForeignKey(bt => bt.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bt => bt.Task)
            .WithMany(t => t.BoardTasks) // Changed from BoardsList to BoardTasks
            .HasForeignKey(bt => bt.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(bt => new { bt.BoardId, bt.TaskId }).IsUnique();
    }
}