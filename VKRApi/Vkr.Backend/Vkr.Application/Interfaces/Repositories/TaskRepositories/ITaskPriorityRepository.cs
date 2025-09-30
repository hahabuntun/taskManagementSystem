using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces.Repositories.TaskRepositories;

public interface ITaskPriorityRepository 
{
    Task<TaskPriority> GetByNameAsync(string name);
}