using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Board;

namespace Vkr.DataAccess.Configurations.BoardConfigurations;

public class BoardConfiguration : IEntityTypeConfiguration<Boards>
{
    public void Configure(EntityTypeBuilder<Boards> builder)
    {
        builder.ToTable("boards").HasKey(b => b.Id);
        builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Description).HasMaxLength(500);
        builder.Property(b => b.CreatedOn).IsRequired().HasDefaultValueSql("NOW()");
        
        builder.HasOne(b => b.Owner)
            .WithMany(w => w.WorkerBoards)
            .HasForeignKey(b => b.OwnerId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Project)
            .WithMany(p => p.BoardsProgect)
            .HasForeignKey(b => b.ProjectId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b => b.BoardColumns)
            .WithOne(bc => bc.Board)
            .HasForeignKey(bc => bc.BoardId);

        builder.HasMany(b => b.BoardTasks) // Changed from TasksList to BoardTasks
            .WithOne(bt => bt.Board)
            .HasForeignKey(bt => bt.BoardId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasIndex(b => b.Name);
        builder.HasIndex(b => b.OwnerId);
        builder.HasIndex(b => b.ProjectId);
    }
}