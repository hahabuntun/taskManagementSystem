using Microsoft.Extensions.DependencyInjection;
using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Services;
using Vkr.Application.Interfaces.Services.BoardServices;
using Vkr.Application.Interfaces.Services.ChecklistServices;
using Vkr.Application.Interfaces.Services.FilesService;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Application.Interfaces.Services.JournalServices;
using Vkr.Application.Interfaces.Services.OrganizationServices;
using Vkr.Application.Interfaces.Services.ProjectServices;
using Vkr.Application.Interfaces.Services.SprintServices;
using Vkr.Application.Interfaces.Services.TaskServices;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Application.Mappings;
using Vkr.Application.Services;
using Vkr.Application.Services.AnalyticsServices;
using Vkr.Application.Services.BoardServices;
using Vkr.Application.Services.CheckListServices;
using Vkr.Application.Services.FilesServices;
using Vkr.Application.Services.HistoryServices;
using Vkr.Application.Services.JournalServices;
using Vkr.Application.Services.OrganizationServices;
using Vkr.Application.Services.ProjectServices;
using Vkr.Application.Services.SprintServices;
using Vkr.Application.Services.TaskExecutorServices;
using Vkr.Application.Services.TaskObserverServices;
using Vkr.Application.Services.TaskServices;
using Vkr.Application.Services.WorkerServices;

namespace Vkr.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection service)
        {
            service.AddScoped<IWorkersService, WorkersService>();
            service.AddScoped<IWorkerPositionsService, WorkerPositionsService>();
            service.AddScoped<IFilesService, FilesService>();
            service.AddScoped<IWorkersManagementService, WorkersManagementService>();
            service.AddScoped<ITaskService, TaskService>();
            service.AddScoped<ITaskExecutorService, TaskExecutorService>();
            service.AddScoped<ITaskObserverService, TaskObserverService>();
            service.AddScoped<ITaskFilterService, TaskFilterService>();
            service.AddScoped<ITaskLinkService, TaskLinkService>();
            service.AddScoped<ITaskTemplateService, TaskTemplateService>();
            service.AddScoped<ITaskTemplateLinkService, TaskTemplateLinkService>();
            service.AddScoped<IProjectService, ProjectService>();
            service.AddScoped<IProjectMemberManagementService, ProjectMemberManagementService>();
            service.AddScoped<IProjectLinkService, ProjectLinkService>();
            service.AddScoped<IBoardService, BoardService>();
            service.AddScoped<ISprintService, SprintService>();
            service.AddScoped<IAuditService, AuditService>();
            service.AddScoped<ITaskMessageService, TaskMessageService>();
            service.AddScoped<IChecklistService, ChecklistService>();
            service.AddScoped<IOrganizationService, OrganizationService>();
            service.AddScoped<ITagService, TagService>();
            service.AddScoped<IAnalyticsService, AnalyticsService>();
            service.AddScoped<INotificationService, NotificationService>();
            service.AddScoped<IHistoryService, HistoryService>();
            // Маппинг
            service.AddAutoMapper(typeof(BoardMappingProfile).Assembly);
            service.AddAutoMapper(typeof(BoardMappingProfile).Assembly);
            service.AddAutoMapper(typeof(ChecklistMappingProfile).Assembly);
            
            return service;
        }
    }
}
