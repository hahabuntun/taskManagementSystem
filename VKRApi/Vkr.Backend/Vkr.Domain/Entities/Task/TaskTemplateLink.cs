using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Task;

public class TaskTemplateLink
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
    /// Идентификатор шаблоны
    /// </summary>
    public int TaskTemplateId { get; set; }
    
    /// <summary>
    /// Связь с шаблоном
    /// </summary>
    public TaskTemplates TaskTemplate { get; set; } = null!;
}