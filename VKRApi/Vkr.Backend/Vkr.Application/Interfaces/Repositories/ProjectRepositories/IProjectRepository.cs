using Vkr.Application.Filters;
using Vkr.Domain.DTO;
using Vkr.Domain.DTO.Project;
using Vkr.Domain.Entities.Progect;

namespace Vkr.Application.Interfaces.Repositories.ProjectRepositories;

public interface IProjectRepository
{
    Task<ProjectDTO?> GetByIdAsync(int id);
    Task<IEnumerable<ProjectDTO?>> GetAllAsync();
    Task<IEnumerable<ProjectDTO>> GetByOrganizationAsync(int organizationId);
    Task<IEnumerable<ProjectDTO>> GetByStatusAsync(int statusId);
    Task<IEnumerable<ProjectDTO>> GetByManagerAsync(int managerId);
    Task<bool> ProjectNameExistsAsync(string name, int organizationId);
    Task<int> AddAsync(ProjectCreateDTO project);
    Task UpdateAsync(int projectId, ProjectUpdateDto projectDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<ProjectDTO>> GetByFilterAsync(ProjectsFilter filter);
}