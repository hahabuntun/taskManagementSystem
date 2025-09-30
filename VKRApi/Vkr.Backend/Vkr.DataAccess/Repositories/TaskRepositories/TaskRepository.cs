using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Domain.DTO;
using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Repositories.TaskRepositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskDTO> GetByIdAsync(int id)
{
    var task = await LoadTaskWithRelationsAsync(id);
    if (task == null)
        throw new KeyNotFoundException($"Task with ID {id} not found");

    var relationshipTypes = await _context.TaskRelationshipTypes
        .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);

    return MapToTaskDTO(task, relationshipTypes, false);
}

    public async Task<IEnumerable<TaskDTO>> GetAllAsync()
    {
        var tasks = await _context.Tasks
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskType)
            .Include(t => t.TaskPriority)
            .Include(t => t.Project)
            .Include(t => t.Creator)
            .Include(t => t.Sprint)
            .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(t => t.TaskObservers).ThenInclude(to => to.Worker)
            .Include(t => t.Tags)
            .Include(t => t.TaskLinks)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Project)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .OrderByDescending(t => t.CreatedOn)
            .ToListAsync();

        var relationshipTypes = await _context.TaskRelationshipTypes
            .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);

        return tasks.Select(task => MapToTaskDTO(task, relationshipTypes)).ToList();
    }

    public async Task<IEnumerable<TaskDTO>> GetBySprintIdAsync(int sprintId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.SprintId == sprintId)
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskType)
            .Include(t => t.TaskPriority)
            .Include(t => t.Project)
            .Include(t => t.Creator)
            .Include(t => t.Sprint)
            .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(t => t.TaskObservers).ThenInclude(to => to.Worker)
            .Include(t => t.Tags)
            .Include(t => t.TaskLinks)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Project)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .OrderBy(t => t.ExpireOn)
            .ToListAsync();

        var relationshipTypes = await _context.TaskRelationshipTypes
            .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);

        return tasks.Select(task => MapToTaskDTO(task, relationshipTypes)).ToList();
    }

    public async Task<IEnumerable<TaskDTO>> GetByAssigneeIdAsync(int workerId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.TaskExecutors.Any(te => te.WorkerId == workerId))
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskType)
            .Include(t => t.TaskPriority)
            .Include(t => t.Project)
            .Include(t => t.Creator)
            .Include(t => t.Sprint)
            .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(t => t.TaskObservers).ThenInclude(to => to.Worker)
            .Include(t => t.Tags)
            .Include(t => t.TaskLinks)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Project)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .OrderByDescending(t => t.ExpireOn)
            .ToListAsync();

        var relationshipTypes = await _context.TaskRelationshipTypes
            .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);

        return tasks.Select(task => MapToTaskDTO(task, relationshipTypes)).ToList();
    }

    public async Task<Tasks> GetEntityByIdAsync(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.Tags)
            .Include(t => t.Project)
            .Include(t => t.TaskLinks)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException($"Task with ID {id} not found");

        return task;
    }

    public async Task<IEnumerable<RelatedTaskDTO>> GetRelatedTasksAsync(int taskId)
{
    var task = await _context.Tasks
        .Include(t => t.Project).ThenInclude(p => p.ProjectStatus)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Project).ThenInclude(p => p.ProjectStatus)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskType)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskStatus)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskPriority)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Creator)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Sprint)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskExecutors).ThenInclude(te => te.Worker)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskObservers).ThenInclude(to => to.Worker)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Tags)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.Project).ThenInclude(p => p.ProjectStatus)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.RelationshipType)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskType)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskStatus)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskPriority)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.Creator)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.Sprint)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskExecutors).ThenInclude(te => te.Worker)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskObservers).ThenInclude(to => to.Worker)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.Tags)
        .FirstOrDefaultAsync(t => t.Id == taskId)
        ?? throw new KeyNotFoundException($"Task with ID {taskId} not found");

    var relationshipTypes = await _context.TaskRelationshipTypes
        .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);

    var relatedTasks = new List<RelatedTaskDTO>();

    if (task.TaskRelationships != null)
    {
        relatedTasks.AddRange(task.TaskRelationships
            .Where(tr => tr != null && tr.RelatedTask != null)
            .Select(tr => new RelatedTaskDTO
            {
                Task = MapToTaskDTO(tr.RelatedTask, relationshipTypes, true),
                RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name) ? name : "Unknown",
                RelationshipId = tr.Id
            }));
    }

    if (task.RelatedTaskRelationships != null)
    {
        relatedTasks.AddRange(task.RelatedTaskRelationships
            .Where(tr => tr != null && tr.Task != null)
            .Select(tr => new RelatedTaskDTO
            {
                Task = MapToTaskDTO(tr.Task, relationshipTypes, true),
                RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name)
                    ? (name == "ParentChild" ? "Parent" : name)
                    : "Unknown",
                RelationshipId = tr.Id
            }));
    }

    return relatedTasks;
}

    public async Task<IEnumerable<TaskDTO>> GetAvailableRelatedTasksAsync(int taskId)
    {
        var task = await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.TaskRelationships)
            .Include(t => t.RelatedTaskRelationships)
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new KeyNotFoundException($"Task with ID {taskId} not found");

        var allTasks = await _context.Tasks
            .Where(t => t.ProjectId == task.ProjectId && t.Id != taskId)
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskType)
            .Include(t => t.TaskPriority)
            .Include(t => t.Project)
            .Include(t => t.Creator)
            .Include(t => t.Sprint)
            .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(t => t.TaskObservers).ThenInclude(to => to.Worker)
            .Include(t => t.Tags)
            .Include(t => t.TaskLinks)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Project)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .ToListAsync();

        var relatedTaskIds = new HashSet<int>();
        relatedTaskIds.UnionWith(task.TaskRelationships.Select(tr => tr.RelatedTaskId));
        relatedTaskIds.UnionWith(task.RelatedTaskRelationships.Select(tr => tr.TaskId));

        var availableTasks = allTasks
            .Where(t => !relatedTaskIds.Contains(t.Id))
            .ToList();

        var relationshipTypes = await _context.TaskRelationshipTypes
            .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);

        return availableTasks.Select(task => MapToTaskDTO(task, relationshipTypes)).ToList();
    }

    public async Task AddAsync(Tasks task, int[] existingTagIds, (string Name, string Color)[] newTags, CreateLinkDTO[] links)
    {
        var now = DateTime.UtcNow;
        var minDate = now.AddYears(-3);
        var maxDate = now.AddYears(3);

        if (task.StartOn.HasValue && (task.StartOn < minDate || task.StartOn > maxDate))
            throw new ArgumentException($"Start date must be between {minDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd}.");
        
        if (task.ExpireOn.HasValue && (task.ExpireOn < minDate || task.ExpireOn > maxDate))
            throw new ArgumentException($"End date must be between {minDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd}.");

        if (task.ProjectId <= 0 || !await _context.Projects.AnyAsync(p => p.Id == task.ProjectId))
            throw new ArgumentException($"Project with ID {task.ProjectId} does not exist");

        var validTagIds = await _context.Tags
            .Where(t => existingTagIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToArrayAsync();

        if (validTagIds.Length != existingTagIds.Length)
            throw new ArgumentException("One or more existing tag IDs are invalid");

        task.CreatedOn = DateTime.UtcNow;
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        var existingTags = await _context.Tags
            .Where(t => validTagIds.Contains(t.Id))
            .ToListAsync();

        var newTagEntities = newTags.Select(nt => new Tags
        {
            Name = nt.Name,
            Color = nt.Color
        }).ToList();

        if (newTagEntities.Any())
        {
            _context.Tags.AddRange(newTagEntities);
            await _context.SaveChangesAsync();
        }

        task.Tags = existingTags.Concat(newTagEntities).ToList();

        var linkEntities = links.Select(l => new TaskLink
        {
            TaskId = task.Id,
            Link = l.Link,
            Description = l.Description,
            CreatedOn = DateTime.UtcNow
        }).ToList();

        if (linkEntities.Any())
        {
            _context.TaskLinks.AddRange(linkEntities);
            await _context.SaveChangesAsync();
        }

        task.TaskLinks = linkEntities;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tasks task, int[] existingTagIds, (string Name, string Color)[] newTags)
    {
        var now = DateTime.UtcNow;
        var minDate = now.AddYears(-3);
        var maxDate = now.AddYears(3);

        if (task.StartOn.HasValue && (task.StartOn < minDate || task.StartOn > maxDate))
            throw new ArgumentException($"Start date must be between {minDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd}.");
        if (task.ExpireOn.HasValue && (task.ExpireOn < minDate || task.ExpireOn > maxDate))
            throw new ArgumentException($"End date must be between {minDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd}.");
        if (task.ProjectId <= 0 || !await _context.Projects.AnyAsync(p => p.Id == task.ProjectId))
            throw new ArgumentException($"Project with ID {task.ProjectId} does not exist");

        var existingTask = await _context.Tasks
            .Include(t => t.Tags)
            .Include(t => t.TaskLinks)
            .FirstOrDefaultAsync(t => t.Id == task.Id)
            ?? throw new KeyNotFoundException($"Task with ID {task.Id} not found");


        existingTask.ShortName = task.ShortName;
        existingTask.Description = task.Description;
        existingTask.Progress = task.Progress;
        existingTask.StartOn = task.StartOn;
        existingTask.ExpireOn = task.ExpireOn;
        existingTask.ProjectId = task.ProjectId;
        existingTask.CreatorId = task.CreatorId;
        existingTask.TaskTypeId = task.TaskTypeId;
        existingTask.TaskStatusId = task.TaskStatusId;
        existingTask.TaskPriorityId = task.TaskPriorityId;
        existingTask.SprintId = task.SprintId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id)
            ?? throw new KeyNotFoundException($"Task with ID {id} not found");

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }


    public async Task AddTaskRelationshipAsync(int taskId, int relatedTaskId, int relationshipTypeId)
    {
        var task = await _context.Tasks.FindAsync(taskId)
            ?? throw new KeyNotFoundException($"Task with ID {taskId} not found");

        var relatedTask = await _context.Tasks.FindAsync(relatedTaskId)
            ?? throw new KeyNotFoundException($"Related task with ID {relatedTaskId} not found");

        if (task.ProjectId != relatedTask.ProjectId)
            throw new ArgumentException("Tasks must belong to the same project");

        var relationshipType = await _context.TaskRelationshipTypes.FindAsync(relationshipTypeId)
            ?? throw new ArgumentException($"Relationship type with ID {relationshipTypeId} does not exist");

        if (taskId == relatedTaskId)
            throw new ArgumentException("A task cannot have a relationship with itself");

        if (await _context.TaskRelationships.AnyAsync(tr =>
            tr.TaskId == taskId && tr.RelatedTaskId == relatedTaskId && tr.TaskRelationshipTypeId == relationshipTypeId))
            throw new ArgumentException("This task relationship already exists");

        if (relationshipTypeId == 5)
        {
            if (await _context.TaskRelationships.AnyAsync(tr =>
                tr.TaskId == relatedTaskId && tr.RelatedTaskId == taskId && tr.TaskRelationshipTypeId == 5))
                throw new ArgumentException("Cannot create ParentChild relationship due to existing reverse hierarchy");
        }

        var relationship = new TaskRelationship
        {
            TaskId = taskId,
            RelatedTaskId = relatedTaskId,
            TaskRelationshipTypeId = relationshipTypeId
        };

        _context.TaskRelationships.Add(relationship);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveTaskRelationshipAsync(int relationshipId)
    {
        var relationship = await _context.TaskRelationships.FindAsync(relationshipId)
            ?? throw new KeyNotFoundException($"Task relationship with ID {relationshipId} not found");

        _context.TaskRelationships.Remove(relationship);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveTaskRelationshipByTaskIdsAsync(int taskId, int relatedTaskId)
    {
        var relationship = await _context.TaskRelationships
            .FirstOrDefaultAsync(tr => (tr.TaskId == taskId && tr.RelatedTaskId == relatedTaskId) ||
                                      (tr.TaskId == relatedTaskId && tr.RelatedTaskId == taskId))
            ?? throw new KeyNotFoundException($"No relationship exists between tasks {taskId} and {relatedTaskId}");

        _context.TaskRelationships.Remove(relationship);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<TaskDTO>> GetAllTasksWithRelationsAsync()
    {
        var tasks = await _context.Tasks
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskPriority)
            .Include(t => t.Project)
            .Include(t => t.Sprint)
            .Include(t => t.Creator)
            .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(t => t.Tags)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Project)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .ToListAsync();

        var relationshipTypes = await _context.TaskRelationshipTypes
            .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);

        return tasks.Select(task => MapToTaskDTO(task, relationshipTypes)).ToList();
    }

    private async Task<Tasks> LoadTaskWithRelationsAsync(int id)
{
    return await _context.Tasks
        .Include(t => t.Creator)
        .Include(t => t.TaskType)
        .Include(t => t.TaskStatus)
        .Include(t => t.TaskPriority)
        .Include(t => t.Project).ThenInclude(p => p.ProjectStatus)
        .Include(t => t.Sprint)
        .Include(t => t.TaskObservers).ThenInclude(to => to.Worker)
        .Include(t => t.BoardTasks)
        .Include(t => t.TaskLinks)
        .Include(t => t.TaskMessages)
        .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Project).ThenInclude(p => p.ProjectStatus)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskType)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskStatus)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskPriority)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Creator)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Sprint)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskExecutors).ThenInclude(te => te.Worker)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.TaskObservers).ThenInclude(to => to.Worker)
        .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Tags)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.Project).ThenInclude(p => p.ProjectStatus)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.RelationshipType)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskType)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskStatus)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskPriority)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.Creator)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.Sprint)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskExecutors).ThenInclude(te => te.Worker)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.TaskObservers).ThenInclude(to => to.Worker)
        .Include(t => t.RelatedTaskRelationships).ThenInclude(tr => tr.Task).ThenInclude(t => t.Tags)
        .Include(t => t.Tags)
        .FirstOrDefaultAsync(t => t.Id == id);
}

    private TaskDTO MapToTaskDTO(Tasks task, Dictionary<int, string> relationshipTypes, bool isRelatedTask = false)
{
    if (task == null) throw new ArgumentNullException(nameof(task));
    if (relationshipTypes == null) throw new ArgumentNullException(nameof(relationshipTypes));

    var relatedTasks = new List<RelatedTaskDTO>();

    // Only populate RelatedTasks for the primary task (not for related tasks)
    if (!isRelatedTask)
    {
        if (task.TaskRelationships != null)
        {
            relatedTasks.AddRange(task.TaskRelationships
                .Where(tr => tr != null && tr.RelatedTask != null)
                .Select(tr => new RelatedTaskDTO
                {
                    Task = MapToTaskDTO(tr.RelatedTask, relationshipTypes, true),
                    RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name) ? name : "Unknown",
                    RelationshipId = tr.Id
                }));
        }

        if (task.RelatedTaskRelationships != null)
        {
            relatedTasks.AddRange(task.RelatedTaskRelationships
                .Where(tr => tr != null && tr.Task != null)
                .Select(tr => new RelatedTaskDTO
                {
                    Task = MapToTaskDTO(tr.Task, relationshipTypes, true),
                    RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name)
                        ? (name == "ParentChild" ? "Parent" : name)
                        : "Unknown",
                    RelationshipId = tr.Id
                }));
        }
    }

    var responsibleExecutor = task.TaskExecutors?.FirstOrDefault(te => te.IsResponsible);

    if (responsibleExecutor == null && task.TaskExecutors?.Any() == true)
    {
        Console.WriteLine($"Warning: No responsible executor found for TaskId={task.Id}. Executors: {string.Join(", ", task.TaskExecutors.Select(te => te.WorkerId))}");
    }

    var responsibleCount = task.TaskExecutors?.Count(te => te.IsResponsible) ?? 0;
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
        Project = task.Project != null ? new ProjectDTO
        {
            Id = task.Project.Id,
            Name = task.Project.Name,
            Description = task.Project.Description,
            Goal = task.Project.Goal,
            ManagerId = task.Project.ManagerId,
            Progress = task.Project.Progress,
            ProjectStatusName = task.Project.ProjectStatus?.Name
        } : null,
        Creator = task.Creator != null ? new WorkerDTO
        {
            Id = task.Creator.Id,
            Name = task.Creator.Name,
            SecondName = task.Creator.SecondName,
            Email = task.Creator.Email,
            CreatedOn = task.Creator.CreatedOn,
            WorkerPositionId = task.Creator.WorkerPositionId,
            CanManageWorkers = task.Creator.CanManageWorkers,
            CanManageProjects = task.Creator.CanManageProjects
        } : null,
        TaskType = task.TaskType != null ? new TaskTypeDTO { Id = task.TaskType.Id, Name = task.TaskType.Name } : null,
        TaskStatus = task.TaskStatus != null ? new TaskStatusDTO { Id = task.TaskStatus.Id, Name = task.TaskStatus.Name, Color = task.TaskStatus.Color } : null,
        TaskPriority = task.TaskPriority != null ? new TaskPriorityDTO { Id = task.TaskPriority.Id, Name = task.TaskPriority.Name, Color = task.TaskPriority.Color } : null,
        Sprint = task.Sprint != null ? new SprintDTO { Id = task.Sprint.Id, Title = task.Sprint.Title } : null,
        Executors = task.TaskExecutors?.Select(te => te.Worker != null ? new WorkerDTO
        {
            Id = te.Worker.Id,
            Name = te.Worker.Name,
            SecondName = te.Worker.SecondName,
            Email = te.Worker.Email,
            CreatedOn = te.Worker.CreatedOn,
            WorkerPositionId = te.Worker.WorkerPositionId,
            CanManageWorkers = te.Worker.CanManageWorkers,
            CanManageProjects = te.Worker.CanManageProjects
        } : null).Where(w => w != null).ToList() ?? new List<WorkerDTO>(),
        Observers = task.TaskObservers?.Select(to => to.Worker != null ? new WorkerDTO
        {
            Id = to.Worker.Id,
            Name = to.Worker.Name,
            SecondName = to.Worker.SecondName,
            Email = to.Worker.Email,
            CreatedOn = to.Worker.CreatedOn,
            WorkerPositionId = to.Worker.WorkerPositionId,
            CanManageWorkers = to.Worker.CanManageWorkers,
            CanManageProjects = to.Worker.CanManageProjects
        } : null).Where(w => w != null).ToList() ?? new List<WorkerDTO>(),
        ResponsibleWorker = responsibleExecutor?.Worker != null ? new WorkerDTO
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
        TagDTOs = task.Tags?.Select(t => t != null ? new FullTagDTO { Id = t.Id, Name = t.Name, Color = t.Color } : null).Where(t => t != null).ToList() ?? new List<FullTagDTO>(),
    };
}

    public async Task<TaskRelationship?> GetTaskRelationshipByIdAsync(int relationshipId)
    {
        return await _context.TaskRelationships.FindAsync(relationshipId);
    }
}

internal class TaskLinkDTO
{
    public int Id { get; set; }
    public object Url { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOn { get; set; }
}