namespace Vkr.Domain.Entities.Task;

public class TaskRelationship
{
    /// <summary>
    /// Unique identifier for the task relationship
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifier of the source task in the relationship
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Identifier of the target task in the relationship
    /// </summary>
    public int RelatedTaskId { get; set; }

    /// <summary>
    /// Identifier of the relationship type
    /// </summary>
    public int TaskRelationshipTypeId { get; set; }

    /// <summary>
    /// Navigation property to the relationship type (e.g., FS, FF, SS, SF)
    /// </summary>
    public TaskRelationshipType RelationshipType { get; set; } = null!;

    /// <summary>
    /// Navigation property to the source task
    /// </summary>
    public Tasks Task { get; set; } = null!;

    /// <summary>
    /// Navigation property to the target task
    /// </summary>
    public Tasks RelatedTask { get; set; } = null!;
}