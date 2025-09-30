namespace Vkr.Domain.DTO.Journal;

/// <summary>
/// Параметры запроса для постраничного вывода и фильтрации аудита
/// </summary>
public class AuditQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize   { get; set; } = 20;

    /// <summary>
    /// Фильтр по сущности, например: "Sprints" или "Tasks"
    /// </summary>
    public string? EntityName { get; set; }

    /// <summary>
    /// Фильтр по ключу сущности (Id в виде строки)
    /// </summary>
    public string? EntityKey  { get; set; }

    public DateTime? From     { get; set; }
    public DateTime? To       { get; set; }
    public string?  ChangeType { get; set; }
    public string?  ChangedBy { get; set; }
}