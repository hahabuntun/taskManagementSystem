namespace Vkr.API.Contracts.ChecklistsConrtacts;

public class UpdateChecklistItemRequest
{
    /// <summary>Новый заголовок пункта</summary>
    public string? Title { get; set; }

    /// <summary>Установить/снять признак выполнения</summary>
    public bool? IsCompleted { get; set; }
}