using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.ProjectRepositories;
using Vkr.Domain.Entities.Progect;

namespace Vkr.DataAccess.Repositories.ProjectRepositories;

public class ProjectChecklistRepository(ApplicationDbContext context): IProjectChecklistRepository
{
    public async Task<ProjectChecklist> GetByIdAsync(int id)
    {
        return await context.ProjectChecklists
            .Include(pc => pc.Project)
            .Include(pc => pc.Creator)
            .Include(pc => pc.Checks)
            .FirstOrDefaultAsync(pc => pc.Id == id);
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public  async Task<IEnumerable<ProjectChecklist>> GetAllAsync()
    {
        return await context.ProjectChecklists
            .Include(pc => pc.Project)
            .Include(pc => pc.Creator)
            .OrderByDescending(pc => pc.CreatedOn)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectChecklist>> GetByProjectAsync(int projectId)
    {
        return await context.ProjectChecklists
            .Where(pc => pc.ProjectId == projectId)
            .Include(pc => pc.Checks)
            .OrderBy(pc => pc.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProjectChecklist>> GetByCreatorAsync(int workerId)
    {
        return await context.ProjectChecklists
            .Where(pc => pc.WorkerId == workerId)
            .Include(pc => pc.Project)
            .OrderByDescending(pc => pc.CreatedOn)
            .ToListAsync();
    }

    public async Task<bool> TitleExistsInProjectAsync(string title, int projectId)
    {
        return await context.ProjectChecklists
            .AnyAsync(pc => 
                pc.Title.ToLower() == title.ToLower() && 
                pc.ProjectId == projectId);
    }
}