using System.ComponentModel.DataAnnotations;
using Vkr.Application.Filters;
using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.ProjectRepositories;
using Vkr.Application.Interfaces.Repositories.TagRepositories;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Notification;
using Vkr.Domain.DTO.Project;
using Vkr.Application.Interfaces.Services.ProjectServices;
using Vkr.Domain.DTO;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services.ProjectServices;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITagRepository _tagRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public ProjectService(
        IProjectRepository projectRepository,
        ITagRepository tagRepository,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _projectRepository = projectRepository;
        _tagRepository = tagRepository;
        _notificationService = notificationService;
        _historyService = historyService;
    }

    public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync()
    {
        return await _projectRepository.GetAllAsync();
    }

    public async Task<ProjectDTO> GetProjectByIdAsync(int id)
    {
        return await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Проект с ID {id} не найден");
    }

    public async Task<IEnumerable<ProjectDTO>> GetProjectsByOrganizationAsync(int organizationId)
    {
        return await _projectRepository.GetByOrganizationAsync(organizationId);
    }

    public async Task<IEnumerable<ProjectDTO>> GetProjectsByStatusAsync(int statusId)
    {
        return await _projectRepository.GetByStatusAsync(statusId);
    }

    public async Task<IEnumerable<ProjectDTO>> GetProjectsByManagerAsync(int managerId)
    {
        return await _projectRepository.GetByManagerAsync(managerId);
    }

    public async Task<int> CreateProjectAsync(ProjectCreateDTO projectDto, int creatorId)
    {
        if (string.IsNullOrWhiteSpace(projectDto.Name))
            throw new ValidationException("Название проекта обязательно");

        if (await _projectRepository.ProjectNameExistsAsync(projectDto.Name, projectDto.OrganizationId))
            throw new ValidationException($"Проект с названием '{projectDto.Name}' уже существует в этой организации");

        var projectId = await _projectRepository.AddAsync(projectDto);
        await _tagRepository.AddTagsToProjectAsync(
            projectId,
            projectDto.ExistingTagIds,
            projectDto.NewTags.Select(t => (t.Name, t.Color)).ToList()
        );

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Новый проект '{projectDto.Name}' создан в организации.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the organization
        var subscriptions = await _notificationService.GetEntitySubscriptions(projectDto.OrganizationId, EntityType.Organization);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Organization && s.EntityId == projectDto.OrganizationId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Новый проект '{projectDto.Name}' создан в организации.",
                RelatedEntityName = projectDto.Name,
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return projectId;
    }

    public async Task UpdateProjectAsync(int projectId, ProjectUpdateDto projectDto, int creatorId)
    {
        if (string.IsNullOrWhiteSpace(projectDto.Name))
            throw new ValidationException("Название проекта обязательно");

        var project = await _projectRepository.GetByIdAsync(projectId)
            ?? throw new KeyNotFoundException($"Проект с ID {projectId} не найден");

        await _projectRepository.UpdateAsync(projectId, projectDto);
        await _tagRepository.UpdateTagsForProjectAsync(
            projectId,
            projectDto.ExistingTagIds,
            projectDto.NewTags.Select(t => (t.Name, t.Color)).ToList()
        );

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Проект '{projectDto.Name}' обновлен.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the project
        var subscriptions = await _notificationService.GetEntitySubscriptions(project.Id, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == projectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Проект '{projectDto.Name}' обновлен.",
                RelatedEntityName = projectDto.Name,
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task DeleteProjectAsync(int id, int creatorId)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Проект с ID {id} не найден");

        var organizationId = 1; 
        var projectName = project.Name;

        await _projectRepository.DeleteAsync(id);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Проект '{projectName}' удален из организации.",
            RelatedEntityId = id,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the organization
        var subscriptions = await _notificationService.GetEntitySubscriptions(1, EntityType.Organization);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Organization && s.EntityId == organizationId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Проект '{projectName}' удален из организации.",
                RelatedEntityName = projectName,
                RelatedEntityId = id,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task<IEnumerable<ProjectDTO>> GetProjectsByFilterAsync(ProjectsFilter filter)
    {
        return await _projectRepository.GetByFilterAsync(filter);
    }

    public async Task<List<Tags>> GetAvailableTags(int projectId)
    {
        return await _tagRepository.GetAvailableTagsForProjectAsync(projectId);
    }

    public async Task<List<Tags>> GetAllTags()
    {
        return await _tagRepository.GetAllTagsForProjectsAsync();
    }
}