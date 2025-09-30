namespace Vkr.Domain.Entities.Sprint;

public class SprintStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public List<Sprints> SprintsList { get; set; } = new();
}