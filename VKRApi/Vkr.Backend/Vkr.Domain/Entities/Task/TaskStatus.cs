namespace Vkr.Domain.Entities.Task;

public class TaskStatus
{
    /// <summary>
    /// Unique identifier for the task status
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the task status (e.g., "В ожидании", "В работе")
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Color associated with the status for UI representation
    /// </summary>
    public string Color { get; set; } = null!;

    /// <summary>
    /// List of tasks associated with this status
    /// </summary>
    public List<Tasks> TasksList { get; set; } = [];
    public List<TaskTemplates> TaskTemplates { get; set; } = new List<TaskTemplates>();
}