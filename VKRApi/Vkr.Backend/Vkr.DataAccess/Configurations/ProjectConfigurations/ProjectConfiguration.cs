using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Organization;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Worker;

namespace Vkr.DataAccess.Configurations.ProjectConfigurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Projects>
{
    public void Configure(EntityTypeBuilder<Projects> builder)
    {
        builder.ToTable("project").HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder
            .Property(x => x.Description)
            .HasMaxLength(10000);

        builder
            .Property(x => x.ManagerId)
            .IsRequired();

        builder
            .Property(x => x.OrganizationId)
            .IsRequired();

        builder.Property(x => x.CreatedOn)
            .HasDefaultValueSql("NOW()");

        builder.Property(x => x.ProjectStatusId);

        builder
            .HasOne(x => x.Organization)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.OrganizationId);

        builder
            .HasOne(x => x.ProjectStatus)
            .WithMany(y => y.ProjectsList)
            .HasForeignKey(x => x.ProjectStatusId);

        builder
            .HasOne(x => x.Manager)
            .WithMany()
            .HasForeignKey(x => x.ManagerId);

        // Configure many-to-many relationship with Workers and seed WorkerProgect
        builder
            .HasMany(x => x.WorkersList)
            .WithMany(x => x.ProjectsList)
            .UsingEntity(j =>
            {
                j.ToTable("WorkerProgect");
                j.HasKey("ProjectsListId", "WorkersListId");
                j.HasData(
                    // Project 1: Иван Петров (Manager) + 4 workers
                    new { ProjectsListId = 1, WorkersListId = 1 }, // Иван Петров
                    new { ProjectsListId = 1, WorkersListId = 2 }, // Анна Смирнова
                    new { ProjectsListId = 1, WorkersListId = 9 }, // Сергей Федоров
                    new { ProjectsListId = 1, WorkersListId = 11 }, // Андрей Воробьев
                    new { ProjectsListId = 1, WorkersListId = 12 }, // Юлия Григорьева

                    // Project 2: Михаил Иванов (Manager) + 3 workers
                    new { ProjectsListId = 2, WorkersListId = 3 }, // Михаил Иванов
                    new { ProjectsListId = 2, WorkersListId = 13 }, // Павел Сидоров
                    new { ProjectsListId = 2, WorkersListId = 14 }, // Татьяна Лебедева
                    new { ProjectsListId = 2, WorkersListId = 15 }, // Виктор Белов

                    // Project 3: Екатерина Козлова (Manager) + 4 workers
                    new { ProjectsListId = 3, WorkersListId = 4 }, // Екатерина Козлова
                    new { ProjectsListId = 3, WorkersListId = 16 }, // Мария Орлова
                    new { ProjectsListId = 3, WorkersListId = 17 }, // Роман Зайцев
                    new { ProjectsListId = 3, WorkersListId = 18 }, // Светлана Егорова
                    new { ProjectsListId = 3, WorkersListId = 19 }, // Владимир Крылов

                    // Project 4: Александр Соколов (Manager) + 3 workers
                    new { ProjectsListId = 4, WorkersListId = 5 }, // Александр Соколов
                    new { ProjectsListId = 4, WorkersListId = 6 }, // Ольга Новикова
                    new { ProjectsListId = 4, WorkersListId = 20 }, // Ксения Васильева
                    new { ProjectsListId = 4, WorkersListId = 8 }, // Елена Попова

                    // Project 5: Дмитрий Морозов (Manager) + 3 workers
                    new { ProjectsListId = 5, WorkersListId = 7 }, // Дмитрий Морозов
                    new { ProjectsListId = 5, WorkersListId = 10 }, // Наталья Кузнецова
                    new { ProjectsListId = 5, WorkersListId = 11 }, // Андрей Воробьев
                    new { ProjectsListId = 5, WorkersListId = 12 } // Юлия Григорьева
                );
            });

        // Seed default projects
        builder.HasData(
            new Projects
            {
                Id = 1,
                Name = "Разработка CRM-системы",
                Description = "Создание CRM-системы для управления клиентами и продажами.",
                ManagerId = 1, // Иван Петров
                OrganizationId = 1,
                ProjectStatusId = 1, // Active
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                Progress = 10
            },
            new Projects
            {
                Id = 2,
                Name = "Мобильное приложение для доставки",
                Description = "Разработка мобильного приложения для службы доставки еды.",
                ManagerId = 3, // Михаил Иванов
                OrganizationId = 1,
                ProjectStatusId = 1, // Active
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                Progress = 20
            },
            new Projects
            {
                Id = 3,
                Name = "Автоматизация складского учета",
                Description = "Внедрение системы автоматизации учета на складе.",
                ManagerId = 4, // Екатерина Козлова
                OrganizationId = 1,
                ProjectStatusId = 2, // InProgress
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                Progress = 30
            },
            new Projects
            {
                Id = 4,
                Name = "Корпоративный портал",
                Description = "Создание внутреннего портала для сотрудников компании.",
                ManagerId = 5, // Александр Соколов
                OrganizationId = 1,
                ProjectStatusId = 1, // Active
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                Progress = 15
            },
            new Projects
            {
                Id = 5,
                Name = "Маркетинговая кампания",
                Description = "Планирование и запуск маркетинговой кампании для нового продукта.",
                ManagerId = 7, // Дмитрий Морозов
                OrganizationId = 1,
                ProjectStatusId = 3, // Planning
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                Progress = 5

            }
        );
    }
}