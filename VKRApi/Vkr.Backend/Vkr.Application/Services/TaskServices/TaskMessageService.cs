using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Application.Interfaces.Services.TaskServices;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Notification;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services.TaskServices;

public class TaskMessageService : ITaskMessageService
{
    private readonly ITaskMessageRepository _taskMessageRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public TaskMessageService(
        ITaskMessageRepository taskMessageRepository,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _taskMessageRepository = taskMessageRepository ?? throw new ArgumentNullException(nameof(taskMessageRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
    }

    public async Task<IEnumerable<TaskMessageDTO>> GetMessagesByTaskAsync(int taskId)
    {
        var messages = await _taskMessageRepository.GetByTaskIdAsync(taskId);
        return messages.Select(MapToDTO);
    }

    public async Task<int> GetMessageCountByTaskAsync(int taskId)
    {
        return await _taskMessageRepository.GetMessageCountForTaskAsync(taskId);
    }

    public async Task<TaskMessageDTO> CreateMessageAsync(TaskMessageCreateDTO dto)
    {
        if (dto.SenderId <= 0)
            throw new ArgumentException("ID отправителя должен быть положительным.", nameof(dto.SenderId));
        if (dto.RelatedTaskId <= 0)
            throw new ArgumentException("ID задачи должен быть положительным.", nameof(dto.RelatedTaskId));

        var messageId = await _taskMessageRepository.CreateAsync(dto);

        var messageDto = new TaskMessageDTO
        {
            Id = messageId,
            MessageText = dto.MessageText,
            CreatedOn = DateTime.UtcNow,
            SenderId = dto.SenderId,
            RelatedTaskId = dto.RelatedTaskId
        };

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Новое сообщение добавлено в задачу {dto.RelatedTaskId}.",
            RelatedEntityId = dto.RelatedTaskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = dto.SenderId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(dto.RelatedTaskId, EntityType.Task);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Task && s.EntityId == dto.RelatedTaskId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Новое сообщение добавлено в задачу {dto.RelatedTaskId}.",
                RelatedEntityName = "Сообщение задачи",
                RelatedEntityId = dto.RelatedTaskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = dto.SenderId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return messageDto;
    }

    public async Task UpdateMessageAsync(int messageId, string newText, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));
        if (messageId <= 0)
            throw new ArgumentException("ID сообщения должен быть положительным.", nameof(messageId));

        var message = await _taskMessageRepository.GetByIdAsync(messageId)
            ?? throw new KeyNotFoundException($"Сообщение с ID {messageId} не найдено");
        var taskId = message.RelatedTaskId;

        await _taskMessageRepository.UpdateMessageTextAsync(messageId, newText);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Сообщение {messageId} обновлено в задаче {taskId}.",
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
                Text = $"Сообщение {messageId} обновлено в задаче {taskId}.",
                RelatedEntityName = "Сообщение задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task DeleteMessageAsync(int messageId, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));
        if (messageId <= 0)
            throw new ArgumentException("ID сообщения должен быть положительным.", nameof(messageId));

        var message = await _taskMessageRepository.GetByIdAsync(messageId)
            ?? throw new KeyNotFoundException($"Сообщение с ID {messageId} не найдено");
        var taskId = message.RelatedTaskId;

        await _taskMessageRepository.DeleteAsync(messageId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Сообщение {messageId} удалено из задачи {taskId}.",
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
                Text = $"Сообщение {messageId} удалено из задачи {taskId}.",
                RelatedEntityName = "Сообщение задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    private TaskMessageDTO MapToDTO(TaskMessage message)
    {
        return new TaskMessageDTO
        {
            Id = message.Id,
            MessageText = message.MessageText,
            CreatedOn = message.CreatedOn,
            SenderId = message.SenderId,
            RelatedTaskId = message.RelatedTaskId,
            SenderEmail = message.Sender?.Email,
            SenderName = message.Sender?.Name,
            SenderSecondName = message.Sender?.SecondName,
            SenderThirdName = message.Sender?.ThirdName
        };
    }
}