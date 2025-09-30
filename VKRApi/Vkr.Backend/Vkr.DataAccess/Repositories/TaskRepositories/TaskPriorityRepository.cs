using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.TaskRepositories;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Repositories.TaskRepositories;

public class TaskPriorityRepository(ApplicationDbContext context): ITaskPriorityRepository
{
    public async Task<TaskPriority> GetByNameAsync(string name)
    {
        return await context.TaskPriorities
            .FirstOrDefaultAsync(tp => tp.Name.ToLower() == name.ToLower());
    }
}