using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Task;

public class TaskObserver
{
    /// <summary>
    /// Identifier of the task in the observer assignment
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Navigation property to the task
    /// </summary>
    public Tasks Task { get; set; } = null!;

    /// <summary>
    /// Identifier of the worker assigned as an observer
    /// </summary>
    public int WorkerId { get; set; }

    /// <summary>
    /// Navigation property to the worker
    /// </summary>
    public Workers Worker { get; set; } = null!;

    /// <summary>
    /// Date and time when the worker was assigned as an observer
    /// </summary>
    public DateTime AssignedOn { get; set; }
}