using System.Collections.Generic;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Task;

namespace Vkr.Domain.Entities;

public class Tags
{
    public int Id { get; set; }

    /// <summary>
    /// Название тега
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Цвет тега (например, hex-код)
    /// </summary>
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Список проектов, связанных с тегом
    /// </summary>
    public List<Projects> Projects { get; set; } = new List<Projects>();

    /// <summary>
    /// Список задач, связанных с тегом
    /// </summary>
    public List<Tasks> Tasks { get; set; } = new List<Tasks>();

    /// <summary>
    /// Список шаблонов задач, связанных с тегом
    /// </summary>
    public List<TaskTemplates> TaskTemplates { get; set; } = new List<TaskTemplates>();
}