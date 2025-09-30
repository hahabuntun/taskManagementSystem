namespace Vkr.Domain.DTO.Sprint;

public class SprintCreateDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Goal { get; set; }
    public DateTime? StartsOn { get; set; }
    public DateTime? ExpireOn { get; set; }
    public int ProjectId { get; set; }
    public int SprintStatusId { get; set; }
}