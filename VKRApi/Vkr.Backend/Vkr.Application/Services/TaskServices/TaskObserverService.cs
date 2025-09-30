using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Application.Interfaces.Repositories.WorkerRepositories;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Notification;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services.TaskObserverServices;

public class TaskObserverService : ITaskObserverService
{
    private readonly ITaskObserverRepository _observerRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IWorkersRepository _workersRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public TaskObserverService(
        ITaskObserverRepository observerRepository,
        ITaskRepository taskRepository,
        IWorkersRepository workersRepository,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _observerRepository = observerRepository ?? throw new ArgumentNullException(nameof(observerRepository));
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        _workersRepository = workersRepository ?? throw new ArgumentNullException(nameof(workersRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
    }

    public async Task<IEnumerable<WorkerDTO>> GetObserversAsync(int taskId)
    {
        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new ArgumentException($"Задача с ID {taskId} не существует.");
        return await _observerRepository.GetObserversAsync(taskId);
    }

    public async Task AddObserverAsync(int taskId, int workerId, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));
        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new ArgumentException($"Задача с ID {taskId} не существует.");
        var worker = await _workersRepository.GetByIdAsync(workerId)
            ?? throw new ArgumentException($"Сотрудник с ID {workerId} не существует.");

        var existingObservers = await _observerRepository.GetObserverEntitiesAsync(taskId);
        if (existingObservers.Any(o => o.WorkerId == workerId))
            throw new ArgumentException($"Сотрудник с ID {workerId} уже является наблюдателем");

        var observer = new TaskObserver
        {
            TaskId = taskId,
            WorkerId = workerId,
            AssignedOn = DateTime.UtcNow
        };

        await _observerRepository.AddObserverAsync(observer);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Сотрудник {workerId} добавлен как наблюдатель задачи {taskId}.",
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
                Text = $"Сотрудник {workerId} добавлен как наблюдатель задачи {taskId}.",
                RelatedEntityName = "Наблюдатель задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task AddObserversAsync(int taskId, int[] workerIds, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new ArgumentException($"Задача с ID {taskId} не существует");
        var validWorkerIds = await _workersRepository.ValidateWorkerIdsAsync(workerIds);
        if (validWorkerIds.Length != workerIds.Length)
            throw new ArgumentException("Один или несколько ID сотрудников недействительны");

        var existingObservers = await _observerRepository.GetObserverEntitiesAsync(taskId);
        var duplicateIds = validWorkerIds.Intersect(existingObservers.Select(o => o.WorkerId)).ToArray();
        if (duplicateIds.Any())
            throw new ArgumentException($"Сотрудники с ID {string.Join(", ", duplicateIds)} уже являются наблюдателями");

        var observers = validWorkerIds.Select(workerId => new TaskObserver
        {
            TaskId = taskId,
            WorkerId = workerId,
            AssignedOn = DateTime.UtcNow
        }).ToList();

        await _observerRepository.AddObserversAsync(observers);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Наблюдатели {string.Join(", ", workerIds)} добавлены к задаче {taskId}.",
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
                Text = $"Наблюдатели {string.Join(", ", workerIds)} добавлены к задаче {taskId}.",
                RelatedEntityName = "Наблюдатели задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task RemoveObserverAsync(int taskId, int workerId, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new ArgumentException($"Задача с ID {taskId} не существует");
        var observer = (await _observerRepository.GetObserverEntitiesAsync(taskId))
            .FirstOrDefault(o => o.WorkerId == workerId)
            ?? throw new ArgumentException($"Сотрудник с ID {workerId} не является наблюдателем");

        await _observerRepository.RemoveObserverAsync(observer);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Сотрудник {workerId} удален как наблюдатель из задачи {taskId}.",
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
                Text = $"Сотрудник {workerId} удален как наблюдатель из задачи {taskId}.",
                RelatedEntityName = "Наблюдатель задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }
}