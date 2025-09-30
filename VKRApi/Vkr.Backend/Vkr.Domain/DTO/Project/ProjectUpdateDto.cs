using Vkr.Domain.DTO.Tag;

namespace Vkr.Domain.DTO.Project;

public class ProjectUpdateDto
{
    public string Name { get; set; }

    public string? Goal { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Progress { get; set; }
    public string? Description { get; set; }
    public int ProjectStatusId { get; set; }
    public List<int>? Members = null;
    /// <summary>
    /// IDs существующих тегов
    /// </summary>
    public List<int> ExistingTagIds { get; set; } = [];

    /// <summary>
    /// Новые теги (имя и цвет)
    /// </summary>
    public List<TagDTO> NewTags { get; set; } = [];
}