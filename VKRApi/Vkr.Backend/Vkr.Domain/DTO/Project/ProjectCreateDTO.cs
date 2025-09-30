
using Vkr.Domain.DTO.Tag;

namespace Vkr.Domain.DTO.Project;

public class ProjectCreateDTO
{
    /// <summary>
    /// Название 
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; }

    public string? Goal { get; set; }

    public int Progress { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    /// <summary>
    /// IDs существующих тегов
    /// </summary>
    public List<int> ExistingTagIds { get; set; } = [];

    /// <summary>
    /// Новые теги (имя и цвет)
    /// </summary>
    public List<TagDTO> NewTags { get; set; } = [];
    /// <summary>
    /// Идентификатор управляющего
    /// </summary>
    public int ManagerId { get; set; }
    
    /// <summary>
    /// Иденификатор организации в которой работает
    /// </summary>
    public int OrganizationId { get; set; }
    
    public int? ProjectStatusId { get; set; }
    
    public List<int> Members { get; set; }
}