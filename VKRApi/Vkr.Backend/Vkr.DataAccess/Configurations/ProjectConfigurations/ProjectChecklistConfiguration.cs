using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Progect;

namespace Vkr.DataAccess.Configurations.ProjectConfigurations;

public class ProjectChecklistConfiguration : IEntityTypeConfiguration<ProjectChecklist>
{
    public void Configure(EntityTypeBuilder<ProjectChecklist> builder)
    {
        builder.ToTable("project_checklists").HasKey(pc => pc.Id);

        // Базовые свойства
        builder.Property(pc => pc.Title)
            .HasMaxLength(200);
        
        builder.Property(pc => pc.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Связь с создателем (работником)
        builder.HasOne(pc => pc.Creator)
            .WithMany(w => w.ProjectChecklists)
            .HasForeignKey(pc => pc.WorkerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Связь с проектом
        builder.HasOne(pc => pc.Project)
            .WithMany(p => p.ProjectChecklists)
            .HasForeignKey(pc => pc.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Связь один-ко-многим с пунктами чек-листа
        builder.HasMany(pc => pc.Checks)
            .WithOne(c => c.ProjectChecklist)
            .HasForeignKey(c => c.ProjectChecklistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Индексы
        builder.HasIndex(pc => pc.ProjectId);
        builder.HasIndex(pc => pc.WorkerId);
        builder.HasIndex(pc => pc.CreatedOn);
    }
}