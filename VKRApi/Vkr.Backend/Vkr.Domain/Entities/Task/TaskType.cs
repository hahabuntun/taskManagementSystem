namespace Vkr.Domain.Entities.Task;

public class TaskType
{
    /// <summary>
    /// Unique identifier for the task type
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the task type (e.g., "Задачи", "Веха")
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// List of tasks associated with this type
    /// </summary>
    public List<Tasks>? TasksList { get; set; }
}