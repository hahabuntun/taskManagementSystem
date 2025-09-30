using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities.Board;

namespace Vkr.Domain.DTO.Board;

public class BoardCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? OwnerId { get; set; } // Nullable for personal boards
    public int? ProjectId { get; set; } // Nullable for project or personal boards
    public BoardBasis Basis { get; set; } // Enum to define board type
}