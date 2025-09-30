using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tags>
{
    public void Configure(EntityTypeBuilder<Tags> builder)
    {
        builder.ToTable("Tags");

        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.Name)
               .IsUnique();

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(t => t.Color)
               .IsRequired()
               .HasMaxLength(20); // Assuming hex code or color name

        // Связь с проектами
        builder.HasMany(t => t.Projects)
               .WithMany(p => p.Tags)
               .UsingEntity<Dictionary<string, object>>(
                   "ProjectTag",
                   j => j.HasOne<Projects>().WithMany().HasForeignKey("ProjectId"),
                   j => j.HasOne<Tags>().WithMany().HasForeignKey("TagId"),
                   j =>
                   {
                       j.HasKey("TagId", "ProjectId");
                       j.ToTable("ProjectTag");
                       // Seed Project-Tag relationships
                       j.HasData(
                           // Project 1: CRM
                           new { TagId = 1, ProjectId = 1 }, // CRM
                           new { TagId = 2, ProjectId = 1 }, // WebApp
                                                             // Project 2: Mobile App
                           new { TagId = 3, ProjectId = 2 }, // MobileApp
                           new { TagId = 4, ProjectId = 2 }, // Delivery
                                                             // Project 3: Warehouse
                           new { TagId = 5, ProjectId = 3 }, // Automation
                           new { TagId = 6, ProjectId = 3 }, // Warehouse
                                                             // Project 4: Portal
                           new { TagId = 7, ProjectId = 4 }, // Portal
                           new { TagId = 2, ProjectId = 4 }, // WebApp
                                                             // Project 5: Marketing
                           new { TagId = 8, ProjectId = 5 }, // Marketing
                           new { TagId = 9, ProjectId = 5 }  // Campaign
                       );
                   });

        // Связь с задачами
        builder.HasMany(t => t.Tasks)
               .WithMany(t => t.Tags)
               .UsingEntity<Dictionary<string, object>>(
                   "TaskTag",
                   j => j.HasOne<Tasks>().WithMany().HasForeignKey("TaskId"),
                   j => j.HasOne<Tags>().WithMany().HasForeignKey("TagId"),
                   j =>
                   {
                       j.HasKey("TagId", "TaskId");
                       j.ToTable("TaskTag");
                       // Seed Task-Tag relationships
                       j.HasData(
                           // Sprint 1: Analysis, Design
                           new { TagId = 10, TaskId = 1 }, // Analysis
                           new { TagId = 11, TaskId = 1 }, // Sprint1
                           new { TagId = 10, TaskId = 2 }, // Analysis
                           new { TagId = 11, TaskId = 2 }, // Sprint1
                           new { TagId = 10, TaskId = 3 }, // Analysis
                           new { TagId = 11, TaskId = 3 }, // Sprint1
                           new { TagId = 10, TaskId = 4 }, // Analysis
                           new { TagId = 11, TaskId = 4 }, // Sprint1
                           new { TagId = 12, TaskId = 5 }, // Milestone
                           new { TagId = 11, TaskId = 5 }, // Sprint1
                           new { TagId = 13, TaskId = 6 }, // Design
                           new { TagId = 11, TaskId = 6 }, // Sprint1
                           new { TagId = 13, TaskId = 7 }, // Design
                           new { TagId = 11, TaskId = 7 }, // Sprint1
                           new { TagId = 13, TaskId = 8 }, // Design
                           new { TagId = 11, TaskId = 8 }, // Sprint1
                           new { TagId = 13, TaskId = 9 }, // Design
                           new { TagId = 11, TaskId = 9 }, // Sprint1
                           new { TagId = 12, TaskId = 10 }, // Milestone
                           new { TagId = 11, TaskId = 10 }, // Sprint1
                           new { TagId = 12, TaskId = 11 }, // Milestone
                           new { TagId = 11, TaskId = 11 }, // Sprint1
                                                            // Sprint 2: Development, Testing, Deployment
                           new { TagId = 14, TaskId = 12 }, // Development
                           new { TagId = 15, TaskId = 12 }, // Sprint2
                           new { TagId = 14, TaskId = 13 }, // Development
                           new { TagId = 15, TaskId = 13 }, // Sprint2
                           new { TagId = 14, TaskId = 14 }, // Development
                           new { TagId = 15, TaskId = 14 }, // Sprint2
                           new { TagId = 14, TaskId = 15 }, // Development
                           new { TagId = 15, TaskId = 15 }, // Sprint2
                           new { TagId = 14, TaskId = 16 }, // Development
                           new { TagId = 15, TaskId = 16 }, // Sprint2
                           new { TagId = 12, TaskId = 17 }, // Milestone
                           new { TagId = 15, TaskId = 17 }, // Sprint2
                           new { TagId = 16, TaskId = 18 }, // Testing
                           new { TagId = 15, TaskId = 18 }, // Sprint2
                           new { TagId = 16, TaskId = 19 }, // Testing
                           new { TagId = 15, TaskId = 19 }, // Sprint2
                           new { TagId = 16, TaskId = 20 }, // Testing
                           new { TagId = 15, TaskId = 20 }, // Sprint2
                           new { TagId = 16, TaskId = 21 }, // Testing
                           new { TagId = 15, TaskId = 21 }, // Sprint2
                           new { TagId = 12, TaskId = 22 }, // Milestone
                           new { TagId = 15, TaskId = 22 }, // Sprint2
                           new { TagId = 17, TaskId = 23 }, // Deployment
                           new { TagId = 15, TaskId = 23 }, // Sprint2
                           new { TagId = 18, TaskId = 24 }, // Documentation
                           new { TagId = 15, TaskId = 24 }, // Sprint2
                           new { TagId = 8, TaskId = 25 }, // Marketing
                           new { TagId = 15, TaskId = 25 }, // Sprint2
                           new { TagId = 17, TaskId = 26 }, // Deployment
                           new { TagId = 15, TaskId = 26 }, // Sprint2
                           new { TagId = 12, TaskId = 27 }, // Milestone
                           new { TagId = 15, TaskId = 27 } // Sprint2
                       );
                   });

        // Связь с шаблонами задач
        builder.HasMany(t => t.TaskTemplates)
               .WithMany(t => t.Tags)
               .UsingEntity<Dictionary<string, object>>(
                   "TaskTemplateTags",
                   j => j.HasOne<TaskTemplates>().WithMany().HasForeignKey("TaskTemplateId"),
                   j => j.HasOne<Tags>().WithMany().HasForeignKey("TagId"),
                   j =>
                   {
                       j.HasKey("TagId", "TaskTemplateId");
                       j.ToTable("TaskTemplateTags");
                       // Seed TaskTemplate-Tag relationships
                       j.HasData(
                           new { TagId = 10, TaskTemplateId = 1 }, // Analysis
                           new { TagId = 19, TaskTemplateId = 1 }, // Requirements
                           new { TagId = 13, TaskTemplateId = 2 }, // Design
                           new { TagId = 20, TaskTemplateId = 2 }, // UI/UX
                           new { TagId = 14, TaskTemplateId = 3 }, // Development
                           new { TagId = 21, TaskTemplateId = 3 }, // API
                           new { TagId = 16, TaskTemplateId = 4 }, // Testing
                           new { TagId = 22, TaskTemplateId = 4 }, // QA
                           new { TagId = 17, TaskTemplateId = 5 }, // Deployment
                           new { TagId = 23, TaskTemplateId = 5 } // Release
                       );
                   });

        builder.HasData(
            new Tags { Id = 1, Name = "CRM", Color = "#E5E7EB" }, // Light Gray 
            new Tags { Id = 2, Name = "WebApp", Color = "#A7F3D0" }, // Mint Green 
            new Tags { Id = 3, Name = "MobileApp", Color = "#BFDBFE" }, // Pale Blue 
            new Tags { Id = 4, Name = "Delivery", Color = "#FDE68A" }, // Soft Yellow 
            new Tags { Id = 5, Name = "Automation", Color = "#DDD6FE" }, // Light Lavender 
            new Tags { Id = 6, Name = "Warehouse", Color = "#D1D5DB" }, // Cool Gray 
            new Tags { Id = 7, Name = "Portal", Color = "#FBCFE8" }, // Baby Pink 
            new Tags { Id = 8, Name = "Marketing", Color = "#99F6E4" }, // Aqua 
            new Tags { Id = 9, Name = "Campaign", Color = "#F5D0FE" }, // Light Pink 
            new Tags { Id = 10, Name = "Analysis", Color = "#E2E8F0" }, // Slate Gray 
            new Tags { Id = 11, Name = "Sprint1", Color = "#BAE6FD" }, // Sky Blue 
            new Tags { Id = 12, Name = "Milestone", Color = "#F9A8D4" }, // Rose 
            new Tags { Id = 13, Name = "Design", Color = "#C7D2FE" }, // Soft Violet 
            new Tags { Id = 14, Name = "Development", Color = "#BBF7D0" }, // Light Green 
            new Tags { Id = 15, Name = "Sprint2", Color = "#A5F3FC" }, // Cyan 
            new Tags { Id = 16, Name = "Testing", Color = "#FED7AA" }, // Peach 
            new Tags { Id = 17, Name = "Deployment", Color = "#D4D4D8" }, // Zinc Gray 
            new Tags { Id = 18, Name = "Documentation", Color = "#CCFBF1" }, // Mint 
            new Tags { Id = 19, Name = "Requirements", Color = "#CFFAFE" }, // Light Cyan 
            new Tags { Id = 20, Name = "UI/UX", Color = "#FECACA" }, // Coral Pink 
            new Tags { Id = 21, Name = "API", Color = "#FEF3C7" }, // Cream Yellow 
            new Tags { Id = 22, Name = "QA", Color = "#EDE9FE" }, // Pale Purple 
            new Tags { Id = 23, Name = "Release", Color = "#FECDD3" }
        );
    }
}