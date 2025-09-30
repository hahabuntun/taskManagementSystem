using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Application.Interfaces.Repositories.WorkerRepositories;

public interface IWorkerPositionsRepository
{
    Task<bool> DeleteWorkerPositionById(int id);
    Task<List<WorkerPositionDto>> GetWorkerPositions();
    Task<WorkerPositionDto?> GetWorkerPositionById(int id);
    Task<WorkerPositionDto?> UpdateWorkerPositions(int id, WorkerPosition workerPosition, int[] taskGiverIds, int[] taskTakerIds);
    Task<WorkerPositionDto> CreateWorkerPosition(WorkerPosition workerPosition, int[]? taskGiverIds = null, int[]? taskTakerIds = null);
    Task<bool> IsWorkerPositionExists(int id);
}