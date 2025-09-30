using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Domain.DTO.Worker;

namespace Vkr.Application.Interfaces;

public interface ITaskExecutorService
{
    Task<IEnumerable<WorkerDTO>> GetExecutorsAsync(int taskId);
    Task AddExecutorAsync(int taskId, int workerId, bool isResponsible, int creatorId);
    Task AddExecutorsAsync(int taskId, int[] workerIds, int? responsibleWorkerId, int creatorId);
    Task RemoveExecutorAsync(int taskId, int workerId, int creatorId);
    Task UpdateResponsibleExecutorAsync(int taskId, int? workerId, int creatorId);
}