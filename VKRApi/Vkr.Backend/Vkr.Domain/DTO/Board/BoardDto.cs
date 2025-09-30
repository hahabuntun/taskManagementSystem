using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities.Board;

namespace Vkr.Domain.DTO.Board;

public class BoardDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? OwnerId { get; set; } // Nullable for personal boards
    public int? ProjectId { get; set; } // Nullable for project or personal boards
    public DateTime CreatedOn { get; set; }
    public List<TaskDTO>? Tasks { get; set; } // Use TaskDTO instead of TaskIds
    public BoardBasis Basis { get; set; } // Enum to define board type
}


public class BoardColumnCreateDto
{
    public string Name { get; set; } = string.Empty;
}

public class BoardColumnDto
{
    public string Name { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public int Order { get; set; }
}

public class BoardTaskDto
{
    public int BoardId { get; set; }
    public int TaskId { get; set; }
    public string? CustomColumnName { get; set; }
}

public class BoardTaskWithDetailsDto : BoardTaskDto
{
    public TaskDTO Task { get; set; }
}

public class AddTaskToBoardDto
{
    public int TaskId { get; set; }
    public string? CustomColumnName { get; set; }
}

public class ChangeTaskColumnDto
{
    public string? ColumnName { get; set; }
}