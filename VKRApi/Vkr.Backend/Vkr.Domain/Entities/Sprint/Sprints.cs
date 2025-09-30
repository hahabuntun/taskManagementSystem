using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Task;

namespace Vkr.Domain.Entities.Sprint;

public class Sprints
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Goal { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? StartsOn { get; set; }
    public DateTime? ExpireOn { get; set; }
    public int ProjectId { get; set; }
    public Projects Project { get; set; } = null!;
    public int SprintStatusId { get; set; }
    public SprintStatus SprintStatus { get; set; } = null!;
    public List<Tasks>? TasksList { get; set; }
}