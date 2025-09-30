using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Task;

namespace Vkr.Application.Interfaces.Services.SprintServices;

public interface ISprintService
{
    Task<int> CreateAsync(SprintCreateDTO dto, int userId);
    Task DeleteAsync(int sprintId, int userId);
    Task UpdateAsync(SprintUpdateDTO dto);
    Task<SprintDTO> GetByIdAsync(int id);
    Task<IEnumerable<SprintDTO>> GetByProjectAsync(int projectId);
    Task<IEnumerable<SprintDTO>> GetByWorkerAsync(int workerId);
}