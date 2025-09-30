using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.ProjectRepositories;
using Vkr.Domain.Entities.Progect;

namespace Vkr.DataAccess.Repositories.ProjectRepositories;

public class ProjectChecklistCheckRepository(ApplicationDbContext context): IProjectChecklistCheckRepository
{
    public async Task<IEnumerable<ProjectChecklistCheck>> GetByChecklistAsync(int checklistId)
    {
        return await context.ProjectChecklistChecks
            .Where(pcc => pcc.ProjectChecklistId == checklistId)
            .OrderBy(pcc => pcc.Id)
            .ToListAsync();
    }

    public async Task ToggleCheckAsync(int checkId)
    {
        var check = await context.ProjectChecklistChecks.FindAsync(checkId);
        if (check != null)
        {
            check.IsChecked = !check.IsChecked;
            await context.SaveChangesAsync();
        }
    }

    public async Task<ProjectChecklistCheck> GetByIdAsync(int id)
    {
        return await context.ProjectChecklistChecks
            .Include(pcc => pcc.ProjectChecklist)
            .FirstOrDefaultAsync(pcc => pcc.Id == id);
    }
}