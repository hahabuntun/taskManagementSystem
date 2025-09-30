using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations.TaskConfigurations;

public class TaskLinkConfiguration : IEntityTypeConfiguration<TaskLink>
{
    public void Configure(EntityTypeBuilder<TaskLink> builder)
    {
        builder.ToTable("task_links").HasKey(pl => pl.Id);

        // Настройка свойств
        builder.Property(pl => pl.Link)
            .IsRequired()
            .HasMaxLength(500); // Ограничение длины ссылки

        builder.Property(pl => pl.Description)
            .HasMaxLength(1000); // Ограничение длины описания

        builder.Property(pl => pl.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()"); // Текущая дата по умолчанию


        builder.HasOne(tl => tl.Task)
            .WithMany(t => t.TaskLinks)
            .HasForeignKey(tl => tl.TaskId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление при удалении проекта

        // Индексы
        builder.HasIndex(tl => tl.TaskId);


         builder.HasData(
            new
            {
                Id = 1,
                Link = "https://www.postgresql.org/docs/current/ddl.html",
                Description = "PostgreSQL documentation on DDL for designing database schemas.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 2,
                Link = "https://dbdesigner.net/",
                Description = "Online tool for creating ER diagrams for CRM database schema.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 3,
                Link = "https://www.lucidchart.com/pages/er-diagrams",
                Description = "Lucidchart guide for creating entity-relationship diagrams.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 4,
                Link = "https://www.sqlshack.com/learn-sql-database-normalization/",
                Description = "Article on database normalization techniques for schema design.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 5,
                Link = "https://github.com/example/crm-schema",
                Description = "GitHub repository with sample CRM database schema scripts.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 6,
                Link = "https://www.vertabelo.com/",
                Description = "Vertabelo tool for designing and visualizing database schemas.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 7,
                Link = "https://www.mysql.com/products/workbench/",
                Description = "MySQL Workbench for designing and managing database schemas.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 8,
                Link = "https://www.tutorialspoint.com/sql/sql-indexes.htm",
                Description = "Tutorial on SQL indexes for optimizing database performance.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 9,
                Link = "https://www.dofactory.com/sql/joins",
                Description = "Guide on SQL joins for designing relational schemas.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 10,
                Link = "https://www.dbml.org/",
                Description = "DBML language for defining database schemas in code.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 11,
                Link = "https://www.learnsql.com/blog/database-design/",
                Description = "Blog on best practices for database schema design.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 12,
                Link = "https://www.pgadmin.org/",
                Description = "pgAdmin tool for managing PostgreSQL database schemas.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 13,
                Link = "https://www.sqlstyle.guide/",
                Description = "SQL style guide for consistent schema naming conventions.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 14,
                Link = "https://www.datacamp.com/courses/database-design",
                Description = "DataCamp course on database design fundamentals.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 15,
                Link = "https://www.draw.io/",
                Description = "Draw.io for creating ER diagrams for CRM schema.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 16,
                Link = "https://www.sqlservercentral.com/articles/database-design",
                Description = "Article on database design principles for relational databases.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 17,
                Link = "https://www.red-gate.com/products/sql-toolbelt/",
                Description = "Redgate SQL Toolbelt for schema management and comparison.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 18,
                Link = "https://www.sqlitetutorial.net/sqlite-constraints/",
                Description = "SQLite tutorial on constraints for schema integrity.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 19,
                Link = "https://www.mongodb.com/docs/manual/core/data-modeling/",
                Description = "MongoDB data modeling guide for hybrid schema design.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            },
            new
            {
                Id = 20,
                Link = "https://www.pluralsight.com/courses/database-design",
                Description = "Pluralsight course on designing efficient database schemas.",
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                TaskId = 1
            }
        );
    }
}