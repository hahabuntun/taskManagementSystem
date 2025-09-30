using Vkr.Domain.DTO.Board;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Application.Interfaces.Services.BoardServices;

public interface IBoardService
{
    Task<IEnumerable<BoardSummaryDto>> GetProjectBoardsAsync(int projectId);
    Task<BoardSummaryDto> AddProjectBoardAsync(int projectId, BoardCreateDto data, Workers creator);
    Task<IEnumerable<BoardSummaryDto>> GetWorkerProjectBoardsAsync(int workerId);
    Task<IEnumerable<BoardSummaryDto>> GetWorkerPersonalBoardsAsync(int workerId);
    Task<BoardSummaryDto> AddWorkerBoardAsync(int workerId, BoardCreateDto data, int creatorId); // Add creatorId
    Task<BoardSummaryDto> GetBoardAsync(int boardId);
    Task<BoardSummaryDto> EditBoardAsync(int boardId, BoardUpdateDto data, int creatorId); // Add creatorId
    Task RemoveBoardAsync(int boardId);
    Task<IEnumerable<BoardColumnDto>> GetBoardColumnsAsync(int boardId);
    Task<BoardColumnDto> AddColumnToBoardAsync(int boardId, BoardColumnCreateDto data, int creatorId);
    Task RemoveColumnFromBoardAsync(int boardId, string columnName, int creatorId);
    Task<IEnumerable<TaskDTO>> GetBoardTasksAsync(int boardId);
    Task<IEnumerable<BoardTaskWithDetailsDto>> GetCustomBoardTasksAsync(int boardId);
    Task<IEnumerable<TaskDTO>> GetAvailableTasksForBoardAsync(int boardId, int workerId);
    Task<BoardTaskDto> AddTaskToBoardAsync(int boardId, AddTaskToBoardDto data, int creatorId);
    Task ChangeTaskColumnAsync(int boardId, int taskId, string? columnName, int creatorId);
    Task RemoveTaskFromBoardAsync(int boardId, int taskId, int creatorId);
    Task ClearBoardAsync(int boardId, int creatorId);
}