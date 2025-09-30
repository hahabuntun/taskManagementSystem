using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Notification;
using Vkr.Domain.Repositories;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services;

public class TaskLinkService : ITaskLinkService
{
    private readonly ITaskLinkRepository _taskLinkRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public TaskLinkService(
        ITaskLinkRepository taskLinkRepository,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _taskLinkRepository = taskLinkRepository ?? throw new ArgumentNullException(nameof(taskLinkRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
    }

    public async Task<int> AddLinkAsync(int taskId, string link, string? description, int creatorId)
    {
        if (taskId <= 0)
            throw new ArgumentException("ID задачи должен быть положительным.", nameof(taskId));
        if (string.IsNullOrWhiteSpace(link))
            throw new ArgumentException("Ссылка не может быть пустой.", nameof(link));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var taskLink = new TaskLink
        {
            TaskId = taskId,
            Link = link,
            Description = description,
            CreatedOn = DateTime.UtcNow
        };

        var linkId = await _taskLinkRepository.AddLinkAsync(taskLink);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Ссылка '{link}' добавлена к задаче {taskId}.",
            RelatedEntityId = taskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Ссылка '{link}' добавлена к задаче {taskId}.",
                RelatedEntityName = "Ссылка задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
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
            throw new ArgumentException("Creator ID must be positive.", nameof(creatorId));

        var updated = await _taskLinkRepository.UpdateLinkAsync(linkId, link, description);

        if (updated)
        {
            var taskLink = await _taskLinkRepository.GetLinkByIdAsync(linkId)
                ?? throw new KeyNotFoundException($"Ссылка с ID {linkId} не найдена");
            var taskId = taskLink.TaskId;

            // Add history record
            var historyDto = new CreateHistoryDTO
            {
                Text = $"Ссылка '{link}' обновлена для задачи {taskId}.",
                RelatedEntityId = taskId,
                RelatedEntityType = HistoryEntityType.Task,
                CreatorId = creatorId
            };
            await _historyService.AddHistoryItemAsync(historyDto);

            // Send notification
            var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
            var subscribedWorkerIds = subscriptions
                .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
                .Select(s => s.WorkerId)
                .ToList();

            if (subscribedWorkerIds.Any())
            {
                var notificationDto = new CreateNotificationDto
                {
                    Text = $"Ссылка '{link}' обновлена для задачи {taskId}.",
                    RelatedEntityName = "Ссылка задачи",
                    RelatedEntityId = taskId,
                    RelatedEntityType = EntityType.Task,
                    CreatorId = creatorId,
                    WorkerIds = subscribedWorkerIds
                };
                await _notificationService.CreateNotificationAsync(notificationDto);
            }
        }

        return updated;
    }

    public async Task<bool> DeleteLinkAsync(int linkId, int creatorId)
    {
        if (linkId <= 0)
            throw new ArgumentException("ID ссылки должен быть положительным.", nameof(linkId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var taskLink = await _taskLinkRepository.GetLinkByIdAsync(linkId)
            ?? throw new KeyNotFoundException($"Ссылка с ID {linkId} не найдена");
        var taskId = taskLink.TaskId;
        var linkUrl = taskLink.Link;

        var deleted = await _taskLinkRepository.DeleteLinkAsync(linkId);

        if (deleted)
        {
            // Add history record
            var historyDto = new CreateHistoryDTO
            {
                Text = $"Ссылка '{linkUrl}' удалена из задачи {taskId}.",
                RelatedEntityId = taskId,
                RelatedEntityType = HistoryEntityType.Task,
                CreatorId = creatorId
            };
            await _historyService.AddHistoryItemAsync(historyDto);

            // Send notification
            var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
            var subscribedWorkerIds = subscriptions
                .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
                .Select(s => s.WorkerId)
                .ToList();

            if (subscribedWorkerIds.Any())
            {
                var notificationDto = new CreateNotificationDto
                {
                    Text = $"Ссылка '{linkUrl}' удалена из задачи {taskId}.",
                    RelatedEntityName = "Ссылка задачи",
                    RelatedEntityId = taskId,
                    RelatedEntityType = EntityType.Task,
                    CreatorId = creatorId,
                    WorkerIds = subscribedWorkerIds
                };
                await _notificationService.CreateNotificationAsync(notificationDto);
            }
        }

        return deleted;
    }

    public async Task<IEnumerable<TaskLink>> GetLinksByTaskIdAsync(int taskId)
    {
        if (taskId <= 0)
            throw new ArgumentException("ID задачи должен быть положительным.", nameof(taskId));

        return await _taskLinkRepository.GetLinksByTaskIdAsync(taskId);
    }
}