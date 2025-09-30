using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Worker;

public class TaskFilterDTO
{
    public string Name { get; set; } = string.Empty;
    public TaskFilterOptionsDTO Options { get; set; } = new TaskFilterOptionsDTO();
}

public class TaskFilterOptionsDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; } // Maps to TaskTypeEnum
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTill { get; set; }
    public DateTime? StartedFrom { get; set; }
    public DateTime? StartedTill { get; set; }
    public DateTime? EndDateFrom { get; set; }
    public DateTime? EndDateTill { get; set; }
    public TaskStatusDTO? Status { get; set; }
    public TaskPriorityDTO? Priority { get; set; }
    public WorkerDTO? Creator { get; set; }
    public List<FullTagDTO>? Tags { get; set; }
}