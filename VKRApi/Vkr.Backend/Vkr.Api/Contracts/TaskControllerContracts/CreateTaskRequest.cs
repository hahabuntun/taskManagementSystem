using System.ComponentModel.DataAnnotations;
using Vkr.Domain.DTO;
using Vkr.Domain.DTO.Tag;

namespace Vkr.API.Contracts.TaskControllerContracts;

public class CreateTaskRequest
{
    /// <summary>
    /// Name of the task
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Progress of the task (0-100%)
    /// </summary>
    public int Progress { get; set; }

    /// <summary>
    /// Description of the task
    /// </summary>
    public string? Description { get; set; }
    public int? StoryPoints { get; set; }

    /// <summary>
    /// Start date of the task
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// End date (deadline) of the task
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// ID of the project the task belongs to
    /// </summary>
    [Required]
    public int ProjectId { get; set; }

    /// <summary>
    /// ID of the worker who created the task
    /// </summary>
    [Required]
    public int CreatorId { get; set; }


    /// <summary>
    /// ID of the task type
    /// </summary>
    [Required]
    public int TaskTypeId { get; set; }

    /// <summary>
    /// ID of the task status
    /// </summary>
    [Required]
    public int TaskStatusId { get; set; }

    /// <summary>
    /// ID of the task priority (optional)
    /// </summary>
    public int TaskPriorityId { get; set; }

    /// <summary>
    /// ID of the sprint the task belongs to (optional)
    /// </summary>
    public int? SprintId { get; set; }


    public int[] ExistingTagIds { get; set; } = Array.Empty<int>();

    /// <summary>
    /// New tags to create and associate with the task
    /// </summary>
    public TagDTO[] NewTags { get; set; } = Array.Empty<TagDTO>();

    public CreateLinkDTO[] Links { get; set; } = Array.Empty<CreateLinkDTO>();

}