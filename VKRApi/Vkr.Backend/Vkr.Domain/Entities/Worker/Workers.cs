using Vkr.Domain.Entities.Board;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Task;
using File = Vkr.Domain.Entities.Files.File;

namespace Vkr.Domain.Entities.Worker;

public class Workers
{
    /// <summary>
    /// Maximum length for string properties
    /// </summary>
    public const int PropertyMaxLength = 50;

    /// <summary>
    /// Unique identifier for the worker
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// First name of the worker
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Last name of the worker
    /// </summary>
    public string SecondName { get; set; } = null!;

    /// <summary>
    /// Middle name (patronymic) of the worker (optional)
    /// </summary>
    public string? ThirdName { get; set; }

    /// <summary>
    /// Email address of the worker
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Normalized email address for consistent lookups
    /// </summary>
    public string NormalizedEmail { get; set; } = null!;

    /// <summary>
    /// Phone number of the worker (optional)
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Hashed password for the worker
    /// </summary>
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// Indicates if the worker can manage other workers
    /// </summary>
    public bool CanManageWorkers { get; set; }

    /// <summary>
    /// Indicates if the worker can manage projects
    /// </summary>
    public bool CanManageProjects { get; set; }

    /// <summary>
    /// Date and time when the worker was created
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Identifier of the worker's position
    /// </summary>
    public int WorkerPositionId { get; set; }

    /// <summary>
    /// Navigation property to the worker's position
    /// </summary>
    public WorkerPosition WorkerPosition { get; set; } = null!;

    /// <summary>
    /// Status of the worker (e.g., Active, Inactive)
    /// </summary>
    public WorkerStatus WorkerStatus { get; set; } = WorkerStatus.Active;

    /// <summary>
    /// Files uploaded by the worker for the organization
    /// </summary>
    public ICollection<File> WorkerFiles { get; set; } = [];

    /// <summary>
    /// Workers managed by this worker (subordinates)
    /// </summary>
    public List<WorkersManagement> SelfSubmissions { get; set; } = [];

    /// <summary>
    /// Managers of this worker
    /// </summary>
    public List<WorkersManagement> SelfManager { get; set; } = [];

    /// <summary>
    /// Projects associated with the worker
    /// </summary>
    public List<Projects> ProjectsList { get; set; } = [];

    /// <summary>
    /// Tasks this worker is observing
    /// </summary>
    public List<TaskObserver> TaskObservers { get; set; } = [];

    /// <summary>
    /// Tasks created by this worker
    /// </summary>
    public List<Tasks>? CreatorTasks { get; set; }

    /// <summary>
    /// Tasks where this worker is an executor
    /// </summary>
    public List<TaskExecutor> TaskExecutors { get; set; } = [];


    /// <summary>
    /// Checklists created by this worker for projects
    /// </summary>
    public List<ProjectChecklist>? ProjectChecklists { get; set; }

    /// <summary>
    /// Messages written by this worker
    /// </summary>
    public List<TaskMessage>? WorkerMessages { get; set; }

    /// <summary>
    /// Boards associated with this worker
    /// </summary>
    public List<Boards>? WorkerBoards { get; set; }
}