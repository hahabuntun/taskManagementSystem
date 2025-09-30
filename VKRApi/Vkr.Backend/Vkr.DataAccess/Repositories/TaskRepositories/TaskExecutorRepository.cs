using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.TaskExecutorRepositories;
using Vkr.DataAccess;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Repositories.TaskRepositories;

public class TaskExecutorRepository : ITaskExecutorRepository
{
    private readonly ApplicationDbContext _context;

    public TaskExecutorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkerDTO>> GetExecutorsAsync(int taskId)
    {
        return await _context.TaskExecutors
            .Where(te => te.TaskId == taskId)
            .Include(te => te.Worker)
            .Select(te => new WorkerDTO
            {
                Id = te.Worker.Id,
                Name = te.Worker.Name,
                SecondName = te.Worker.SecondName,
                Email = te.Worker.Email,
                CreatedOn = te.Worker.CreatedOn,
                WorkerPositionId = te.Worker.WorkerPositionId,
                CanManageWorkers = te.Worker.CanManageWorkers,
                CanManageProjects = te.Worker.CanManageProjects
            })
            .ToListAsync();
    }

    public async Task<List<TaskExecutor>> GetExecutorEntitiesAsync(int taskId)
    {
        return await _context.TaskExecutors
            .Where(te => te.TaskId == taskId)
            .ToListAsync();
    }

    public async Task AddExecutorAsync(TaskExecutor executor)
    {
        _context.TaskExecutors.Add(executor);
        await _context.SaveChangesAsync();
    }

    public async Task AddExecutorsAsync(IEnumerable<TaskExecutor> executors)
    {
        _context.TaskExecutors.AddRange(executors);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExecutorAsync(TaskExecutor executor)
    {
        _context.TaskExecutors.Update(executor);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveExecutorAsync(TaskExecutor executor)
    {
        _context.TaskExecutors.Remove(executor);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountResponsibleExecutorsAsync(int taskId)
    {
        return await _context.TaskExecutors
            .CountAsync(te => te.TaskId == taskId && te.IsResponsible);
    }
}