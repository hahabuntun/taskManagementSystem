using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Sprint;

namespace Vkr.DataAccess.Configurations.SprintConfigurations;

public class 
    SprintConfiguration : IEntityTypeConfiguration<Sprints>
{
    public void Configure(EntityTypeBuilder<Sprints> builder)
    {
        builder.ToTable("sprints").HasKey(s => s.Id);

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(s => s.StartsOn)
            .IsRequired();

        builder.Property(s => s.ExpireOn)
            .IsRequired();

        builder.Property(s => s.ProjectId)
            .IsRequired();

        builder.Property(s => s.SprintStatusId)
            .IsRequired();

        builder.HasOne(s => s.Project)
            .WithMany(p => p.Sprints)
            .HasForeignKey(s => s.ProjectId);

        builder.HasOne(s => s.SprintStatus)
            .WithMany(s => s.SprintsList)
            .HasForeignKey(s => s.SprintStatusId);

        builder.HasMany(s => s.TasksList)
            .WithOne(t => t.Sprint)
            .HasForeignKey(t => t.SprintId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(s => s.Title);

        builder.HasData(
            new Sprints
            {
                Id = 1,
                Title = "Sprint 1 - CRM",
                Goal = "Initial setup and core features",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                StartsOn = new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc),
                ExpireOn = new DateTime(2025, 6, 3, 23, 59, 59, DateTimeKind.Utc),
                ProjectId = 1,
                SprintStatusId = 1 // PLANNED
            },
            new Sprints
            {
                Id = 2,
                Title = "Sprint 2 - CRM",
                Goal = "User authentication and dashboard",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                StartsOn = new DateTime(2025, 6, 4, 0, 0, 0, DateTimeKind.Utc),
                ExpireOn = new DateTime(2025, 6, 18, 23, 59, 59, DateTimeKind.Utc),
                ProjectId = 1,
                SprintStatusId = 1 // PLANNED
            }
        );
    }
}