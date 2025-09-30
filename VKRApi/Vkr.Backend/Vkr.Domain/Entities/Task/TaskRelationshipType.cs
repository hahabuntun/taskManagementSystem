namespace Vkr.Domain.Entities.Task;

public class TaskRelationshipType
{
    /// <summary>
    /// Unique identifier for the task relationship type
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the relationship type (e.g., "FinishToStart", "FinishToFinish")
    /// </summary>
    public string Name { get; set; } = null!;
}