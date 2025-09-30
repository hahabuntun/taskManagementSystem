using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Application.Interfaces;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.Entities.Notification;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Repositories;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services;

public class ProjectLinkService : IProjectLinkService
{
    private readonly IProjectLinkRepository _projectLinkRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public ProjectLinkService(
        IProjectLinkRepository projectLinkRepository,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _projectLinkRepository = projectLinkRepository ?? throw new ArgumentNullException(nameof(projectLinkRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
    }

    public async Task<int> AddLinkAsync(int projectId, string link, string? description, int creatorId)
    {
        if (projectId <= 0)
            throw new ArgumentException("ID проекта должен быть положительным.", nameof(projectId));
        if (string.IsNullOrWhiteSpace(link))
            throw new ArgumentException("Ссылка не может быть пустой.", nameof(link));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var projectLink = new ProjectLink
        {
            ProjectId = projectId,
            Link = link,
            Description = description,
            CreatedOn = DateTime.UtcNow
        };

        var linkId = await _projectLinkRepository.AddLinkAsync(projectLink);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Новая ссылка '{link}' добавлена к проекту.",
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
                Text = $"Новая ссылка '{link}' добавлена к проекту.",
                RelatedEntityName = "Ссылка проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return linkId;
    }

    public async Task<bool> UpdateLinkAsync(int linkId, string link, string? description, int creatorId)
    {
        if (linkId <= 0)
            throw new ArgumentException("ID ссылки должен быть положительным.", nameof(linkId));
        if (string.IsNullOrWhiteSpace(link))
            throw new ArgumentException("Ссылка не может быть пустой.", nameof(link));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var updated = await _projectLinkRepository.UpdateLinkAsync(linkId, link, description);
        if (!updated)
            return false;

        var projectLink = await _projectLinkRepository.GetLinkByIdAsync(linkId)
            ?? throw new KeyNotFoundException($"Ссылка с ID {linkId} не найдена");
        var projectId = projectLink.ProjectId;

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Ссылка '{link}' обновлена.",
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
                Text = $"Ссылка '{link}' обновлена.",
                RelatedEntityName = "Ссылка проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return true;
    }

    public async Task<bool> DeleteLinkAsync(int linkId, int creatorId)
    {
        if (linkId <= 0)
            throw new ArgumentException("ID ссылки должен быть положительным.", nameof(linkId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var projectLink = await _projectLinkRepository.GetLinkByIdAsync(linkId)
            ?? throw new KeyNotFoundException($"Ссылка с ID {linkId} не найдена");
        var projectId = projectLink.ProjectId;
        var linkUrl = projectLink.Link;

        var deleted = await _projectLinkRepository.DeleteLinkAsync(linkId);
        if (!deleted)
            return false;

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Ссылка '{linkUrl}' удалена из проекта.",
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
                Text = $"Ссылка '{linkUrl}' удалена из проекта.",
                RelatedEntityName = "Ссылка проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return true;
    }

    public async Task<IEnumerable<ProjectLink>> GetLinksByProjectIdAsync(int projectId)
    {
        if (projectId <= 0)
            throw new ArgumentException("ID проекта должен быть положительным.", nameof(projectId));

        return await _projectLinkRepository.GetLinksByProjectIdAsync(projectId);
    }
}