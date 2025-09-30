using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.History;

public record HistoryDTO
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public int RelatedEntityId { get; init; }
    public HistoryEntityType RelatedEntityType { get; init; }
    public DateTime CreatedOn { get; init; }
    public int CreatorId { get; init; }
    public SimpleWorkerDTO Creator { get; init; }
}


public record CreateHistoryDTO
{
    public string Text { get; init; } = string.Empty;
    public int RelatedEntityId { get; init; }
    public HistoryEntityType RelatedEntityType { get; init; }
    public int CreatorId { get; init; }
}
