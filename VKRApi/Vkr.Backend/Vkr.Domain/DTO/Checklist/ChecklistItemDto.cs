namespace Vkr.Domain.DTO.Checklist;

public class ChecklistItemDto
{
    public int  Id          { get; set; }
    public int  ChecklistId { get; set; }
    public string Title     { get; set; } = null!;
    public bool IsCompleted { get; set; }
}