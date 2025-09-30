using Vkr.Domain.Enums.CheckLists;

namespace Vkr.Domain.Entities.CheckLists;

public class Checklist
{
    public int                  Id         { get; set; }
    public ChecklistOwnerType  OwnerType  { get; set; }
    public int                  OwnerId    { get; set; }
    public string               Title      { get; set; } = null!;
    public ICollection<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();
}