namespace Vkr.Domain.Entities.CheckLists;

public class ChecklistItem
{
    public int    Id          { get; set; }
    public int    ChecklistId { get; set; }
    public string Title       { get; set; } = null!;
    public bool   IsCompleted { get; set; }

    public Checklist? Checklist { get; set; }
}