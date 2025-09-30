using Vkr.Domain.Entities.Worker;
namespace Vkr.Domain.Entities.Progect;

public class ProjectLink
{
    /// <summary>
    /// Идентификатор проектной ссылки
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Проектная ссылка
    /// </summary>
    public string Link { get; set; }
    
    /// <summary>
    /// Описание ссылки
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Дата создания ссылки
    /// </summary>
    public DateTime CreatedOn { get; set; }
    

    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Навигационное поле проекта
    /// </summary>
    public Projects Projects { get; set; } = null!;
}