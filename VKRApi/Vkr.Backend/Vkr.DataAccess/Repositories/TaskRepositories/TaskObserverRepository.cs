using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.DataAccess;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Task;


namespace Vkr.DataAccess.Repositories.TaskRepositories;

public class TaskObserverRepository : ITaskObserverRepository
{
    private readonly ApplicationDbContext _context;

    public TaskObserverRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkerDTO>> GetObserversAsync(int taskId)
    {
        return await _context.TaskObservers
            .Where(to => to.TaskId == taskId)
            .Include(to => to.Worker)
            .Select(to => new WorkerDTO
            {
                Id = to.Worker.Id,
                Name = to.Worker.Name,
                SecondName = to.Worker.SecondName,
                Email = to.Worker.Email,
                CreatedOn = to.Worker.CreatedOn,
                WorkerPositionId = to.Worker.WorkerPositionId,
                CanManageWorkers = to.Worker.CanManageWorkers,
                CanManageProjects = to.Worker.CanManageProjects
            })
            .ToListAsync();
    }

    public async Task<List<TaskObserver>> GetObserverEntitiesAsync(int taskId)
    {
        return await _context.TaskObservers
            .Where(to => to.TaskId == taskId)
            .ToListAsync();
    }

    public async Task AddObserverAsync(TaskObserver observer)
    {
        _context.TaskObservers.Add(observer);
        await _context.SaveChangesAsync();
    }

    public async Task AddObserversAsync(IEnumerable<TaskObserver> observers)
    {
        _context.TaskObservers.AddRange(observers);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveObserverAsync(TaskObserver observer)
    {
        _context.TaskObservers.Remove(observer);
        await _context.SaveChangesAsync();
    }
}