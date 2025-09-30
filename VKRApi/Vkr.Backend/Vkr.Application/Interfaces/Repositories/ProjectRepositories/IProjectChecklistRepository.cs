using Vkr.Domain.Entities.Progect;

namespace Vkr.Application.Interfaces.Repositories.ProjectRepositories;

public interface IProjectChecklistRepository
{
    Task<IEnumerable<ProjectChecklist>> GetByProjectAsync(int projectId);
    Task<IEnumerable<ProjectChecklist>> GetByCreatorAsync(int workerId);
    Task<bool> TitleExistsInProjectAsync(string title, int projectId);
}