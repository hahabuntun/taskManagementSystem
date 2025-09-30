using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.BoardRepositories;
using Vkr.Application.Interfaces.Services.BoardServices;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.DTO.Board;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.Entities.Board;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Worker;
using Vkr.Domain.Entities.Notification;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;
using Vkr.Domain.DTO;

namespace Vkr.Application.Services.BoardServices;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public BoardService(
        IBoardRepository boardRepository,
        ITaskRepository taskRepository,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _boardRepository = boardRepository;
        _taskRepository = taskRepository;
        _notificationService = notificationService;
        _historyService = historyService;
    }

    public async Task<IEnumerable<BoardSummaryDto>> GetProjectBoardsAsync(int projectId)
    {
        var boards = await _boardRepository.GetProjectBoardsAsync(projectId);
        return boards.Select(MapToSummaryDto);
    }

    public async Task<BoardSummaryDto> AddProjectBoardAsync(int projectId, BoardCreateDto data, Workers creator)
    {
        var board = new Boards
        {
            Name = data.Name,
            Description = data.Description,
            ProjectId = projectId,
            OwnerId = null, // Project board, no owner
            CreatedOn = DateTime.UtcNow,
            Basis = data.Basis
        };
        await _boardRepository.AddBoardAsync(board);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Доска '{board.Name}' создана для проекта.",
            RelatedEntityId = board.Id,
            RelatedEntityType = HistoryEntityType.Board,
            CreatorId = creator.Id
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        return new BoardSummaryDto
        {
            Id = board.Id,
            Name = board.Name,
            Basis = board.Basis,
            CreatedOn = board.CreatedOn,
            OwnerId = board.OwnerId,
            ProjectId = board.ProjectId,
        };
    }

    public async Task<IEnumerable<BoardSummaryDto>> GetWorkerProjectBoardsAsync(int workerId)
    {
        var boards = await _boardRepository.GetWorkerProjectBoardsAsync(workerId);
        return boards.Select(MapToSummaryDto);
    }

    public async Task<IEnumerable<BoardSummaryDto>> GetWorkerPersonalBoardsAsync(int workerId)
    {
        var boards = await _boardRepository.GetWorkerPersonalBoardsAsync(workerId);
        return boards.Select(MapToSummaryDto);
    }

    public async Task<BoardSummaryDto> AddWorkerBoardAsync(int workerId, BoardCreateDto data, int creatorId)
    {
        var board = new Boards
        {
            Name = data.Name,
            Description = data.Description,
            OwnerId = workerId,
            ProjectId = null, // Personal board, no project
            CreatedOn = DateTime.UtcNow,
            Basis = data.Basis
        };
        await _boardRepository.AddBoardAsync(board);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Личная доска '{board.Name}' создана.",
            RelatedEntityId = board.Id,
            RelatedEntityType = HistoryEntityType.Board,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to the worker
        var notificationDto = new CreateNotificationDto
        {
            Text = $"Личная доска '{board.Name}' создана.",
            RelatedEntityName = board.Name,
            RelatedEntityId = board.Id,
            RelatedEntityType = EntityType.Board,
            CreatorId = creatorId,
            WorkerIds = new List<int> { workerId }
        };
        await _notificationService.CreateNotificationAsync(notificationDto);

        return new BoardSummaryDto
        {
            Id = board.Id,
            Name = board.Name,
            Basis = board.Basis,
            CreatedOn = board.CreatedOn,
            OwnerId = board.OwnerId,
            ProjectId = board.ProjectId,
        };
    }

    public async Task<BoardSummaryDto> GetBoardAsync(int boardId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");
        return new BoardSummaryDto
        {
            Id = board.Id,
            Name = board.Name,
            Basis = board.Basis,
            CreatedOn = board.CreatedOn,
            OwnerId = board.OwnerId,
            ProjectId = board.ProjectId,
        };
    }

    public async Task<BoardSummaryDto> EditBoardAsync(int boardId, BoardUpdateDto data, int creatorId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        board.Name = data.Name;
        board.Description = data.Description;
        await _boardRepository.UpdateBoardAsync(board);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Доска '{board.Name}' обновлена.",
            RelatedEntityId = boardId,
            RelatedEntityType = HistoryEntityType.Board,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to subscribed workers
        var subscriptions = await _notificationService.GetEntitySubscriptions(board.Id, EntityType.Board);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Board && s.EntityId == boardId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Доска '{board.Name}' обновлена.",
                RelatedEntityName = board.Name,
                RelatedEntityId = boardId,
                RelatedEntityType = EntityType.Board,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return new BoardSummaryDto
        {
            Id = board.Id,
            Name = board.Name,
            Basis = board.Basis,
            CreatedOn = board.CreatedOn,
            OwnerId = board.OwnerId,
            ProjectId = board.ProjectId,
        };
    }

    public async Task RemoveBoardAsync(int boardId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        await _boardRepository.RemoveBoardAsync(boardId);
    }

    public async Task<IEnumerable<BoardColumnDto>> GetBoardColumnsAsync(int boardId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        if (board.Basis != BoardBasis.CustomColumns)
            throw new InvalidOperationException("Колонки доступны только для пользовательских досок");

        var columns = await _boardRepository.GetBoardColumnsAsync(boardId);
        return columns.Select(c => new BoardColumnDto
        {
            Name = c.Name,
            BoardId = c.BoardId,
            Order = c.Order
        });
    }

    public async Task<BoardColumnDto> AddColumnToBoardAsync(int boardId, BoardColumnCreateDto data, int creatorId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        if (board.Basis != BoardBasis.CustomColumns)
            throw new InvalidOperationException("Колонки можно добавлять только к пользовательским доскам");

        var existingColumns = await _boardRepository.GetBoardColumnsAsync(boardId);
        var column = new BoardColumns
        {
            Name = data.Name,
            BoardId = boardId,
            Order = existingColumns.Any() ? existingColumns.Max(c => c.Order) + 1 : 1
        };
        await _boardRepository.AddColumnAsync(column);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Новая колонка '{data.Name}' добавлена к доске '{board.Name}'.",
            RelatedEntityId = boardId,
            RelatedEntityType = HistoryEntityType.Board,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to subscribed workers
        var subscriptions = await _notificationService.GetEntitySubscriptions(board.Id, EntityType.Board);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Board && s.EntityId == boardId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Новая колонка '{data.Name}' добавлена к доске '{board.Name}'.",
                RelatedEntityName = board.Name,
                RelatedEntityId = boardId,
                RelatedEntityType = EntityType.Board,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return new BoardColumnDto
        {
            Name = column.Name,
            BoardId = column.BoardId,
            Order = column.Order
        };
    }

    public async Task RemoveColumnFromBoardAsync(int boardId, string columnName, int creatorId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        if (board.Basis != BoardBasis.CustomColumns)
            throw new InvalidOperationException("Колонки можно удалять только из пользовательских досок");

        await _boardRepository.RemoveColumnAsync(boardId, columnName);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Колонка '{columnName}' удалена из доски '{board.Name}'.",
            RelatedEntityId = boardId,
            RelatedEntityType = HistoryEntityType.Board,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to subscribed workers
        var subscriptions = await _notificationService.GetEntitySubscriptions(board.Id, EntityType.Board);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Board && s.EntityId == boardId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Колонка '{columnName}' удалена из доски '{board.Name}'.",
                RelatedEntityName = board.Name,
                RelatedEntityId = boardId,
                RelatedEntityType = EntityType.Board,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task<IEnumerable<TaskDTO>> GetBoardTasksAsync(int boardId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        var tasks = await _boardRepository.GetBoardTasksAsync(boardId);
        var relationshipTypes = await _boardRepository.GetRelationshipTypesAsync();
        return tasks.Select(t => MapToTaskDto(t, relationshipTypes));
    }

    public async Task<IEnumerable<BoardTaskWithDetailsDto>> GetCustomBoardTasksAsync(int boardId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        if (board.Basis != BoardBasis.CustomColumns)
            throw new InvalidOperationException("Пользовательские задачи доступны только для пользовательских досок");

        var boardTasks = await _boardRepository.GetCustomBoardTasksAsync(boardId);
        var relationshipTypes = await _boardRepository.GetRelationshipTypesAsync();
        return boardTasks.Select(bt => new BoardTaskWithDetailsDto
        {
            BoardId = bt.BoardId,
            TaskId = bt.TaskId,
            CustomColumnName = bt.CustomColumnName,
            Task = MapToTaskDto(bt.Task, relationshipTypes)
        });
    }

    public async Task<IEnumerable<TaskDTO>> GetAvailableTasksForBoardAsync(int boardId, int workerId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        var tasks = await _boardRepository.GetAvailableTasksForBoardAsync(boardId, workerId);
        var relationshipTypes = await _boardRepository.GetRelationshipTypesAsync();
        return tasks.Select(t => MapToTaskDto(t, relationshipTypes));
    }

    public async Task<BoardTaskDto> AddTaskToBoardAsync(int boardId, AddTaskToBoardDto data, int creatorId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        var task = await _taskRepository.GetEntityByIdAsync(data.TaskId)
            ?? throw new KeyNotFoundException($"Задача с ID {data.TaskId} не найдена");

        if (board.ProjectId.HasValue)
        {
            if (task.ProjectId != board.ProjectId)
                throw new InvalidOperationException($"Задача {data.TaskId} не принадлежит проекту {board.ProjectId}.");
        }

        if (board.Basis == BoardBasis.CustomColumns && data.CustomColumnName != null)
        {
            var column = await _boardRepository.GetBoardColumnAsync(boardId, data.CustomColumnName)
                ?? throw new KeyNotFoundException($"Колонка {data.CustomColumnName} не найдена на доске {boardId}");
        }
        else if (board.Basis == BoardBasis.CustomColumns && data.CustomColumnName == null)
        {
        }
        else if (data.CustomColumnName != null)
        {
            throw new InvalidOperationException("Имя пользовательской колонки можно указать только для пользовательских досок");
        }

        var boardTask = new BoardTask
        {
            BoardId = boardId,
            TaskId = data.TaskId,
            CustomColumnName = data.CustomColumnName
        };
        await _boardRepository.AddTaskToBoardAsync(boardTask);

        // Add history record
        var columnText = data.CustomColumnName != null ? $" в колонку '{data.CustomColumnName}'" : "";
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Задача '{task.ShortName}' добавлена к доске '{board.Name}'{columnText}.",
            RelatedEntityId = boardId,
            RelatedEntityType = HistoryEntityType.Board,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to subscribed workers
        var subscriptions = await _notificationService.GetEntitySubscriptions(board.Id, EntityType.Board);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Board && s.EntityId == boardId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Задача '{task.ShortName}' добавлена к доске '{board.Name}'{columnText}.",
                RelatedEntityName = task.ShortName ?? "Задача",
                RelatedEntityId = task.Id,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return new BoardTaskDto
        {
            BoardId = boardTask.BoardId,
            TaskId = boardTask.TaskId,
            CustomColumnName = boardTask.CustomColumnName
        };
    }

    public async Task ChangeTaskColumnAsync(int boardId, int taskId, string? columnName, int creatorId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        if (board.Basis != BoardBasis.CustomColumns)
            throw new InvalidOperationException("Изменение колонок разрешено только для пользовательских досок");

        var boardTask = await _boardRepository.GetBoardTaskAsync(boardId, taskId)
            ?? throw new KeyNotFoundException($"Задача {taskId} не найдена на доске {boardId}");

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Задача с ID {taskId} не найдена");

        if (columnName != null)
        {
            var column = await _boardRepository.GetBoardColumnAsync(boardId, columnName)
                ?? throw new KeyNotFoundException($"Колонка {columnName} не найдена на доске {boardId}");
        }

        var oldColumnName = boardTask.CustomColumnName;
        boardTask.CustomColumnName = columnName;
        await _boardRepository.UpdateBoardTaskAsync(boardTask);

        // Add history record
        var fromText = oldColumnName != null ? $"из колонки '{oldColumnName}' " : "";
        var toText = columnName != null ? $"в колонку '{columnName}'" : "без колонки";
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Задача '{task.ShortName}' перемещена {fromText}{toText} на доске '{board.Name}'.",
            RelatedEntityId = taskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to subscribed workers
        var subscriptions = await _notificationService.GetEntitySubscriptions(board.Id, EntityType.Board);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Board && s.EntityId == boardId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Задача '{task.ShortName}' перемещена {fromText}{toText} на доске '{board.Name}'.",
                RelatedEntityName = task.ShortName ?? "Задача",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task RemoveTaskFromBoardAsync(int boardId, int taskId, int creatorId)
    {
        var boardTask = await _boardRepository.GetBoardTaskAsync(boardId, taskId)
            ?? throw new KeyNotFoundException($"Задача {taskId} не найдена на доске {boardId}");

        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        var task = await _taskRepository.GetEntityByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Задача с ID {taskId} не найдена");

        await _boardRepository.RemoveTaskFromBoardAsync(boardId, taskId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Задача '{task.ShortName}' удалена из доски '{board.Name}'.",
            RelatedEntityId = boardId,
            RelatedEntityType = HistoryEntityType.Board,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to subscribed workers
        var subscriptions = await _notificationService.GetEntitySubscriptions(board.Id, EntityType.Board);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Board && s.EntityId == boardId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Задача '{task.ShortName}' удалена из доски '{board.Name}'.",
                RelatedEntityName = task.ShortName ?? "Задача",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task ClearBoardAsync(int boardId, int creatorId)
    {
        var board = await _boardRepository.GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Доска с ID {boardId} не найдена");

        await _boardRepository.ClearBoardAsync(boardId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Все задачи удалены с доски '{board.Name}'.",
            RelatedEntityId = boardId,
            RelatedEntityType = HistoryEntityType.Board,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to subscribed workers
        var subscriptions = await _notificationService.GetEntitySubscriptions(board.Id, EntityType.Board);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Board && s.EntityId == boardId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Все задачи очищены из доски '{board.Name}'.",
                RelatedEntityName = board.Name,
                RelatedEntityId = boardId,
                RelatedEntityType = EntityType.Board,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    private BoardSummaryDto MapToSummaryDto(Boards board)
    {
        return new BoardSummaryDto
        {
            Id = board.Id,
            Name = board.Name,
            OwnerId = board.OwnerId,
            ProjectId = board.ProjectId,
            TaskCount = board.BoardTasks?.Count ?? 0,
            Basis = board.Basis,
            CreatedOn = board.CreatedOn
        };
    }

    private TaskDTO MapToTaskDto(Tasks task, Dictionary<int, string> relationshipTypes)
    {
        var relatedTasks = new List<RelatedTaskDTO>();
        relatedTasks.AddRange(task.TaskRelationships.Select(tr => new RelatedTaskDTO
        {
            Task = new TaskDTO { Id = tr.RelatedTask.Id, ShortName = tr.RelatedTask.ShortName ?? string.Empty },
            RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name) ? name : "Unknown",
            RelationshipId = tr.Id
        }));

        relatedTasks.AddRange(task.RelatedTaskRelationships.Select(tr => new RelatedTaskDTO
        {
            Task = new TaskDTO { Id = tr.Task.Id, ShortName = tr.Task.ShortName ?? string.Empty },
            RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name)
                ? (name == "ParentChild" ? "Parent" : name)
                : "Unknown",
            RelationshipId = tr.Id
        }));

        var responsibleExecutor = task.TaskExecutors.FirstOrDefault(te => te.IsResponsible);

        if (responsibleExecutor == null && task.TaskExecutors.Any())
        {
            Console.WriteLine($"Warning: No responsible executor found for TaskId={task.Id}. Executors: {string.Join(", ", task.TaskExecutors.Select(te => te.WorkerId))}");
        }

        var responsibleCount = task.TaskExecutors.Count(te => te.IsResponsible);
        if (responsibleCount > 1)
        {
            Console.WriteLine($"Error: Multiple responsible executors found for TaskId={task.Id}. Executors: {string.Join(", ", task.TaskExecutors.Where(te => te.IsResponsible).Select(te => te.WorkerId))}");
            throw new InvalidOperationException("Multiple responsible executors detected for task. Only one is allowed.");
        }

        return new TaskDTO
        {
            Id = task.Id,
            ShortName = task.ShortName ?? string.Empty,
            Description = task.Description,
            CreatedOn = task.CreatedOn,
            Progress = task.Progress,
            StartOn = task.StartOn,
            ExpireOn = task.ExpireOn,
            StoryPoints = task.StoryPoints,
            Project = new ProjectDTO { Id = task.Project.Id, Name = task.Project.Name },
            Creator = new WorkerDTO { Id = task.Creator.Id, Name = task.Creator.Name },
            TaskType = new TaskTypeDTO { Id = task.TaskType.Id, Name = task.TaskType.Name },
            TaskStatus = new TaskStatusDTO { Id = task.TaskStatus.Id, Name = task.TaskStatus.Name, Color = task.TaskStatus.Color },
            TaskPriority = task.TaskPriority != null ? new TaskPriorityDTO { Id = task.TaskPriority.Id, Name = task.TaskPriority.Name, Color = task.TaskPriority.Color } : null,
            Sprint = task.Sprint != null ? new SprintDTO { Id = task.Sprint.Id, Title = task.Sprint.Title } : null,
            Executors = task.TaskExecutors.Select(te => new WorkerDTO
            {
                Id = te.Worker.Id,
                Name = te.Worker.Name,
                SecondName = te.Worker.SecondName,
                Email = te.Worker.Email,
                CreatedOn = te.Worker.CreatedOn,
                WorkerPositionId = te.Worker.WorkerPositionId,
                CanManageWorkers = te.Worker.CanManageWorkers,
                CanManageProjects = te.Worker.CanManageProjects
            }).ToList(),
            Observers = task.TaskObservers.Select(to => new WorkerDTO
            {
                Id = to.Worker.Id,
                Name = to.Worker.Name,
                SecondName = to.Worker.SecondName,
                Email = to.Worker.Email,
                CreatedOn = to.Worker.CreatedOn,
                WorkerPositionId = to.Worker.WorkerPositionId,
                CanManageWorkers = to.Worker.CanManageWorkers,
                CanManageProjects = to.Worker.CanManageProjects
            }).ToList(),
            ResponsibleWorker = responsibleExecutor != null ? new WorkerDTO
            {
                Id = responsibleExecutor.Worker.Id,
                Name = responsibleExecutor.Worker.Name,
                SecondName = responsibleExecutor.Worker.SecondName,
                Email = responsibleExecutor.Worker.Email,
                CreatedOn = responsibleExecutor.Worker.CreatedOn,
                WorkerPositionId = responsibleExecutor.Worker.WorkerPositionId,
                CanManageWorkers = responsibleExecutor.Worker.CanManageWorkers,
                CanManageProjects = responsibleExecutor.Worker.CanManageProjects
            } : null,
            RelatedTasks = relatedTasks,
            TagDTOs = task.Tags.Select(t => new FullTagDTO { Id = t.Id, Name = t.Name, Color = t.Color }).ToList()
        };
    }
}