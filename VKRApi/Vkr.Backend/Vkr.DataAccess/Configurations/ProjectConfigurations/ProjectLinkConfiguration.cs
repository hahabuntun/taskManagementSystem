using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Progect;

namespace Vkr.DataAccess.Configurations.ProjectConfigurations;

public class ProjectLinkConfiguration : IEntityTypeConfiguration<ProjectLink>
{
    public void Configure(EntityTypeBuilder<ProjectLink> builder)
    {
        builder.ToTable("project_links").HasKey(pl => pl.Id);

        // Настройка свойств
        builder.Property(pl => pl.Link)
            .IsRequired()
            .HasMaxLength(500); // Ограничение длины ссылки

        builder.Property(pl => pl.Description)
            .HasMaxLength(1000); // Ограничение длины описания

        builder.Property(pl => pl.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()"); // Текущая дата по умолчанию


        // Связь с проектом
        builder.HasOne(pl => pl.Projects)
            .WithMany(p => p.ProjectLinks)
            .HasForeignKey(pl => pl.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление при удалении проекта

        // Индексы
        builder.HasIndex(pl => pl.ProjectId);

        builder.HasData(
            new
            {
                Id = 1,
                Link = "https://www.atlassian.com/software/jira",
                Description = "Jira for tracking CRM project tasks and sprints.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 2,
                Link = "https://www.confluence.com/",
                Description = "Confluence for CRM project documentation and collaboration.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 3,
                Link = "https://github.com/example/crm-project",
                Description = "GitHub repository for CRM project source code.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 4,
                Link = "https://www.figma.com/",
                Description = "Figma for designing CRM UI/UX prototypes.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 5,
                Link = "https://swagger.io/",
                Description = "Swagger for documenting CRM API endpoints.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 6,
                Link = "https://docs.microsoft.com/en-us/ef/core/",
                Description = "Entity Framework Core docs for CRM backend development.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 7,
                Link = "https://reactjs.org/",
                Description = "React documentation for CRM frontend development.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 8,
                Link = "https://www.postman.com/",
                Description = "Postman for testing CRM API endpoints.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 9,
                Link = "https://www.salesforce.com/products/what-is-crm/",
                Description = "Salesforce guide on CRM systems for reference.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 10,
                Link = "https://www.trello.com/",
                Description = "Trello for managing CRM project tasks and workflows.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 11,
                Link = "https://www.notion.so/",
                Description = "Notion for CRM project notes and planning.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 12,
                Link = "https://www.zoho.com/crm/",
                Description = "Zoho CRM for inspiration and feature benchmarking.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 13,
                Link = "https://www.gitlab.com/",
                Description = "GitLab for CI/CD pipeline setup for CRM project.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 14,
                Link = "https://www.cypress.io/",
                Description = "Cypress for end-to-end testing of CRM frontend.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 15,
                Link = "https://jestjs.io/",
                Description = "Jest for unit testing CRM frontend components.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 16,
                Link = "https://www.auth0.com/",
                Description = "Auth0 for CRM authentication and user management.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 17,
                Link = "https://www.docker.com/",
                Description = "Docker for containerizing CRM application services.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 18,
                Link = "https://www.mongodb.com/docs/atlas/",
                Description = "MongoDB Atlas for potential NoSQL integration in CRM.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 19,
                Link = "https://www.slack.com/",
                Description = "Slack for CRM project team communication.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            },
            new
            {
                Id = 20,
                Link = "https://www.pluralsight.com/courses/building-crm",
                Description = "Pluralsight course on building CRM systems.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                ProjectId = 1
            }
        );
    }
}