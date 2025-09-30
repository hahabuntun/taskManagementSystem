using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Interfaces.Services.BoardServices;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Domain.DTO.Board;
using Vkr.Domain.DTO.Task;

namespace Vkr.API.Controllers.BoardControllers;

[ApiController]
[Route("api/boards")]
public class BoardController : ControllerBase
{
    private readonly IBoardService _boardService;
    private readonly IWorkersService _workerService;

    public BoardController(IBoardService boardService, IWorkersService workerService)
    {
        _boardService = boardService;
        _workerService = workerService;
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<BoardSummaryDto>>> GetProjectBoards(int projectId)
    {
        var boards = await _boardService.GetProjectBoardsAsync(projectId);
        return Ok(boards);
    }

    [HttpPost("project/{projectId}")]
    public async Task<ActionResult<BoardSummaryDto>> AddProjectBoard(int projectId, [FromBody] BoardCreateDto data)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        var creator = await _workerService.GetWorkerByIdAsync(userId);
        if (creator == null)
            return NotFound($"Worker with ID {userId} not found.");

        var board = await _boardService.AddProjectBoardAsync(projectId, data, creator);
        return CreatedAtAction(nameof(GetBoard), new { boardId = board.Id }, board);
    }

    [HttpGet("worker/{workerId}/projects")]
    public async Task<ActionResult<IEnumerable<BoardSummaryDto>>> GetWorkerProjectBoards(int workerId)
    {
        var boards = await _boardService.GetWorkerProjectBoardsAsync(workerId);
        return Ok(boards);
    }

    [HttpGet("worker/{workerId}/personal")]
    public async Task<ActionResult<IEnumerable<BoardSummaryDto>>> GetWorkerPersonalBoards(int workerId)
    {
        var boards = await _boardService.GetWorkerPersonalBoardsAsync(workerId);
        return Ok(boards);
    }

    [HttpPost("worker/{workerId}")]
    public async Task<ActionResult<BoardSummaryDto>> AddWorkerBoard(int workerId, [FromBody] BoardCreateDto data)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        var board = await _boardService.AddWorkerBoardAsync(workerId, data, userId);
        return CreatedAtAction(nameof(GetBoard), new { boardId = board.Id }, board);
    }

    [HttpGet("{boardId}")]
    public async Task<ActionResult<BoardSummaryDto>> GetBoard(int boardId)
    {
        var board = await _boardService.GetBoardAsync(boardId);
        return Ok(board);
    }

    [HttpPut("{boardId}")]
    public async Task<ActionResult<BoardSummaryDto>> EditBoard(int boardId, [FromBody] BoardUpdateDto data)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        var board = await _boardService.EditBoardAsync(boardId, data, userId);
        return Ok(board);
    }

    [HttpDelete("{boardId}")]
    public async Task<ActionResult> RemoveBoard(int boardId)
    {
        await _boardService.RemoveBoardAsync(boardId);
        return NoContent();
    }

    [HttpGet("{boardId}/columns")]
    public async Task<ActionResult<IEnumerable<BoardColumnDto>>> GetBoardColumns(int boardId)
    {
        var columns = await _boardService.GetBoardColumnsAsync(boardId);
        return Ok(columns);
    }

    [HttpPost("{boardId}/columns")]
    public async Task<ActionResult<BoardColumnDto>> AddColumnToBoard(int boardId, [FromBody] BoardColumnCreateDto data)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        var column = await _boardService.AddColumnToBoardAsync(boardId, data, userId);
        return CreatedAtAction(nameof(GetBoardColumns), new { boardId }, column);
    }

    [HttpDelete("{boardId}/columns/{columnName}")]
    public async Task<ActionResult> RemoveColumnFromBoard(int boardId, string columnName)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        await _boardService.RemoveColumnFromBoardAsync(boardId, columnName, userId);
        return NoContent();
    }

    [HttpGet("{boardId}/tasks")]
    public async Task<ActionResult<IEnumerable<TaskDTO>>> GetBoardTasks(int boardId)
    {
        var tasks = await _boardService.GetBoardTasksAsync(boardId);
        return Ok(tasks);
    }

    [HttpGet("{boardId}/custom-tasks")]
    public async Task<ActionResult<IEnumerable<BoardTaskWithDetailsDto>>> GetCustomBoardTasks(int boardId)
    {
        var tasks = await _boardService.GetCustomBoardTasksAsync(boardId);
        return Ok(tasks);
    }

    [HttpGet("{boardId}/available-tasks/{workerId}")]
    public async Task<ActionResult<IEnumerable<TaskDTO>>> GetAvailableTasksForBoard(int boardId, int workerId)
    {
        var tasks = await _boardService.GetAvailableTasksForBoardAsync(boardId, workerId);
        return Ok(tasks);
    }

    [HttpPost("{boardId}/tasks")]
    public async Task<ActionResult<BoardTaskDto>> AddTaskToBoard(int boardId, [FromBody] AddTaskToBoardDto data)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        var boardTask = await _boardService.AddTaskToBoardAsync(boardId, data, userId);
        return CreatedAtAction(nameof(GetBoardTasks), new { boardId }, boardTask);
    }

    [HttpPut("{boardId}/tasks/{taskId}/column")]
    public async Task<ActionResult> ChangeTaskColumn(int boardId, int taskId, [FromBody] ChangeTaskColumnDto data)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        await _boardService.ChangeTaskColumnAsync(boardId, taskId, data.ColumnName, userId);
        return NoContent();
    }

    [HttpDelete("{boardId}/tasks/{taskId}")]
    public async Task<ActionResult> RemoveTaskFromBoard(int boardId, int taskId)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        await _boardService.RemoveTaskFromBoardAsync(boardId, taskId, userId);
        return NoContent();
    }

    [HttpDelete("{boardId}/clear")]
    public async Task<ActionResult> ClearBoard(int boardId)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        await _boardService.ClearBoardAsync(boardId, userId);
        return NoContent();
    }
}