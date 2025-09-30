using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.BoardRepositories;
using Vkr.DataAccess;
using Vkr.Domain.Entities.Board;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Repositories.BoardRepositories;

public class BoardRepository : IBoardRepository
{
    private readonly ApplicationDbContext _context;

    public BoardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Boards>> GetProjectBoardsAsync(int projectId)
    {
        return await _context.Boards
            .Where(b => b.ProjectId == projectId && b.OwnerId == null)
            .Include(b => b.BoardTasks)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Boards>> GetWorkerProjectBoardsAsync(int workerId)
    {
        return await _context.Boards
            .Where(b => b.ProjectId.HasValue && b.OwnerId == null && b.Project.WorkersList.Any(m => m.Id == workerId))
            .Include(b => b.BoardTasks)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Boards>> GetWorkerPersonalBoardsAsync(int workerId)
    {
        return await _context.Boards
            .Where(b => b.OwnerId == workerId && b.ProjectId == null)
            .Include(b => b.BoardTasks)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<Boards> GetBoardAsync(int boardId)
    {
        return await _context.Boards
            .Include(b => b.Owner)
            .Include(b => b.BoardColumns)
            .FirstOrDefaultAsync(b => b.Id == boardId);
    }

    public async Task AddBoardAsync(Boards board)
    {
        // Validate mutual exclusivity of OwnerId and ProjectId
        if (board.OwnerId.HasValue && board.ProjectId.HasValue)
            throw new InvalidOperationException("A board cannot have both an OwnerId and a ProjectId.");
        if (!board.OwnerId.HasValue && !board.ProjectId.HasValue)
            throw new InvalidOperationException("A board must have either an OwnerId or a ProjectId.");

        await _context.Boards.AddAsync(board);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBoardAsync(Boards board)
    {
        // Validate mutual exclusivity of OwnerId and ProjectId
        if (board.OwnerId.HasValue && board.ProjectId.HasValue)
            throw new InvalidOperationException("A board cannot have both an OwnerId and a ProjectId.");
        if (!board.OwnerId.HasValue && !board.ProjectId.HasValue)
            throw new InvalidOperationException("A board must have either an OwnerId or a ProjectId.");

        _context.Boards.Update(board);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveBoardAsync(int boardId)
    {
        var board = await GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Board with ID {boardId} not found");

        _context.Boards.Remove(board);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<BoardColumns>> GetBoardColumnsAsync(int boardId)
    {
        return await _context.BoardColumns
            .Where(bc => bc.BoardId == boardId)
            .OrderBy(bc => bc.Order)
            .ToListAsync();
    }

    public async Task<BoardColumns> GetBoardColumnAsync(int boardId, string columnName)
    {
        return await _context.BoardColumns
            .FirstOrDefaultAsync(bc => bc.BoardId == boardId && bc.Name == columnName);
    }

    public async Task AddColumnAsync(BoardColumns column)
    {
        await _context.BoardColumns.AddAsync(column);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveColumnAsync(int boardId, string columnName)
{
    var column = await GetBoardColumnAsync(boardId, columnName)
        ?? throw new KeyNotFoundException($"Column {columnName} not found on board {boardId}");

    // Update BoardTask entries to set CustomColumnName to null for the removed column
    var affectedBoardTasks = await _context.BoardTasks
        .Where(bt => bt.BoardId == boardId && bt.CustomColumnName == columnName)
        .ToListAsync();

    foreach (var boardTask in affectedBoardTasks)
    {
        boardTask.CustomColumnName = null;
        _context.BoardTasks.Update(boardTask);
    }

    // Remove the column
    _context.BoardColumns.Remove(column);
    await _context.SaveChangesAsync();
}

    public async Task<IEnumerable<Tasks>> GetBoardTasksAsync(int boardId)
    {
        return await _context.Tasks
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskType)
            .Include(t => t.TaskPriority)
            .Include(t => t.Project)
            .Include(t => t.Creator)
            .Include(t => t.Sprint)
            .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(t => t.TaskObservers).ThenInclude(to => to.Worker)
            .Include(t => t.Tags)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .Where(t => t.BoardTasks.Any(bt => bt.BoardId == boardId))
            .ToListAsync();
    }

    public async Task<IEnumerable<BoardTask>> GetCustomBoardTasksAsync(int boardId)
    {
        return await _context.BoardTasks
            .Where(bt => bt.BoardId == boardId)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.TaskStatus)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.TaskType)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.TaskPriority)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.Project)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.Creator)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.Sprint)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.TaskObservers).ThenInclude(to => to.Worker)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.Tags)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask)
            .Include(bt => bt.Task)
            .ThenInclude(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tasks>> GetAvailableTasksForBoardAsync(int boardId, int workerId)
    {
        var board = await GetBoardAsync(boardId)
            ?? throw new KeyNotFoundException($"Board with ID {boardId} not found");

        IQueryable<Tasks> query;
        if (board.ProjectId.HasValue)
        {
            // Project board: allow any task from the same project that isn't already on the board
            query = _context.Tasks
                .Where(t => t.ProjectId == board.ProjectId);
        }
        else
        {
            // Personal board: only tasks where the worker is the creator, an executor, an observer, or the project manager
            query = _context.Tasks
                .Where(t =>
                    t.CreatorId == workerId || // Worker is the creator
                    t.TaskExecutors.Any(te => te.WorkerId == workerId) || // Worker is an executor
                    t.TaskObservers.Any(to => to.WorkerId == workerId) || // Worker is an observer
                    t.Project.ManagerId == workerId // Worker is the project manager
                );
        }

        query = query
            .Where(t => !t.BoardTasks.Any(bt => bt.BoardId == boardId)) // Exclude tasks already on the board
            .Distinct() // Ensure unique tasks
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskType)
            .Include(t => t.TaskPriority)
            .Include(t => t.Project)
            .Include(t => t.Creator)
            .Include(t => t.Sprint)
            .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(t => t.TaskObservers).ThenInclude(to => to.Worker)
            .Include(t => t.Tags)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .OrderBy(t => t.CreatedOn);

        return await query.ToListAsync();
    }

    public async Task<BoardTask> GetBoardTaskAsync(int boardId, int taskId)
    {
        return await _context.BoardTasks
            .FirstOrDefaultAsync(bt => bt.BoardId == boardId && bt.TaskId == taskId);
    }

    public async Task AddTaskToBoardAsync(BoardTask boardTask)
    {
        if (await _context.BoardTasks.AnyAsync(bt => bt.BoardId == boardTask.BoardId && bt.TaskId == boardTask.TaskId))
            throw new InvalidOperationException($"Task {boardTask.TaskId} is already on board {boardTask.BoardId}");

        await _context.BoardTasks.AddAsync(boardTask);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBoardTaskAsync(BoardTask boardTask)
    {
        _context.BoardTasks.Update(boardTask);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveTaskFromBoardAsync(int boardId, int taskId)
    {
        var boardTask = await GetBoardTaskAsync(boardId, taskId)
            ?? throw new KeyNotFoundException($"Task {taskId} not found on board {boardId}");

        _context.BoardTasks.Remove(boardTask);
        await _context.SaveChangesAsync();
    }

    public async Task ClearBoardAsync(int boardId)
    {
        var boardTasks = await _context.BoardTasks
            .Where(bt => bt.BoardId == boardId)
            .ToListAsync();

        _context.BoardTasks.RemoveRange(boardTasks);
        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<int, string>> GetRelationshipTypesAsync()
    {
        return await _context.TaskRelationshipTypes
            .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);
    }
}