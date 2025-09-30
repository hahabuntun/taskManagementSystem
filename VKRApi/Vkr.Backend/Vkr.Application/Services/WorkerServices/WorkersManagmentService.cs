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

public class WorkersManagementService : IWorkersManagementService
{
    private readonly IWorkersManagmentRepository _repository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService; // Добавлено
    private readonly IWorkersRepository _workersRepository; // Добавлено для получения данных сотрудников

    public WorkersManagementService(
        IWorkersManagmentRepository repository,
        INotificationService notificationService,
        IHistoryService historyService, // Добавлено
        IWorkersRepository workersRepository) // Добавлено
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
        _workersRepository = workersRepository ?? throw new ArgumentNullException(nameof(workersRepository));
    }

    public async Task<bool> SetConnection(WorkersManagmentDTO request, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));
        if (request.ManagerId <= 0)
            throw new ArgumentException("Идентификатор руководителя должен быть положительным.", nameof(request.ManagerId));
        if (request.SubordinateId <= 0)
            throw new ArgumentException("Идентификатор подчиненного должен быть положительным.", nameof(request.SubordinateId));

        // Получение имен сотрудников для сообщений
        var manager = await _workersRepository.GetByIdAsync(request.ManagerId)
            ?? throw new KeyNotFoundException($"Руководитель с ID {request.ManagerId} не найден");
        var subordinate = await _workersRepository.GetByIdAsync(request.SubordinateId)
            ?? throw new KeyNotFoundException($"Подчиненный с ID {request.SubordinateId} не найден");

        var result = await _repository.SetConnection(request);

        if (result)
        {
            // Add history record
            var historyDto = new CreateHistoryDTO
            {
                Text = $"Сотрудник '{subordinate.Email}' назначен подчиненным руководителю '{manager.Email}' в организации.",
                RelatedEntityId = request.SubordinateId,
                RelatedEntityType = HistoryEntityType.Worker,
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
                    Text = $"Сотрудник '{subordinate.Email}' назначен подчиненным руководителю '{manager.Email}' в организации.",
                    RelatedEntityName = subordinate.Email ?? "Сотрудник",
                    RelatedEntityId = request.SubordinateId,
                    RelatedEntityType = EntityType.Organization,
                    CreatorId = creatorId,
                    WorkerIds = subscribedWorkerIds
                };
                await _notificationService.CreateNotificationAsync(notificationDto);
            }
        }

        return result;
    }

    public async Task<bool> DeleteConnection(int managerId, int subordinateId, int creatorId)
    {
        if (managerId <= 0)
            throw new ArgumentException("Идентификатор руководителя должен быть положительным.", nameof(managerId));
        if (subordinateId <= 0)
            throw new ArgumentException("Идентификатор подчиненного должен быть положительным.", nameof(subordinateId));
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        // Получение имен сотрудников для сообщений
        var manager = await _workersRepository.GetByIdAsync(managerId)
            ?? throw new KeyNotFoundException($"Руководитель с ID {managerId} не найден");
        var subordinate = await _workersRepository.GetByIdAsync(subordinateId)
            ?? throw new KeyNotFoundException($"Подчиненный с ID {subordinateId} не найден");

        var result = await _repository.DeleteConnection(managerId, subordinateId);

        if (result)
        {
            // Add history record
            var historyDto = new CreateHistoryDTO
            {
                Text = $"Сотрудник '{subordinate.Email}' удален из подчинения руководителя '{manager.Email}' в организации.",
                RelatedEntityId = subordinateId,
                RelatedEntityType = HistoryEntityType.Worker,
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
                    Text = $"Сотрудник '{subordinate.Email}' удален из подчинения руководителя '{manager.Email}' в организации.",
                    RelatedEntityName = subordinate.Email ?? "Сотрудник",
                    RelatedEntityId = subordinateId,
                    RelatedEntityType = EntityType.Organization,
                    CreatorId = creatorId,
                    WorkerIds = subscribedWorkerIds
                };
                await _notificationService.CreateNotificationAsync(notificationDto);
            }
        }

        return result;
    }

    public async Task<int> UpdateConnection(WorkersManagmentDTO request, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));
        if (request.ManagerId <= 0)
            throw new ArgumentException("Идентификатор руководителя должен быть положительным.", nameof(request.ManagerId));
        if (request.SubordinateId <= 0)
            throw new ArgumentException("Идентификатор подчиненного должен быть положительным.", nameof(request.SubordinateId));

        // Получение имен сотрудников для сообщений
        var manager = await _workersRepository.GetByIdAsync(request.ManagerId)
            ?? throw new KeyNotFoundException($"Руководитель с ID {request.ManagerId} не найден");
        var subordinate = await _workersRepository.GetByIdAsync(request.SubordinateId)
            ?? throw new KeyNotFoundException($"Подчиненный с ID {request.SubordinateId} не найден");

        var result = await _repository.UpdateConnection(request);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Связь руководитель-подчиненный обновлена для сотрудника '{subordinate.Email}' и руководителя '{manager.Email}' в организации.",
            RelatedEntityId = request.SubordinateId,
            RelatedEntityType = HistoryEntityType.Worker,
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
                Text = $"Связь руководитель-подчиненный обновлена для сотрудника '{subordinate.Email}' и руководителя '{manager.Email}' в организации.",
                RelatedEntityName = subordinate.Email ?? "Сотрудник",
                RelatedEntityId = request.SubordinateId,
                RelatedEntityType = EntityType.Organization,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return result;
    }

    public async Task<List<Workers>> GetSubordinates(int managerId)
    {
        return await _repository.GetSubordinates(managerId);
    }

    public async Task<List<Workers>> GetManagers(int subordinateId)
    {
        return await _repository.GetManagers(subordinateId);
    }
}