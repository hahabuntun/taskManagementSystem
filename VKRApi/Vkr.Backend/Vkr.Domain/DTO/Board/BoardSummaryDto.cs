using Vkr.Domain.Entities.Board;

namespace Vkr.Domain.DTO.Board;

public class BoardSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OwnerId { get; set; } // Nullable for personal boards
    public int? ProjectId { get; set; } // Nullable for project or personal boards
    public int TaskCount { get; set; }
    public DateTime CreatedOn { get; set; }
    public BoardBasis Basis { get; set; } // Enum to define board type
}