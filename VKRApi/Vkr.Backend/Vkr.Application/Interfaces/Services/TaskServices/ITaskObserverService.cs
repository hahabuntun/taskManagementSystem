using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Domain.DTO.Worker;

namespace Vkr.Application.Interfaces;

public interface ITaskObserverService
{
    Task<IEnumerable<WorkerDTO>> GetObserversAsync(int taskId);
    Task AddObserverAsync(int taskId, int workerId, int creatorId);
    Task AddObserversAsync(int taskId, int[] workerIds, int creatorId);
    Task RemoveObserverAsync(int taskId, int workerId, int creatorId);
}