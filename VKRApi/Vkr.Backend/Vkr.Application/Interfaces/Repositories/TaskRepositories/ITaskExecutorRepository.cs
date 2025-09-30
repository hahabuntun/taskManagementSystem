using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces.Repositories.TaskExecutorRepositories;

public interface ITaskExecutorRepository
{
    Task<IEnumerable<WorkerDTO>> GetExecutorsAsync(int taskId);
    Task<List<TaskExecutor>> GetExecutorEntitiesAsync(int taskId);
    Task AddExecutorAsync(TaskExecutor executor);
    Task AddExecutorsAsync(IEnumerable<TaskExecutor> executors);
    Task UpdateExecutorAsync(TaskExecutor executor);
    Task RemoveExecutorAsync(TaskExecutor executor);
    Task<int> CountResponsibleExecutorsAsync(int taskId);
}