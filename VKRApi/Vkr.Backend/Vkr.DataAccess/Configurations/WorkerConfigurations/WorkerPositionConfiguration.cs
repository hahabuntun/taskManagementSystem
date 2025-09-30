using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Worker;

namespace Vkr.DataAccess.Configurations.WorkerConfigurations;

public class WorkerPositionConfiguration : IEntityTypeConfiguration<WorkerPosition>
{
    public void Configure(EntityTypeBuilder<WorkerPosition> builder)
    {
        builder.ToTable("worker_positions");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Title)
            .HasMaxLength(WorkerPosition.MaxTitleLength)
            .IsRequired();

        // Configure one-to-many relationship with TaskGiverRelations
        builder
            .HasMany(x => x.TaskGiverRelations)
            .WithOne(x => x.WorkerPosition)
            .HasForeignKey(x => x.WorkerPositionId);

        // Configure one-to-many relationship with TaskTakerRelations
        builder
            .HasMany(x => x.TaskTakerRelations)
            .WithOne(x => x.SubordinateWorkerPosition)
            .HasForeignKey(x => x.SubordinateWorkerPositionId);

        builder.HasData(new[]
        {
            new { Id = 1, Title = "Junior Developer", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 2, Title = "Mid-Level Developer", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 3, Title = "Senior Developer", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 4, Title = "Team Lead", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 5, Title = "Project Manager", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 6, Title = "Product Owner", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 7, Title = "Scrum Master", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 8, Title = "QA Engineer", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 9, Title = "DevOps Engineer", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 10, Title = "System Architect", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 11, Title = "UI/UX Designer", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 12, Title = "Business Analyst", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 13, Title = "Data Scientist", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 14, Title = "Database Administrator", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 15, Title = "Security Specialist", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 16, Title = "Technical Writer", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 17, Title = "Support Engineer", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 18, Title = "Marketing Specialist", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 19, Title = "HR Manager", TaskGiverRelations = new List<WorkerPositionRelation>() },
            new { Id = 20, Title = "Finance Analyst", TaskGiverRelations = new List<WorkerPositionRelation>() }
        });

    }
}