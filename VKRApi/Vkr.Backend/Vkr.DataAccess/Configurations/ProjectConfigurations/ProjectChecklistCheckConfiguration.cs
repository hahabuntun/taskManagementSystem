using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Progect;

namespace Vkr.DataAccess.Configurations.ProjectConfigurations;

public class ProjectChecklistCheckConfiguration: IEntityTypeConfiguration<ProjectChecklistCheck>
{
    public void Configure(EntityTypeBuilder<ProjectChecklistCheck> builder)
    {
        builder.ToTable("project_checklist_checks").HasKey(pcc => pcc.Id);

        // Базовые свойства
        builder.Property(pcc => pcc.Tittle)
            .HasMaxLength(200); // Опечатка в свойстве (Tittle вместо Title)
            
        builder.Property(pcc => pcc.Description)
            .HasMaxLength(1000);
            
        builder.Property(pcc => pcc.IsChecked)
            .IsRequired()
            .HasDefaultValue(false);

        // Связь с чек-листом
        builder.HasOne(pcc => pcc.ProjectChecklist)
            .WithMany(pc => pc.Checks)
            .HasForeignKey(pcc => pcc.ProjectChecklistId)
            .IsRequired();
    }
}