using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Task;

public class TaskExecutor
{
    /// <summary>
    /// Identifier of the task in the executor assignment
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Navigation property to the task
    /// </summary>
    public Tasks Task { get; set; } = null!;

    /// <summary>
    /// Identifier of the worker assigned as an executor
    /// </summary>
    public int WorkerId { get; set; }

    /// <summary>
    /// Navigation property to the worker
    /// </summary>
    public Workers Worker { get; set; } = null!;

    /// <summary>
    /// Indicates if this worker is the responsible executor for the task
    /// </summary>
    public bool IsResponsible { get; set; }
}