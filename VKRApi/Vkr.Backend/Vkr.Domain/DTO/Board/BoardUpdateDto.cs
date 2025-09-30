using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities.Board;

namespace Vkr.Domain.DTO.Board;

public class BoardUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}