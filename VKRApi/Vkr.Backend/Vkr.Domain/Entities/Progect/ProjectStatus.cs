namespace Vkr.Domain.Entities.Progect;

public class ProjectStatus
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public int RelatedColorId { get; set; }

    public ColorInfo RelatedColor { get; set; } = null!;
    
    public List<Projects>? ProjectsList { get; set; }
}