using Vkr.Domain.Entities;

namespace Vkr.Application.Interfaces.Repositories;

public interface ITaskFilterRepository
{
    Task<IEnumerable<TaskFilter>> GetTaskFiltersAsync();
    Task<TaskFilter?> GetTaskFilterByNameAsync(string name);
    Task AddTaskFilterAsync(TaskFilter filter);
    Task UpdateTaskFilterAsync(TaskFilter filter);
    Task RemoveTaskFilterAsync(string name);
}