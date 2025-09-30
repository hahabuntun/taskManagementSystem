using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Board;

public class Boards
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? OwnerId { get; set; }
    public Workers? Owner { get; set; }
    public int? ProjectId { get; set; }
    public Projects? Project { get; set; }
    public DateTime CreatedOn { get; set; }
    public List<BoardTask> BoardTasks { get; set; } = new(); // Changed from TasksList to BoardTasks
    public int BoardTypeId { get; set; }
    public List<BoardColumns> BoardColumns { get; set; } = new();
    public BoardBasis Basis { get; set; } // Enum for board type
}

public enum BoardBasis
{
    Status,
    Priority,
    Deadline,
    Assignee,
    CustomColumns
}

public class BoardColumns
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public int Order { get; set; }
    public Boards Board { get; set; }
    public List<BoardTask> BoardTasks { get; set; } = new(); // Changed to reference BoardTask
}

public class BoardTask
{
    public int BoardId { get; set; }
    public int TaskId { get; set; }
    public string? CustomColumnName { get; set; } // For custom boards
    public Boards Board { get; set; }
    public Tasks Task { get; set; }
}