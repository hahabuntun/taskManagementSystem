using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Progect;

public class ProjectChecklist
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Когда создан
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Идентификатор создателя
    /// </summary>
    public int WorkerId { get; set; }

    /// <summary>
    /// Навигационное свойство создателя
    /// </summary>
    public Workers Creator { get; set; }  = null!;

    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Навигационое свойство проекта
    /// </summary>
    public Projects Project { get; set; } = null!;

    /// <summary>
    /// Проверки списка
    /// </summary>
    public List<ProjectChecklistCheck> Checks { get; set; }  = null!;
}