using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.WorkerRepositories;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Application.Interfaces.Services.HistoryServices; // Добавлено
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Worker;
using Vkr.Domain.Entities.Notification;
using Vkr.Domain.Entities.History; // Добавлено

namespace Vkr.Application.Services.WorkerServices;

public class WorkerPositionsService : IWorkerPositionsService
{
    private readonly IWorkerPositionsRepository _workerPositionsRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService; // Добавлено

    public WorkerPositionsService(
        IWorkerPositionsRepository workerPositionsRepository,
        INotificationService notificationService,
        IHistoryService historyService) // Добавлено
    {
        _workerPositionsRepository = workerPositionsRepository ?? throw new ArgumentNullException(nameof(workerPositionsRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService)); // Добавлено
    }

    public async Task<WorkerPositionDto> CreateWorkerPosition(
        WorkerPosition workerPosition, 
        int[]? taskGiverIds, 
        int[]? taskTakerIds, 
        int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        var createdPosition = await _workerPositionsRepository.CreateWorkerPosition(
            workerPosition, taskGiverIds, taskTakerIds);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Новая должность '{workerPosition.Title}' создана в организации.",
            RelatedEntityId = createdPosition.Id,
            RelatedEntityType = HistoryEntityType.Organization,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the organization
        var subscriptions = await _notificationService.GetEntitySubscriptions(1, EntityType.Organization);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Organization && s.EntityId == 1)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Новая должность '{workerPosition.Title}' создана в организации.",
                RelatedEntityName = workerPosition.Title ?? "Должность",
                RelatedEntityId = createdPosition.Id,
                RelatedEntityType = EntityType.Organization,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return createdPosition;
    }

    public async Task<bool> DeleteWorkerPositionAsync(int id, int creatorId)
    {
        if (id <= 0)
            throw new ArgumentException("Идентификатор должности должен быть положительным.", nameof(id));
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        // Get position name for notification
        var position = await _workerPositionsRepository.GetWorkerPositionById(id)
            ?? throw new KeyNotFoundException($"Должность с ID {id} не найдена");
        var positionName = position.Title;

        var deleted = await _workerPositionsRepository.DeleteWorkerPositionById(id);
        if (!deleted)
            return false;

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Должность '{positionName}' удалена из организации.",
            RelatedEntityId = id,
            RelatedEntityType = HistoryEntityType.Organization,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the organization
        var subscriptions = await _notificationService.GetEntitySubscriptions(1, EntityType.Organization);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Organization && s.EntityId == 1)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Должность '{positionName}' удалена из организации.",
                RelatedEntityName = positionName ?? "Должность",
                RelatedEntityId = id,
                RelatedEntityType = EntityType.Organization,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return true;
    }

    public async Task<List<WorkerPositionDto>> GetAllWorkerPositionsAsync()
    {
        return await _workerPositionsRepository.GetWorkerPositions();
    }

    public async Task<WorkerPositionDto?> GetWorkerPositionsByIdAsync(int positionId)
    {
        return await _workerPositionsRepository.GetWorkerPositionById(positionId);
    }

    public async Task<WorkerPositionDto?> UpdateWorkerPositionAsync(
        int id, 
        WorkerPosition workerPosition, 
        int[] taskGiverIds, 
        int[] taskTakerIds, 
        int creatorId)
    {
        if (id <= 0)
            throw new ArgumentException("Идентификатор должности должен быть положительным.", nameof(id));
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        var updatedPosition = await _workerPositionsRepository.UpdateWorkerPositions(
            id, workerPosition, taskGiverIds, taskTakerIds);

        if (updatedPosition != null)
        {
            // Add history record
            var historyDto = new CreateHistoryDTO
            {
                Text = $"Должность '{workerPosition.Title}' обновлена в организации.",
                RelatedEntityId = id,
                RelatedEntityType = HistoryEntityType.Organization,
                CreatorId = creatorId
            };
            await _historyService.AddHistoryItemAsync(historyDto);

            // Send notification to workers subscribed to the organization
            var subscriptions = await _notificationService.GetEntitySubscriptions(1, EntityType.Organization);
            var subscribedWorkerIds = subscriptions
                .Where(s => s.EntityType == EntityType.Organization && s.EntityId == 1)
                .Select(s => s.WorkerId)
                .ToList();

            if (subscribedWorkerIds.Any())
            {
                var notificationDto = new CreateNotificationDto
                {
                    Text = $"Должность '{workerPosition.Title}' обновлена в организации.",
                    RelatedEntityName = workerPosition.Title ?? "Должность",
                    RelatedEntityId = id,
                    RelatedEntityType = EntityType.Organization,
                    CreatorId = creatorId,
                    WorkerIds = subscribedWorkerIds
                };
                await _notificationService.CreateNotificationAsync(notificationDto);
            }
        }

        return updatedPosition;
    }

    public async Task<bool> IsWorkerPositionExists(int id)
    {
        return await _workerPositionsRepository.IsWorkerPositionExists(id);
    }
}