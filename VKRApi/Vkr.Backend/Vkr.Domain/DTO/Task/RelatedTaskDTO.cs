namespace Vkr.Domain.DTO.Task;

public class RelatedTaskDTO
{
    public TaskDTO Task { get; set; } = null!;
    public string RelationshipType { get; set; } = null!; // e.g., "FinishToStart", "FinishToFinish", "StartToStart", "StartToFinish", "ParentChild"
    public int? RelationshipId { get; set; }
}