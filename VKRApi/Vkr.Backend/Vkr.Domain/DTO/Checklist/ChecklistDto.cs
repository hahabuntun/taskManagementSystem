using Vkr.Domain.Enums.CheckLists;

namespace Vkr.Domain.DTO.Checklist;

public class ChecklistDto
{
    public int Id                           { get; set; }
    public ChecklistOwnerType OwnerType    { get; set; }
    public int OwnerId                     { get; set; }
    public string Title                    { get; set; } = null!;
    public List<ChecklistItemDto> Items    { get; set; } = new();
}