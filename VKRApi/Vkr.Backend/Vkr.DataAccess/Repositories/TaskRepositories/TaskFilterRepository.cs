using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories;
using Vkr.DataAccess;
using Vkr.Domain.Entities;

namespace Vkr.DataAccess.Repositories;

public class TaskFilterRepository : ITaskFilterRepository
{
    private readonly ApplicationDbContext _context;

    public TaskFilterRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskFilter>> GetTaskFiltersAsync()
    {
        return await _context.TaskFilters.ToListAsync();
    }

    public async Task<TaskFilter?> GetTaskFilterByNameAsync(string name)
    {
        return await _context.TaskFilters.FirstOrDefaultAsync(tf => tf.Name == name);
    }

    public async Task AddTaskFilterAsync(TaskFilter filter)
    {
        await _context.TaskFilters.AddAsync(filter);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTaskFilterAsync(TaskFilter filter)
    {
        _context.TaskFilters.Update(filter);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveTaskFilterAsync(string name)
    {
        var filter = await GetTaskFilterByNameAsync(name);
        if (filter != null)
        {
            _context.TaskFilters.Remove(filter);
            await _context.SaveChangesAsync();
        }
    }
}