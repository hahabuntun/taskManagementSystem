using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities.Sprint;

namespace Vkr.Domain.DTO.Sprint;

public class SprintDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Goal { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? StartsOn { get; set; }
    public DateTime? ExpireOn { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public int SprintStatusId { get; set; }
    public SprintStatusDTO SprintStatus { get; set; } = null!;
    public List<TaskDTO> Tasks { get; set; } = new();
}