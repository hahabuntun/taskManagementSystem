using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities.Sprint;

namespace Vkr.Application.Interfaces.Repositories.SprintRepositories;

public interface ISprintRepository
{
    Task<int> AddAsync(SprintCreateDTO sprint);
    Task DeleteAsync(int sprintId);
    Task UpdateAsync(SprintUpdateDTO sprint);
    Task<Sprints> GetByIdAsync(int id);
    Task<IEnumerable<Sprints>> GetByProjectAsync(int projectId);
    Task<IEnumerable<Sprints>> GetByWorkerAsync(int workerId);
}