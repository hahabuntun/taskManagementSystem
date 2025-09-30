using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Application.Interfaces;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.Entities.Notification;
using Vkr.Domain.Entities.Worker;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;
using Vkr.Application.Interfaces.Repositories.ProjectRepositories;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Application.Interfaces.Services.ProjectServices;

namespace Vkr.Application.Services.ProjectServices;

public class ProjectMemberManagementService : IProjectMemberManagementService
{
    private readonly IProjectMemberManagementRepository _repository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;
    private readonly IWorkersService _workerService;
    private readonly IProjectService _projectService;

    public ProjectMemberManagementService(
        IProjectMemberManagementRepository repository,
        INotificationService notificationService,
        IProjectService projectService,
        IWorkersService workerService,
        IHistoryService historyService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
        _workerService = workerService ?? throw new ArgumentNullException(nameof(workerService));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
    }

    public async Task<IEnumerable<ProjectMemberDTO>> GetAllMembersAsync(int projectId)
    {
        return await _repository.GetAllMembersAsync(projectId);
    }

    public async Task<ProjectMemberDTO> GetMemberAsync(int projectId, int memberId)
    {
        var member = await _repository.GetMemberAsync(projectId, memberId)
            ?? throw new KeyNotFoundException($"Участник {memberId} не найден в проекте {projectId}");
        return member;
    }

    public async Task<IEnumerable<Workers>> GetMemberSubordinatesAsync(int projectId, int memberId)
    {
        return await _repository.GetMemberSubordinatesAsync(projectId, memberId);
    }

    public async Task<IEnumerable<Workers>> GetMemberDirectors(int projectId, int memberId)
    {
        return await _repository.GetMemberDirectors(projectId, memberId);
    }

    public async Task<bool> AddMemberAsync(int projectId, int workerId, int creatorId)
    {
        if (projectId <= 0)
            throw new ArgumentException("ID проекта должен быть положительным.", nameof(projectId));
        if (workerId <= 0)
            throw new ArgumentException("ID сотрудника должен быть положительным.", nameof(workerId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var added = await _repository.AddMember(projectId, workerId);
        if (!added)
            return false;
        var worker = await _workerService.GetWorkerByIdAsync(workerId);
        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"{worker.Email} добавлен в проект.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the project
        var subscriptions = await _notificationService.GetEntitySubscriptions(projectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == projectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"{worker.Email} добавлен в проект.",
                RelatedEntityName = "Участник проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return true;
    }

    public async Task<bool> RemoveMemberAsync(int projectId, int workerId, int creatorId)
    {
        if (projectId <= 0)
            throw new ArgumentException("ID проекта должен быть положительным.", nameof(projectId));
        if (workerId <= 0)
            throw new ArgumentException("ID сотрудника должен быть положительным.", nameof(workerId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var removed = await _repository.RemoveMember(projectId, workerId);
        if (!removed)
            return false;
        var worker = await _workerService.GetWorkerByIdAsync(workerId);
        var project = await _projectService.GetProjectByIdAsync(projectId);
        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"{worker.Email} удален из проекта.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the project
        var subscriptions = await _notificationService.GetEntitySubscriptions(projectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == projectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"{worker.Email} удален из проекта ${project.Name}.",
                RelatedEntityName = "Участник проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return true;
    }

    public async Task<bool> AddSubordinateToMember(int projectId, int memberId, int subordinateId, int creatorId)
    {
        if (projectId <= 0)
            throw new ArgumentException("ID проекта должен быть положительным.", nameof(projectId));
        if (memberId <= 0)
            throw new ArgumentException("ID участника должен быть положительным.", nameof(memberId));
        if (subordinateId <= 0)
            throw new ArgumentException("ID подчиненного должен быть положительным.", nameof(subordinateId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var added = await _repository.AddSubordinateToMember(projectId, memberId, subordinateId);
        if (!added)
            return false;
        var sub = await _workerService.GetWorkerByIdAsync(subordinateId);
        var memb = await _workerService.GetWorkerByIdAsync(memberId);
        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            
            Text = $"{sub.Email} назначен подчиненным для {memb.Email}.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the project
        var subscriptions = await _notificationService.GetEntitySubscriptions(projectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == projectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                
                Text = $"{sub.Email} назначен подчиненным для {memb.Email}.",
                RelatedEntityName = "Участник проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return true;
    }

    public async Task<bool> RemoveSubordinateFromMember(int projectId, int memberId, int subordinateId, int creatorId)
    {
        if (projectId <= 0)
            throw new ArgumentException("ID проекта должен быть положительным.", nameof(projectId));
        if (memberId <= 0)
            throw new ArgumentException("ID участника должен быть положительным.", nameof(memberId));
        if (subordinateId <= 0)
            throw new ArgumentException("ID подчиненного должен быть положительным.", nameof(subordinateId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var removed = await _repository.RemoveSubordinateFromMember(projectId, memberId, subordinateId);
        if (!removed)
            return false;
        var sub = await _workerService.GetWorkerByIdAsync(subordinateId);
        var memb = await _workerService.GetWorkerByIdAsync(memberId);
        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"{sub.Email} удален из подчиненных для {memb.Email}.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the project
        var subscriptions = await _notificationService.GetEntitySubscriptions(projectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == projectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Сотрудник с ID {subordinateId} удален из подчиненных для {memberId} в проекте.",
                RelatedEntityName = "Участник проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return true;
    }
}