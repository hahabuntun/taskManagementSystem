using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Vkr.DataAccess.Configurations;
using Vkr.DataAccess.Configurations.BoardConfigurations;
using Vkr.DataAccess.Configurations.HistoryConfigurations;
using Vkr.DataAccess.Configurations.NotificationConfigurations;
using Vkr.DataAccess.Configurations.OrganizationConfigurations;
using Vkr.DataAccess.Configurations.ProjectConfigurations;
using Vkr.DataAccess.Configurations.SprintConfigurations;
using Vkr.DataAccess.Configurations.TaskConfigurations;
using Vkr.DataAccess.Configurations.WorkerConfigurations;
using Vkr.DataAccess.Journal;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Board;
using Vkr.Domain.Entities.CheckLists;
using Vkr.Domain.Entities.History;
using Vkr.Domain.Entities.Notification;
using Vkr.Domain.Entities.Organization;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Sprint;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Worker;
using TaskStatus = Vkr.Domain.Entities.Task.TaskStatus;

namespace Vkr.DataAccess
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor) : AuditableDbContext(options)
    {
        public DbSet<Workers> Workers { get; set; }
        public DbSet<WorkerPosition> WorkerPositions { get; set; }
        public DbSet<WorkerPositionRelation> WorkerPositionRelations { get; set; }
        public DbSet<WorkersManagement> WorkersManagements { get; set; }
        public DbSet<Organizations> Organizations { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectStatus> ProjectStatus { get; set; }
        public DbSet<ProjectChecklist> ProjectChecklists { get; set; }
        public DbSet<ProjectChecklistCheck> ProjectChecklistChecks { get; set; }
        public DbSet<ProjectLink> ProjectLinks { get; set; }
        public DbSet<ProjectMemberManagement> ProjectMemberManagements { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskLink> TaskLinks { get; set; }
        public DbSet<TaskMessage> TaskMessage { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }
        public DbSet<TaskPriority> TaskPriorities { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<TaskFilter> TaskFilters { get; set; }
        public DbSet<TaskRelationship> TaskRelationships { get; set; }
        public DbSet<TaskRelationshipType> TaskRelationshipTypes { get; set; }
        public DbSet<TaskExecutor> TaskExecutors { get; set; }
        public DbSet<TaskObserver> TaskObservers { get; set; }
        public DbSet<TaskTemplates> TaskTemplates { get; set; }
        public DbSet<TaskTemplateLink> TaskTemplateLinks { get; set; }
        public DbSet<Sprints> Sprints { get; set; }
        public DbSet<SprintStatus> SprintStatus { get; set; }
        public DbSet<Boards> Boards { get; set; }
        public DbSet<BoardColumns> BoardColumns { get; set; } // Added
        public DbSet<BoardTask> BoardTasks { get; set; }
        public DbSet<ColorInfo> Colors { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<Vkr.Domain.Entities.Files.File> Files { get; set; }
        
        public DbSet<Checklist> Checklists { get; set; }
        
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<WorkerNotification> WorkerNotifications { get; set; }

        public DbSet<History> History { get; set; }

        /// <summary>
        /// Откуда взять текущего пользователя для аудита
        /// </summary>
        protected override string? GetCurrentUserId()
            => httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BoardConfiguration());
            modelBuilder.ApplyConfiguration(new BoardColumnConfiguration()); // Added
            modelBuilder.ApplyConfiguration(new BoardTaskConfiguration()); // Added
            modelBuilder.ApplyConfiguration(new WorkerConfiguration());
            modelBuilder.ApplyConfiguration(new WorkerPositionConfiguration());
            modelBuilder.ApplyConfiguration(new WorkerPositionRelationConfiguration());
            modelBuilder.ApplyConfiguration(new WorkersManagmentConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectStatusConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectChecklistConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectChecklistCheckConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectLinkConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectMemberManagementConfiguration());
            modelBuilder.ApplyConfiguration(new SprintConfiguration());
            modelBuilder.ApplyConfiguration(new SprintStatusConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new TaskStatusConfiguration());
            modelBuilder.ApplyConfiguration(new TaskPriorityConfiguration());
            modelBuilder.ApplyConfiguration(new TaskLinkConfiguration());
            modelBuilder.ApplyConfiguration(new TaskFilterConfiguration());
            modelBuilder.ApplyConfiguration(new TaskMessageConfiguration());
            modelBuilder.ApplyConfiguration(new TaskTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TaskRelationshipConfiguration());
            modelBuilder.ApplyConfiguration(new TaskRelationshipTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TaskExecutorConfiguration());
            modelBuilder.ApplyConfiguration(new TaskObserverConfiguration());
            modelBuilder.ApplyConfiguration(new TaskTemplateConfiguration());
            modelBuilder.ApplyConfiguration(new TaskTemplateLinkConfiguration());
            modelBuilder.ApplyConfiguration(new ColorInfoConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
            modelBuilder.ApplyConfiguration(new WorkerNotificationConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryConfiguration());
        }
    }
}