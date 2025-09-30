using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Task;

public class TaskLink
{
    public int Id { get; set; }
    
    /// <summary>
    /// Внешняя ссылка
    /// </summary>
    public string Link { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Когда создана
    /// </summary>
    public DateTime CreatedOn { get; set; }
    
    
    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public int TaskId { get; set; }
    
    /// <summary>
    /// Связь с задаче
    /// </summary>
    public Tasks Task { get; set; } = null!;
}