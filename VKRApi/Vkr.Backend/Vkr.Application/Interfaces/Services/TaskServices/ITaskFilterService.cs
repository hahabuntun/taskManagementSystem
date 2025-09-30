using Vkr.Domain.DTO.Task;

namespace Vkr.Application.Interfaces.Services;

public interface ITaskFilterService
{
    Task<IEnumerable<TaskFilterDTO>> GetTaskFiltersAsync();
    Task AddTaskFilterAsync(string name, TaskFilterOptionsDTO options);
    Task EditTaskFilterAsync(string name, TaskFilterOptionsDTO options);
    Task RemoveTaskFilterAsync(string name);
}