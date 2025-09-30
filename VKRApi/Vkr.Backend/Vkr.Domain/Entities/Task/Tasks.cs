using Vkr.Domain.Entities.Board;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Sprint;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Task;

public class Tasks
{
    /// <summary>
    /// Unique identifier for the task
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Short name or title of the task
    /// </summary>
    public string? ShortName { get; set; }

    public int? StoryPoints { get; set; }

    /// <summary>
    /// Detailed description of the task
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Progress of the task (0-100%)
    /// </summary>
    public int Progress { get; set; }

    /// <summary>
    /// Date and time when the task was created
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Date and time when the task is scheduled to start
    /// </summary>
    public DateTime? StartOn { get; set; }

    /// <summary>
    /// Deadline for task completion
    /// </summary>
    public DateTime? ExpireOn { get; set; }

    /// <summary>
    /// Identifier of the parent task (for hierarchical relationships)
    /// </summary>
    public int? ParentTaskId { get; set; }

    /// <summary>
    /// Navigation property to the parent task
    /// </summary>
    public Tasks? ParentTask { get; set; } = null;

    /// <summary>
    /// Identifier of the worker who created the task
    /// </summary>
    public int CreatorId { get; set; }

    /// <summary>
    /// Navigation property to the worker who created the task
    /// </summary>
    public Workers Creator { get; set; } = null!;


    /// <summary>
    /// Identifier of the task type
    /// </summary>
    public int TaskTypeId { get; set; }

    /// <summary>
    /// Navigation property to the task type (e.g., "Задачи" or "Веха")
    /// </summary>
    public TaskType TaskType { get; set; } = null!;

    /// <summary>
    /// Identifier of the task status
    /// </summary>
    public int TaskStatusId { get; set; }

    /// <summary>
    /// Navigation property to the task status (e.g., "В ожидании", "В работе")
    /// </summary>
    public TaskStatus TaskStatus { get; set; } = null!;

    /// <summary>
    /// Identifier of the project the task belongs to
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Navigation property to the project
    /// </summary>
    public Projects Project { get; set; } = null!;

    /// <summary>
    /// Identifier of the sprint the task belongs to (optional)
    /// </summary>
    public int? SprintId { get; set; }

    /// <summary>
    /// Navigation property to the sprint
    /// </summary>
    public Sprints? Sprint { get; set; }

    /// <summary>
    /// Identifier of the task priority
    /// </summary>
    public int? TaskPriorityId { get; set; }

    /// <summary>
    /// Navigation property to the task priority (e.g., "Низкий", "Высокий")
    /// </summary>
    public TaskPriority TaskPriority { get; set; } = null!;

    /// <summary>
    /// List of workers observing the task
    /// </summary>
    public List<TaskObserver>? TaskObservers { get; set; }

    /// <summary>
    /// List of external links associated with the task
    /// </summary>
    public List<TaskLink>? TaskLinks { get; set; }

    /// <summary>
    /// List of messages related to the task
    /// </summary>
    public List<TaskMessage>? TaskMessages { get; set; }

    /// <summary>
    /// List of boards the task is associated with
    /// <remarks>
    /// A task must be on at least one board, as a project is considered a large organized board
    /// </remarks>
    /// </summary>
    public List<BoardTask> BoardTasks { get; set; } = new();

    /// <summary>
    /// List of tags associated with the task
    /// </summary>
    public List<Tags> Tags { get; set; } = [];

    /// <summary>
    /// List of executor assignments for the task, supporting multiple executors
    /// </summary>
    public List<TaskExecutor> TaskExecutors { get; set; } = [];

    /// <summary>
    /// List of outgoing task relationships (dependencies where this task is the source)
    /// </summary>
    public List<TaskRelationship> TaskRelationships { get; set; } = [];

    /// <summary>
    /// List of incoming task relationships (dependencies where this task is the target)
    /// </summary>
    public List<TaskRelationship> RelatedTaskRelationships { get; set; } = [];
}