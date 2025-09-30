using Vkr.Application.Filters;
using Vkr.Domain.DTO;
using Vkr.Domain.DTO.Project;
using Vkr.Domain.Entities;

namespace Vkr.Application.Interfaces.Services.ProjectServices;

public interface IProjectService
{
    Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync();
    Task<ProjectDTO> GetProjectByIdAsync(int id);
    Task<IEnumerable<ProjectDTO>> GetProjectsByOrganizationAsync(int organizationId);
    Task<IEnumerable<ProjectDTO>> GetProjectsByStatusAsync(int statusId);
    Task<IEnumerable<ProjectDTO>> GetProjectsByManagerAsync(int managerId);
    Task<int> CreateProjectAsync(ProjectCreateDTO projectDto, int creatorId);
    Task UpdateProjectAsync(int projectId, ProjectUpdateDto projectDto, int creatorId);
    Task DeleteProjectAsync(int id, int creatorId);
    Task<IEnumerable<ProjectDTO>> GetProjectsByFilterAsync(ProjectsFilter filter);
    Task<List<Tags>> GetAvailableTags(int projectId);
    Task<List<Tags>> GetAllTags();
}