using Vkr.Domain.Entities.Board;
using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces.Repositories.BoardRepositories;


public interface IBoardRepository
{
    Task<IEnumerable<Boards>> GetProjectBoardsAsync(int projectId);
    Task<IEnumerable<Boards>> GetWorkerProjectBoardsAsync(int workerId);
    Task<IEnumerable<Boards>> GetWorkerPersonalBoardsAsync(int workerId);
    Task<Boards> GetBoardAsync(int boardId);
    Task AddBoardAsync(Boards board);
    Task UpdateBoardAsync(Boards board);
    Task RemoveBoardAsync(int boardId);
    Task<IEnumerable<BoardColumns>> GetBoardColumnsAsync(int boardId);
    Task<BoardColumns> GetBoardColumnAsync(int boardId, string columnName);
    Task AddColumnAsync(BoardColumns column);
    Task RemoveColumnAsync(int boardId, string columnName);
    Task<IEnumerable<Tasks>> GetBoardTasksAsync(int boardId);
    Task<IEnumerable<BoardTask>> GetCustomBoardTasksAsync(int boardId);
    Task<IEnumerable<Tasks>> GetAvailableTasksForBoardAsync(int boardId, int workerId);
    Task<BoardTask> GetBoardTaskAsync(int boardId, int taskId);
    Task AddTaskToBoardAsync(BoardTask boardTask);
    Task UpdateBoardTaskAsync(BoardTask boardTask);
    Task RemoveTaskFromBoardAsync(int boardId, int taskId);
    Task ClearBoardAsync(int boardId);
    Task<Dictionary<int, string>> GetRelationshipTypesAsync();
}