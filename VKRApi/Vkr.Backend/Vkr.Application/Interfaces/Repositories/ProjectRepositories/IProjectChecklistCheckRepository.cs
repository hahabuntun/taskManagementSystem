using Vkr.Domain.Entities.Progect;

namespace Vkr.Application.Interfaces.Repositories.ProjectRepositories;

public interface IProjectChecklistCheckRepository
{
    Task<IEnumerable<ProjectChecklistCheck>> GetByChecklistAsync(int checklistId);
    Task ToggleCheckAsync(int checkId);

    Task<ProjectChecklistCheck> GetByIdAsync(int id);
}