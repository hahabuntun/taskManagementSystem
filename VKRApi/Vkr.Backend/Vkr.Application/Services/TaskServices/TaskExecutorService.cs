using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.TaskExecutorRepositories;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Application.Interfaces.Repositories.WorkerRepositories;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Notification;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services.TaskExecutorServices;

public class TaskExecutorService : ITaskExecutorService
{
    private readonly ITaskExecutorRepository _executorRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IWorkersRepository _workersRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public TaskExecutorService(
        ITaskExecutorRepository executorRepository,
        ITaskRepository taskRepository,
        IWorkersRepository workersRepository,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _executorRepository = executorRepository ?? throw new ArgumentNullException(nameof(executorRepository));
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        _workersRepository = workersRepository ?? throw new ArgumentNullException(nameof(workersRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
    }

    public async Task<IEnumerable<WorkerDTO>> GetExecutorsAsync(int taskId)
    {
        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new ArgumentException($"Задача с ID {taskId} не существует");
        return await _executorRepository.GetExecutorsAsync(taskId);
    }

    public async Task AddExecutorAsync(int taskId, int workerId, bool isResponsible, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new ArgumentException($"Задача с ID {taskId} не существует");
        var worker = await _workersRepository.GetByIdAsync(workerId)
            ?? throw new ArgumentException($"Сотрудник с ID {workerId} не существует");

        var existingExecutors = await _executorRepository.GetExecutorEntitiesAsync(taskId);
        if (existingExecutors.Any(e => e.WorkerId == workerId))
            throw new ArgumentException($"Сотрудник с ID {workerId} уже является исполнителем");

        if (isResponsible)
        {
            foreach (var exec in existingExecutors.Where(e => e.IsResponsible))
            {
                exec.IsResponsible = false;
                await _executorRepository.UpdateExecutorAsync(exec);
            }
        }

        var executor = new TaskExecutor
        {
            TaskId = taskId,
            WorkerId = workerId,
            IsResponsible = isResponsible
        };

        await _executorRepository.AddExecutorAsync(executor);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Сотрудник {workerId} добавлен как {(isResponsible ? "ответственный " : "")}исполнитель задачи {taskId}.",
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
                Text = $"Сотрудник {workerId} добавлен как {(isResponsible ? "ответственный " : "")}исполнитель задачи {taskId}.",
                RelatedEntityName = "Исполнитель задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task AddExecutorsAsync(int taskId, int[] workerIds, int? responsibleWorkerId, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new ArgumentException($"Задача с ID {taskId} не существует");
        var validWorkerIds = await _workersRepository.ValidateWorkerIdsAsync(workerIds);
        if (validWorkerIds.Length != workerIds.Length)
            throw new ArgumentException("Один или несколько ID сотрудников недействительны");

        var existingExecutors = await _executorRepository.GetExecutorEntitiesAsync(taskId);
        var duplicateIds = validWorkerIds.Intersect(existingExecutors.Select(e => e.WorkerId)).ToArray();
        if (duplicateIds.Any())
            throw new ArgumentException($"Сотрудники с ID {string.Join(", ", duplicateIds)} уже являются исполнителями");

        if (responsibleWorkerId.HasValue)
        {
            if (!validWorkerIds.Contains(responsibleWorkerId.Value))
                throw new ArgumentException($"Ответственный сотрудник с ID {responsibleWorkerId.Value} отсутствует в списке сотрудников");
            foreach (var executor in existingExecutors.Where(e => e.IsResponsible))
            {
                executor.IsResponsible = false;
                await _executorRepository.UpdateExecutorAsync(executor);
            }
        }

        var executors = validWorkerIds.Select(workerId => new TaskExecutor
        {
            TaskId = taskId,
            WorkerId = workerId,
            IsResponsible = responsibleWorkerId.HasValue && workerId == responsibleWorkerId.Value
        }).ToList();

        await _executorRepository.AddExecutorsAsync(executors);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Исполнители {string.Join(", ", workerIds)} добавлены к задаче {taskId}{(responsibleWorkerId.HasValue ? $" с ответственным {responsibleWorkerId.Value}" : "")}.",
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
                Text = $"Исполнители {string.Join(", ", workerIds)} добавлены к задаче {taskId}{(responsibleWorkerId.HasValue ? $" с ответственным {responsibleWorkerId.Value}" : "")}.",
                RelatedEntityName = "Исполнители задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task RemoveExecutorAsync(int taskId, int workerId, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new ArgumentException($"Задача с ID {taskId} не существует");
        var executors = await _executorRepository.GetExecutorEntitiesAsync(taskId);
        var executor = executors.FirstOrDefault(e => e.WorkerId == workerId)
            ?? throw new ArgumentException($"Сотрудник с ID {workerId} не является исполнителем");

        var wasResponsible = executor.IsResponsible;

        if (wasResponsible)
        {
            foreach (var exec in executors.Where(e => e.IsResponsible))
            {
                exec.IsResponsible = false;
                await _executorRepository.UpdateExecutorAsync(exec);
            }
        }

        await _executorRepository.RemoveExecutorAsync(executor);

        var responsibleCount = await _executorRepository.CountResponsibleExecutorsAsync(taskId);
        if (responsibleCount > 0)
        {
            foreach (var exec in executors.Where(e => e.IsResponsible))
            {
                exec.IsResponsible = false;
                await _executorRepository.UpdateExecutorAsync(exec);
            }
        }

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Сотрудник {workerId} удален как {(wasResponsible ? "ответственный " : "")}исполнитель из задачи {taskId}.",
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
                Text = $"Сотрудник {workerId} удален как {(wasResponsible ? "ответственный " : "")}исполнитель из задачи {taskId}.",
                RelatedEntityName = "Исполнитель задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task UpdateResponsibleExecutorAsync(int taskId, int? workerId, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new ArgumentException($"Задача с ID {taskId} не существует");

        var executors = await _executorRepository.GetExecutorEntitiesAsync(taskId);

        foreach (var executor in executors.Where(e => e.IsResponsible))
        {
            executor.IsResponsible = false;
            await _executorRepository.UpdateExecutorAsync(executor);
        }

        if (workerId.HasValue)
        {
            var worker = await _workersRepository.GetByIdAsync(workerId.Value)
                ?? throw new ArgumentException($"Сотрудник с ID {workerId.Value} не существует");

            var executor = executors.FirstOrDefault(e => e.WorkerId == workerId.Value);

            if (executor == null)
            {
                executor = new TaskExecutor
                {
                    TaskId = taskId,
                    WorkerId = workerId.Value,
                    IsResponsible = true
                };
                await _executorRepository.AddExecutorAsync(executor);
            }
            else
            {
                executor.IsResponsible = true;
                await _executorRepository.UpdateExecutorAsync(executor);
            }
        }

        var responsibleCount = await _executorRepository.CountResponsibleExecutorsAsync(taskId);
        if (responsibleCount > 1)
        {
            throw new InvalidOperationException("Только один исполнитель может быть ответственным");
        }

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = workerId.HasValue
                ? $"Сотрудник {workerId.Value} назначен ответственным исполнителем задачи {taskId}."
                : $"Ответственный исполнитель для задачи {taskId} удален.",
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
                Text = workerId.HasValue
                    ? $"Сотрудник {workerId.Value} назначен ответственным исполнителем задачи {taskId}."
                    : $"Ответственный исполнитель для задачи {taskId} удален.",
                RelatedEntityName = "Исполнитель задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }
}