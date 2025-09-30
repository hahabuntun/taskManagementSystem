using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.BoardRepositories;
using Vkr.Application.Interfaces.Repositories.TagRepositories;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Application.Interfaces.Services.TaskServices;
using Vkr.Application.Interfaces.Services.HistoryServices; // Добавлено
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Notification;
using Vkr.Domain.Entities.History; // Добавлено

namespace Vkr.Application.Services.TaskServices;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService; // Добавлено

    public TaskService(
        ITaskRepository taskRepository,
        ITagRepository tagRepository,
        IBoardRepository boardRepository,
        INotificationService notificationService,
        IHistoryService historyService) // Добавлено
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService)); // Добавлено
    }

    public async Task<TaskDTO> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TaskDTO>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task<IEnumerable<TaskDTO>> GetTasksBySprintAsync(int sprintId)
    {
        return await _taskRepository.GetBySprintIdAsync(sprintId);
    }

    public async Task<IEnumerable<TaskDTO>> GetTasksByAssigneeAsync(int workerId)
    {
        return await _taskRepository.GetByAssigneeIdAsync(workerId);
    }

    public async Task<int> CreateTaskAsync(CreateTaskDTO options)
    {
        ValidateTaskOptions(options);
        if (options.CreatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(options.CreatorId));

        var task = new Tasks
        {
            ShortName = options.Name,
            Description = options.Description,
            Progress = options.Progress,
            StoryPoints = options.StoryPoints,
            StartOn = options.StartDate,
            ExpireOn = options.EndDate,
            ProjectId = options.ProjectId,
            CreatorId = options.CreatorId,
            TaskTypeId = options.TaskTypeId,
            TaskStatusId = options.TaskStatusId,
            TaskPriorityId = options.TaskPriorityId,
            SprintId = options.SprintId
        };

        await _taskRepository.AddAsync(
            task,
            options.ExistingTagIds,
            options.NewTags.Select(t => (t.Name, t.Color)).ToArray(),
            options.Links);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Задача '{task.ShortName}' создана.",
            RelatedEntityId = task.ProjectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = options.CreatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the project
        var subscriptions = await _notificationService.GetEntitySubscriptions(task.ProjectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == options.ProjectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Новая задача '{task.ShortName}' добавлена в проект {task.Project.Name}",
                RelatedEntityName = task.ShortName ?? "Задача",
                RelatedEntityId = task.Id,
                RelatedEntityType = EntityType.Task,
                CreatorId = options.CreatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return task.Id;
    }

    public async Task UpdateTaskAsync(int id, CreateTaskDTO options, int creatorId)
{
    if (creatorId <= 0)
        throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

    var existingTask = await _taskRepository.GetEntityByIdAsync(id)
        ?? throw new KeyNotFoundException($"Задача с ID {id} не найдена");

    existingTask.ShortName = options.Name;
    existingTask.StoryPoints = options.StoryPoints;
    existingTask.Description = options.Description;
    existingTask.Progress = options.Progress;
    existingTask.StartOn = options.StartDate;
    existingTask.ExpireOn = options.EndDate;
    existingTask.TaskTypeId = options.TaskTypeId;
    existingTask.TaskStatusId = options.TaskStatusId;
    existingTask.TaskPriorityId = options.TaskPriorityId;

    // Handle sprint changes
    if (options.SprintId.HasValue)
    {
        existingTask.SprintId = options.SprintId;
        var sprintHistoryDto = new CreateHistoryDTO
        {
            Text = $"Задача '{existingTask.ShortName}' добавлена в спринт.",
            RelatedEntityId = options.SprintId.Value,
            RelatedEntityType = HistoryEntityType.Sprint,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(sprintHistoryDto);
    }
    else if (!options.SprintId.HasValue && existingTask.SprintId.HasValue)
    {
        var oldSprintId = existingTask.SprintId.Value; // Store the original SprintId
        existingTask.SprintId = null; // Set to null after storing
        var sprintHistoryDto = new CreateHistoryDTO
        {
            Text = $"Задача '{existingTask.ShortName}' удалена из спринта.",
            RelatedEntityId = oldSprintId, // Use the stored SprintId
            RelatedEntityType = HistoryEntityType.Sprint,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(sprintHistoryDto);
    }

    await _taskRepository.UpdateAsync(
        existingTask,
        options.ExistingTagIds,
        options.NewTags.Select(t => (t.Name, t.Color)).ToArray());

    // Add history record for task update
    var historyDto = new CreateHistoryDTO
    {
        Text = $"Задача '{existingTask.ShortName}' обновлена.",
        RelatedEntityId = id,
        RelatedEntityType = HistoryEntityType.Task,
        CreatorId = creatorId
    };
    await _historyService.AddHistoryItemAsync(historyDto);

    // Send notification to workers subscribed to the task
    var subscriptions = await _notificationService.GetEntitySubscriptions(id, EntityType.Task);
    var subscribedWorkerIds = subscriptions
        .Where(s => s.EntityType == EntityType.Task && s.EntityId == id)
        .Select(s => s.WorkerId)
        .ToList();

    if (subscribedWorkerIds.Any())
    {
        var notificationDto = new CreateNotificationDto
        {
            Text = $"Задача '{existingTask.ShortName}' обновлена.",
            RelatedEntityName = existingTask.ShortName ?? "Задача",
            RelatedEntityId = id,
            RelatedEntityType = EntityType.Task,
            CreatorId = creatorId,
            WorkerIds = subscribedWorkerIds
        };
        await _notificationService.CreateNotificationAsync(notificationDto);
    }
}

    public async Task DeleteTaskAsync(int id, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        var existingTask = await _taskRepository.GetEntityByIdAsync(id)
            ?? throw new KeyNotFoundException($"Задача с ID {id} не найдена");
        var taskName = existingTask.ShortName;
        var project = existingTask.Project;

        await _taskRepository.DeleteAsync(id);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Задача '{taskName}' удалена из проекта ${project.Name}.",
            RelatedEntityId = project.Id,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the project
        var subscriptions = await _notificationService.GetEntitySubscriptions(project.Id, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == project.Id)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Задача '{taskName}' удалена из проекта {project.Name}.",
                RelatedEntityName = taskName ?? "Задача",
                RelatedEntityId = project.Id,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task AddTaskRelationshipAsync(int taskId, int relatedTaskId, int relationshipTypeId, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Задача с ID {taskId} не найдена");
        var relatedTask = await _taskRepository.GetEntityByIdAsync(relatedTaskId)
            ?? throw new KeyNotFoundException($"Связанная задача с ID {relatedTaskId} не найдена");

        await _taskRepository.AddTaskRelationshipAsync(taskId, relatedTaskId, relationshipTypeId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Добавлена связь между задачей '{task.ShortName}' и задачей '{relatedTask.ShortName}'.",
            RelatedEntityId = taskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the task
        var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Добавлена связь между задачей '{task.ShortName}' и задачей '{relatedTask.ShortName}'.",
                RelatedEntityName = task.ShortName ?? "Задача",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task RemoveTaskRelationshipAsync(int relationshipId, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        var relationship = await _taskRepository.GetTaskRelationshipByIdAsync(relationshipId)
            ?? throw new KeyNotFoundException($"Связь с ID {relationshipId} не найдена");
        var taskId = relationship.TaskId;
        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Задача с ID {taskId} не найдена");

        await _taskRepository.RemoveTaskRelationshipAsync(relationshipId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Связь с {relationship.RelatedTask.ShortName} удалена.",
            RelatedEntityId = taskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the task
        var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Связь с {relationship.RelatedTask.ShortName} удалена для задачи '{task.ShortName}'.",
                RelatedEntityName = task.ShortName ?? "Задача",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task RemoveTaskRelationshipByTaskIdsAsync(int taskId, int relatedTaskId, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Задача с ID {taskId} не найдена");
        var relatedTask = await _taskRepository.GetEntityByIdAsync(relatedTaskId)
            ?? throw new KeyNotFoundException($"Связанная задача с ID {relatedTaskId} не найдена");

        await _taskRepository.RemoveTaskRelationshipByTaskIdsAsync(taskId, relatedTaskId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Связь c задачей '{relatedTask.ShortName}' удалена.",
            RelatedEntityId = taskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the task
        var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Связь между задачей '{task.ShortName}' и задачей '{relatedTask.ShortName}' удалена.",
                RelatedEntityName = task.ShortName ?? "Задача",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task<IEnumerable<RelatedTaskDTO>> GetRelatedTasksAsync(int taskId)
    {
        return await _taskRepository.GetRelatedTasksAsync(taskId);
    }

    public async Task<IEnumerable<TaskDTO>> GetAvailableRelatedTasksAsync(int taskId)
    {
        return await _taskRepository.GetAvailableRelatedTasksAsync(taskId);
    }

    public async Task<List<Tags>> GetAvailableTags(int taskId)
    {
        return await _tagRepository.GetAvailableTagsForTaskAsync(taskId);
    }

    public async Task<List<Tags>> GetAllTags()
    {
        return await _tagRepository.GetAllTagsForTasksAsync();
    }

    private static void ValidateTaskOptions(CreateTaskDTO options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.Name))
            throw new ArgumentException("Название задачи обязательно.", nameof(options.Name));

        if (options.TaskTypeId <= 0)
            throw new ArgumentException("Идентификатор типа задачи должен быть положительным.", nameof(options.TaskTypeId));

        if (options.TaskStatusId <= 0)
            throw new ArgumentException("Идентификатор статуса задачи должен быть положительным.", nameof(options.TaskStatusId));

        if (options.TaskPriorityId <= 0)
            throw new ArgumentException("Идентификатор приоритета задачи должен быть положительным.", nameof(options.TaskPriorityId));

        if (options.EndDate.HasValue && options.StartDate.HasValue && options.EndDate < options.StartDate)
            throw new ArgumentException("Дата окончания не может быть раньше даты начала.", nameof(options.EndDate));

        if (options.ProjectId <= 0)
            throw new ArgumentException("Идентификатор проекта должен быть положительным.", nameof(options.ProjectId));
    }
}