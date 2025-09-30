using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Worker;

namespace Vkr.DataAccess.Configurations.ProjectConfigurations;

public class ProjectMemberManagementConfiguration : IEntityTypeConfiguration<ProjectMemberManagement>
{
    public void Configure(EntityTypeBuilder<ProjectMemberManagement> builder)
    {
        builder.ToTable("project_member_managements").HasKey(x => x.Id);

        builder
            .HasOne(x => x.Worker)
            .WithMany()
            .HasForeignKey(x => x.WorkerId);

        builder
            .HasOne(x => x.Subordinate)
            .WithMany()
            .HasForeignKey(x => x.SubordinateId);

        builder
            .HasOne(x => x.Project)
            .WithMany(x => x.ProjectMemberManagements)
            .HasForeignKey(x => x.ProjectId);

        // Seed ProjectMemberManagement to define management hierarchies
        builder.HasData(
            // Project 1: Иван Петров manages Анна Смирнова and Сергей Федоров
            new ProjectMemberManagement
            {
                Id = 1,
                WorkerId = 1, // Иван Петров
                ProjectId = 1,
                SubordinateId = 2 // Анна Смирнова
            },
            new ProjectMemberManagement
            {
                Id = 2,
                WorkerId = 1,
                ProjectId = 1,
                SubordinateId = 9 // Сергей Федоров
            },
            // Project 2: Михаил Иванов manages Павел Сидоров
            new ProjectMemberManagement
            {
                Id = 3,
                WorkerId = 3, // Михаил Иванов
                ProjectId = 2,
                SubordinateId = 13 // Павел Сидоров
            },
            // Project 3: Екатерина Козлова manages Мария Орлова and Роман Зайцев
            new ProjectMemberManagement
            {
                Id = 4,
                WorkerId = 4, // Екатерина Козлова
                ProjectId = 3,
                SubordinateId = 16 // Мария Орлова
            },
            new ProjectMemberManagement
            {
                Id = 5,
                WorkerId = 4,
                ProjectId = 3,
                SubordinateId = 17 // Роман Зайцев
            },
            // Project 4: Александр Соколов manages Ольга Новикова
            new ProjectMemberManagement
            {
                Id = 6,
                WorkerId = 5, // Александр Соколов
                ProjectId = 4,
                SubordinateId = 6 // Ольга Новикова
            },
            // Project 5: Дмитрий Морозов manages Наталья Кузнецова
            new ProjectMemberManagement
            {
                Id = 7,
                WorkerId = 7, // Дмитрий Морозов
                ProjectId = 5,
                SubordinateId = 10 // Наталья Кузнецова
            }
        );
    }
}