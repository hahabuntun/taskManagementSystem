using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces.Repositories.TaskRepositories;
public interface ITaskObserverRepository
{
    Task<IEnumerable<WorkerDTO>> GetObserversAsync(int taskId);
    Task<List<TaskObserver>> GetObserverEntitiesAsync(int taskId);
    Task AddObserverAsync(TaskObserver observer);
    Task AddObserversAsync(IEnumerable<TaskObserver> observers);
    Task RemoveObserverAsync(TaskObserver observer);
}