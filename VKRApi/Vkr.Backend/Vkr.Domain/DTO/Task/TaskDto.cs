using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Worker;

namespace Vkr.Domain.DTO.Task;

public class TaskDTO
{
    public int Id { get; set; }
    public string ShortName { get; set; } = string.Empty;
    public int? StoryPoints { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedOn { get; set; }
    public int Progress { get; set; }
    public DateTime? StartOn { get; set; }
    public DateTime? ExpireOn { get; set; }
    public ProjectDTO Project { get; set; }
    public WorkerDTO Creator { get; set; }
    public TaskTypeDTO TaskType { get; set; }
    public TaskStatusDTO TaskStatus { get; set; }
    public TaskPriorityDTO TaskPriority { get; set; }
    public SprintDTO Sprint { get; set; }
    public List<RelatedTaskDTO> RelatedTasks { get; set; }
    public List<FullTagDTO> TagDTOs { get; set; }
    public List<WorkerDTO> Executors { get; set; }
    public List<WorkerDTO> Observers { get; set; }
    public WorkerDTO ResponsibleWorker { get; set; }
}