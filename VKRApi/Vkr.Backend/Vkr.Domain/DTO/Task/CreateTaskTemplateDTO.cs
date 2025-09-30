using Vkr.Domain.Entities;

namespace Vkr.Domain.DTO.Task;

public class CreateTaskTemplateDTO
{
    public string TemplateName { get; set; } = string.Empty;

    public string? TaskName { get; set; }

    public string? Description { get; set; }

    public int? TaskStatusId { get; set; }

    public int? TaskPriorityId { get; set; }

    public int? TaskTypeId { get; set; } // Foreign key for TaskType

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? Progress { get; set; }

    public int? StoryPoints { get; set; }
    public List<CreateLinkDTO> Links { get; set; }
    public List<int> TagIds { get; set; }
    
}