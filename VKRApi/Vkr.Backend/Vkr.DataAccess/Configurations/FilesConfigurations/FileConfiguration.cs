using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = Vkr.Domain.Entities.Files.File;

namespace Vkr.DataAccess.Configurations.FilesConfigurations;

public class FileConfiguration<T> : IEntityTypeConfiguration<T>
    where T: File 
{
    public virtual void Configure(EntityTypeBuilder<T> b)
    {
        b.ToTable("files");
        b.HasKey(f => f.Id);
        b.Property(f => f.OwnerType).IsRequired();
        b.Property(f => f.OwnerId).IsRequired();
        b.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(File.NameMaxLength);
        b.Property(f => f.Description)
            .HasMaxLength(File.DescriptionMaxLength);
        b.Property(f => f.Key).IsRequired();
        b.Property(f => f.CreatedAt)
            .HasColumnType("timestamp with time zone");
        b.Property(f => f.CreatorId).IsRequired();
        b.HasIndex(f => new { f.OwnerType, f.OwnerId });
    }
}