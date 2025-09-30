using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Worker;

namespace Vkr.DataAccess.Configurations.WorkerConfigurations;

public class WorkerPositionRelationConfiguration : IEntityTypeConfiguration<WorkerPositionRelation>
{
    public void Configure(EntityTypeBuilder<WorkerPositionRelation> builder)
    {
        builder.ToTable("worker_position_relations");

        // Configure primary key
        builder.HasKey(x => x.Id);

        // Configure foreign key to WorkerPosition (the position assigning tasks)
        builder
            .HasOne(x => x.WorkerPosition)
            .WithMany(wp => wp.TaskGiverRelations)
            .HasForeignKey(x => x.WorkerPositionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure foreign key to SubordinateWorkerPosition (the position receiving tasks)
        builder
            .HasOne(x => x.SubordinateWorkerPosition)
            .WithMany(wp => wp.TaskTakerRelations)
            .HasForeignKey(x => x.SubordinateWorkerPositionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}