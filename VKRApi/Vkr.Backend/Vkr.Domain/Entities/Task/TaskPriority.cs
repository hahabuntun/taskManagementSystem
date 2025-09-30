namespace Vkr.Domain.Entities.Task;

public class TaskPriority
{
    public int Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }

    public string Color { get; set; }
    public List<Tasks> TasksList { get; set; }
    public List<TaskTemplates> TaskTemplates { get; set; } = new List<TaskTemplates>();
}