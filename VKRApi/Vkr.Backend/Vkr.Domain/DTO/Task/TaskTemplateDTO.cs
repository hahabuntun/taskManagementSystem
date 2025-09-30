using Vkr.Domain.DTO.Tag;
using Vkr.Domain.Entities.Task;

namespace Vkr.Domain.DTO.Task;


public class TaskTemplateDTO
{
    public int Id { get; set; }

    public string TemplateName { get; set; } = string.Empty;

    public string? TaskName { get; set; }

    public string? Description { get; set; }

    public TaskStatusDTO? TaskStatus { get; set; }

    public TaskPriorityDTO? TaskPriority { get; set; }

    public TaskTypeDTO? TaskType { get; set; } // Navigation property

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? Progress { get; set; }

    public int? StoryPoints { get; set; }

    public List<FullTagDTO> Tags { get; set; } = new List<FullTagDTO>();

}