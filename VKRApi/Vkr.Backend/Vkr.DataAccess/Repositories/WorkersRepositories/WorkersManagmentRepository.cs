using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.WorkerRepositories;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Worker;

namespace Vkr.DataAccess.Repositories.WorkersRepositories;

public class WorkersManagmentRepository(ApplicationDbContext context) : IWorkersManagmentRepository
{
    /// <inheritdoc/>
    public async Task<bool> SetConnection(WorkersManagmentDTO request)
    {
        await context.WorkersManagements.AddAsync(new WorkersManagement()
        {
            ManagerId = request.ManagerId,
            SubordinateId = request.SubordinateId,
        });
        await context.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteConnection(int managerId, int subordinateId)
    {
        var connection = await context.WorkersManagements
            .FirstOrDefaultAsync(wm => wm.ManagerId == managerId && wm.SubordinateId == subordinateId);

        if (connection == null)
        {
            return false;
        }

        context.WorkersManagements.Remove(connection);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<int> UpdateConnection(WorkersManagmentDTO request)
    {
        return await context.WorkersManagements.Where(wm =>
                wm.ManagerId == request.ManagerId && wm.SubordinateId == request.SubordinateId)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(x => x.SubordinateId, y => request.SubordinateId)
                .SetProperty(x => x.ManagerId, y => request.ManagerId));
    }

    public async Task<List<Workers>> GetSubordinates(int managerId)
    {
        return await context.WorkersManagements
            .Where(wm => wm.ManagerId == managerId)
            .Include(wm => wm.Subordinate)
                .ThenInclude(s => s.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                    .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(wm => wm.Subordinate)
                .ThenInclude(s => s.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                    .ThenInclude(r => r.WorkerPosition)
            .Select(wm => wm.Subordinate)
            .ToListAsync();
    }

    public async Task<List<Workers>> GetManagers(int subordinateId)
    {
        return await context.WorkersManagements
            .Where(wm => wm.SubordinateId == subordinateId)
            .Include(wm => wm.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskGiverRelations)
                    .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(wm => wm.Manager)
                .ThenInclude(m => m.WorkerPosition)
                    .ThenInclude(wp => wp.TaskTakerRelations)
                    .ThenInclude(r => r.WorkerPosition)
            .Select(wm => wm.Manager)
            .ToListAsync();
    }
}