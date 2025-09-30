
using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.History;


public enum HistoryEntityType
{
    Task = 0,
    Project = 1,
    Sprint = 2,
    Board = 3,
    Organization = 4,
    Worker = 5,
}

public class History
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;

    public HistoryEntityType RelatedEntityType { get; set; }
    public int RelatedEntityId { get; set; }

    public Workers Creator { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public int CreatorId { get; set; }
    
}