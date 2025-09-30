using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Worker;
using Task = System.Threading.Tasks.Task;

namespace Vkr.Application.Interfaces.Services.WorkerServices;

public interface IWorkerPositionsService
{
    Task<List<WorkerPositionDto>> GetAllWorkerPositionsAsync();
    Task<WorkerPositionDto?> GetWorkerPositionsByIdAsync(int positionId);
    Task<WorkerPositionDto?> UpdateWorkerPositionAsync(int id, WorkerPosition workerPosition, int[] taskGiverIds, int[] taskTakerIds, int creatorId);
    Task<bool> DeleteWorkerPositionAsync(int id, int creatorId);
    Task<WorkerPositionDto> CreateWorkerPosition(WorkerPosition workerPosition, int[]? taskGiverIds, int[]? taskTakerIds, int creatorId);
    Task<bool> IsWorkerPositionExists(int id);
}