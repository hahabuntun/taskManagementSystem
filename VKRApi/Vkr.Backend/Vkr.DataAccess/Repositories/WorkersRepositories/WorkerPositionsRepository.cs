using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.WorkerRepositories;
using Vkr.DataAccess;
using Vkr.Domain.Entities.Worker;
using Task = System.Threading.Tasks.Task;
using Vkr.Domain.DTO.Worker;

namespace Vkr.DataAccess.Repositories.WorkersRepositories;

public class WorkerPositionsRepository(ApplicationDbContext context) : IWorkerPositionsRepository
{
    public async Task<bool> DeleteWorkerPositionById(int id)
    {
        var numDeleted = await context.WorkerPositions
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return numDeleted != 0;
    }

     public async Task<List<WorkerPositionDto>> GetWorkerPositions()
    {
        return await context.WorkerPositions
            .AsNoTracking()
            .Select(x => new WorkerPositionDto
            {
                Id = x.Id,
                Title = x.Title,
                TaskGivers = x.TaskTakerRelations
                    .Select(r => new WorkerPositionSummary
                    {
                        Id = r.WorkerPosition.Id,
                        Title = r.WorkerPosition.Title
                    })
                    .ToList(),
                TaskTakers = x.TaskGiverRelations
                    .Select(r => new WorkerPositionSummary
                    {
                        Id = r.SubordinateWorkerPosition.Id,
                        Title = r.SubordinateWorkerPosition.Title
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<WorkerPositionDto?> GetWorkerPositionById(int id)
    {
        return await context.WorkerPositions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new WorkerPositionDto
            {
                Id = x.Id,
                Title = x.Title,
                TaskGivers = x.TaskTakerRelations
                    .Select(r => new WorkerPositionSummary
                    {
                        Id = r.WorkerPosition.Id,
                        Title = r.WorkerPosition.Title
                    })
                    .ToList(),
                TaskTakers = x.TaskGiverRelations
                    .Select(r => new WorkerPositionSummary
                    {
                        Id = r.SubordinateWorkerPosition.Id,
                        Title = r.SubordinateWorkerPosition.Title
                    })
                    .ToList()
            })
            .SingleOrDefaultAsync();
    }

    public async Task<WorkerPositionDto?> UpdateWorkerPositions(int id, WorkerPosition workerPosition, int[] taskGiverIds, int[] taskTakerIds)
    {
        var existingPosition = await context.WorkerPositions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (existingPosition == null)
        {
            return null;
        }

        await context.WorkerPositions
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.Title, workerPosition.Title));

        await context.WorkerPositionRelations
            .Where(x => x.SubordinateWorkerPositionId == id || x.WorkerPositionId == id)
            .ExecuteDeleteAsync();

        var newRelations = new List<WorkerPositionRelation>();

        if (taskGiverIds != null && taskGiverIds.Length > 0)
        {
            newRelations.AddRange(taskGiverIds
                .Distinct()
                .Select(giverId => new WorkerPositionRelation
                {
                    WorkerPositionId = giverId,
                    SubordinateWorkerPositionId = id
                }));
        }

        if (taskTakerIds != null && taskTakerIds.Length > 0)
        {
            newRelations.AddRange(taskTakerIds
                .Distinct()
                .Select(takerId => new WorkerPositionRelation
                {
                    WorkerPositionId = id,
                    SubordinateWorkerPositionId = takerId
                }));
        }

        // Remove duplicates based on WorkerPositionId and SubordinateWorkerPositionId
        newRelations = newRelations
            .GroupBy(r => new { r.WorkerPositionId, r.SubordinateWorkerPositionId })
            .Select(g => g.First())
            .ToList();

        if (newRelations.Count > 0)
        {
            await context.WorkerPositionRelations.AddRangeAsync(newRelations);
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Failed to update worker position relations.", ex);
        }

        return await context.WorkerPositions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new WorkerPositionDto
            {
                Id = x.Id,
                Title = x.Title,
                TaskGivers = x.TaskTakerRelations
                    .Select(r => new WorkerPositionSummary
                    {
                        Id = r.WorkerPosition.Id,
                        Title = r.WorkerPosition.Title
                    })
                    .ToList(),
                TaskTakers = x.TaskGiverRelations
                    .Select(r => new WorkerPositionSummary
                    {
                        Id = r.SubordinateWorkerPosition.Id,
                        Title = r.SubordinateWorkerPosition.Title
                    })
                    .ToList()
            })
            .SingleOrDefaultAsync();
    }

    public async Task<WorkerPositionDto> CreateWorkerPosition(WorkerPosition workerPosition, int[]? taskGiverIds = null, int[]? taskTakerIds = null)
    {

        context.WorkerPositions.Add(workerPosition);
        await context.SaveChangesAsync();

        var newRelations = new List<WorkerPositionRelation>();

        if (taskGiverIds != null && taskGiverIds.Length > 0)
        {
            newRelations.AddRange(taskGiverIds
                .Distinct()
                .Select(giverId => new WorkerPositionRelation
                {
                    WorkerPositionId = giverId,
                    SubordinateWorkerPositionId = workerPosition.Id
                }));
        }

        if (taskTakerIds != null && taskTakerIds.Length > 0)
        {
            newRelations.AddRange(taskTakerIds
                .Distinct()
                .Select(takerId => new WorkerPositionRelation
                {
                    WorkerPositionId = workerPosition.Id,
                    SubordinateWorkerPositionId = takerId
                }));
        }

        // Remove duplicates based on WorkerPositionId and SubordinateWorkerPositionId
        newRelations = newRelations
            .GroupBy(r => new { r.WorkerPositionId, r.SubordinateWorkerPositionId })
            .Select(g => g.First())
            .ToList();

        if (newRelations.Count > 0)
        {
            await context.WorkerPositionRelations.AddRangeAsync(newRelations);
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Failed to create worker position relations.", ex);
        }

        return await context.WorkerPositions
            .AsNoTracking()
            .Where(x => x.Id == workerPosition.Id)
            .Select(x => new WorkerPositionDto
            {
                Id = x.Id,
                Title = x.Title,
                TaskGivers = x.TaskTakerRelations
                    .Select(r => new WorkerPositionSummary
                    {
                        Id = r.WorkerPosition.Id,
                        Title = r.WorkerPosition.Title
                    })
                    .ToList(),
                TaskTakers = x.TaskGiverRelations
                    .Select(r => new WorkerPositionSummary
                    {
                        Id = r.SubordinateWorkerPosition.Id,
                        Title = r.SubordinateWorkerPosition.Title
                    })
                    .ToList()
            })
            .SingleAsync();
    }

    public async Task<bool> IsWorkerPositionExists(int id)
    {
        return await context.WorkerPositions.AnyAsync(x => x.Id == id);
    }
}