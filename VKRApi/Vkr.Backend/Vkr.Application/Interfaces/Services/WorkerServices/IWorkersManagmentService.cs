using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Application.Interfaces.Services.WorkerServices;

public interface IWorkersManagementService
{
    Task<bool> SetConnection(WorkersManagmentDTO request, int creatorId);
    Task<bool> DeleteConnection(int managerId, int subordinateId, int creatorId);
    Task<int> UpdateConnection(WorkersManagmentDTO request, int creatorId);
    Task<List<Workers>> GetSubordinates(int managerId);
    Task<List<Workers>> GetManagers(int subordinateId);
}