namespace Vkr.Domain.DTO.Task;

public class TaskMessageCreateDTO
{
    public string MessageText { get; set; }
    public int SenderId { get; set; }
    public int RelatedTaskId { get; set; }
}