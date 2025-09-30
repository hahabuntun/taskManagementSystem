using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vkr.Application.Interfaces.Repositories;
using Vkr.Application.Interfaces.Repositories.AnalyticsRepositories;
using Vkr.Application.Interfaces.Repositories.BoardRepositories;
using Vkr.Application.Interfaces.Repositories.CheklistRepository;
using Vkr.Application.Interfaces.Repositories.FilesRepositories;
using Vkr.Application.Interfaces.Repositories.HistoryRepository;
using Vkr.Application.Interfaces.Repositories.JournalRepositories;
using Vkr.Application.Interfaces.Repositories.OrganizationRepositories;
using Vkr.Application.Interfaces.Repositories.ProjectRepositories;
using Vkr.Application.Interfaces.Repositories.SprintRepositories;
using Vkr.Application.Interfaces.Repositories.TagRepositories;
using Vkr.Application.Interfaces.Repositories.TaskExecutorRepositories;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Application.Interfaces.Repositories.TaskTemplateRepositories;
using Vkr.Application.Interfaces.Repositories.WorkerRepositories;
using Vkr.DataAccess.Journal;
using Vkr.DataAccess.Repositories;
using Vkr.DataAccess.Repositories.BoardRepositories;
using Vkr.DataAccess.Repositories.CheckListRepository;
using Vkr.DataAccess.Repositories.JournalRepositories;
using Vkr.DataAccess.Repositories.OrganizationRepositories;
using Vkr.DataAccess.Repositories.ProjectRepositories;
using Vkr.DataAccess.Repositories.SprintRepositories;
using Vkr.DataAccess.Repositories.TagRepositories;
using Vkr.DataAccess.Repositories.TaskRepositories;
using Vkr.DataAccess.Repositories.TaskTemplateRepositories;
using Vkr.DataAccess.Repositories.WorkersRepositories;
using Vkr.Domain.Repositories;
using Vkr.Infrastructure.Repositories;

namespace Vkr.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection service, string connectionString)
    {
        service.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        service.AddDbContext<AuditableDbContext, ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        service.AddScoped<IWorkersRepository, WorkersRepository>();
        service.AddScoped<IWorkerPositionsRepository, WorkerPositionsRepository>();
        service.AddScoped<IFilesRepository, FilesRepository>();
        service.AddScoped<IWorkersManagmentRepository, WorkersManagmentRepository>();
        service.AddScoped<ITaskRepository, TaskRepository>();
        service.AddScoped<ITaskExecutorRepository, TaskExecutorRepository>();
        service.AddScoped<ITaskObserverRepository, TaskObserverRepository>();
        service.AddScoped<ITaskFilterRepository, TaskFilterRepository>();
        service.AddScoped<IBoardRepository, BoardRepository>();
        service.AddScoped<ITaskMessageRepository, TaskMessageRepository>();
        service.AddScoped<ITaskLinkRepository, TaskLinkRepository>();
         service.AddScoped<ITaskTemplateRepository, TaskTemplateRepository>();
        service.AddScoped<ITaskTemplateLinkRepository, TaskTemplateLinkRepository>();
        service.AddScoped<ITaskPriorityRepository, TaskPriorityRepository>();
        service.AddScoped<IProjectRepository, ProjectRepository>();
        service.AddScoped<IProjectMemberManagementRepository, ProjectMemberManagementRepository>();
        service.AddScoped<IProjectChecklistRepository, ProjectChecklistRepository>();
        service.AddScoped<IProjectLinkRepository, ProjectLinkRepository>();
        service.AddScoped<IProjectChecklistCheckRepository, ProjectChecklistCheckRepository>();
        service.AddScoped<ISprintRepository, SprintRepository>();
        service.AddScoped<IAuditLogRepository, AuditLogRepository>();
        service.AddScoped<ITagRepository, TagRepository>();
        service.AddScoped<IOrganizationRepository, OrganizationRepository>();
        service.AddScoped<IChecklistRepository, ChecklistRepository>();
        service.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
        service.AddScoped<INotificationRepository, NotificationRepository>();
        service.AddScoped<IHistoryRepository, HistoryRepository>();
        return service;
    }
}