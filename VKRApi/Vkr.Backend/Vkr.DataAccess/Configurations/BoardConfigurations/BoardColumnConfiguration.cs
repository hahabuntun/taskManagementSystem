using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Board;

namespace Vkr.DataAccess.Configurations.BoardConfigurations;

public class BoardColumnConfiguration : IEntityTypeConfiguration<BoardColumns>
{
    public void Configure(EntityTypeBuilder<BoardColumns> builder)
    {
        builder.ToTable("board_columns").HasKey(bc => bc.Id);

        builder.Property(bc => bc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(bc => bc.BoardId)
            .IsRequired();

        builder.Property(bc => bc.Order)
            .IsRequired();

        builder.HasOne(bc => bc.Board)
            .WithMany(b => b.BoardColumns)
            .HasForeignKey(bc => bc.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(bc => new { bc.BoardId, bc.Name }).IsUnique();
    }
}