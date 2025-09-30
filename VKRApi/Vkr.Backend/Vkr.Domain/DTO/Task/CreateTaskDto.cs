using Vkr.Domain.DTO.Tag;

namespace Vkr.Domain.DTO.Task;

public class CreateTaskDTO
{
    public string Name { get; set; } = string.Empty;
    public int Progress { get; set; }
    public int? StoryPoints { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int ProjectId { get; set; }
    public int CreatorId { get; set; }
    public int TaskTypeId { get; set; }
    public int TaskStatusId { get; set; }
    public int TaskPriorityId { get; set; }
    public int? SprintId { get; set; }
    public int[] ExistingTagIds { get; set; } = Array.Empty<int>();
    public TagDTO[] NewTags { get; set; } = Array.Empty<TagDTO>();

    public CreateLinkDTO[] Links { get; set; } = Array.Empty<CreateLinkDTO>();
}