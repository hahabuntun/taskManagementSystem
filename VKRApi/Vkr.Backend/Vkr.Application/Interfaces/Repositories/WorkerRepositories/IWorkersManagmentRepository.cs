using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Application.Interfaces.Repositories.WorkerRepositories;

public interface  IWorkersManagmentRepository
{
    /// <summary>
    /// Установить связь
    /// </summary>
    /// <param name="request">Сущность связи между сотрудниками</param>
    Task<bool> SetConnection(WorkersManagmentDTO request);

    /// <summary>
    /// Удалить связь
    /// </summary>
    Task<bool> DeleteConnection(int managerId, int subordinateId);
    
    /// <summary>
    /// Обновить связь
    /// </summary>
    /// <param name="request">Сущность связи между сотрудниками</param>
    Task<int> UpdateConnection(WorkersManagmentDTO request);

    /// <summary>
    /// Получить подчинённых 
    /// </summary>
    Task<List<Workers>> GetSubordinates(int managerId);

    /// <summary>
    /// Получить управляющих
    /// </summary>
    Task<List<Workers>> GetManagers(int subordinateId);
}