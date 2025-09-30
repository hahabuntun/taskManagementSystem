namespace Vkr.DataAccess.Repositories.WorkersRepositories;

using Microsoft.EntityFrameworkCore;
using Vkr.Application.Filters;
using Vkr.Application.Interfaces.Repositories.WorkerRepositories;
using Vkr.Domain.DTO;
using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Entities.Worker;

public class WorkersRepository(ApplicationDbContext context) : IWorkersRepository
{
    public async Task<Workers> GetByEmail(string email)
    {
        return await context.Workers
            .Include(w => w.WorkerPosition)
                .ThenInclude(wp => wp.TaskGiverRelations)
                .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(w => w.WorkerPosition)
                .ThenInclude(wp => wp.TaskTakerRelations)
                .ThenInclude(r => r.WorkerPosition)
            .FirstOrDefaultAsync(w => w.NormalizedEmail == email.ToUpper());
    }

    public async Task<Workers?> GetByIdAsync(int id)
    {
        var worker = await context.Workers
            .Include(w => w.WorkerPosition)
                .ThenInclude(wp => wp.TaskGiverRelations)
                .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(w => w.WorkerPosition)
                .ThenInclude(wp => wp.TaskTakerRelations)
                .ThenInclude(r => r.WorkerPosition)
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id);

        return worker;
    }

    public async Task<Workers> UpdateAsync(int id, Workers workers)
    {
        var numUpdated = await context.Workers
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.Name, workers.Name)
                .SetProperty(x => x.SecondName, workers.SecondName)
                .SetProperty(x => x.ThirdName, workers.ThirdName)
                .SetProperty(x => x.CanManageProjects, workers.CanManageProjects)
                .SetProperty(x => x.CanManageWorkers, workers.CanManageWorkers)
                .SetProperty(x => x.WorkerPositionId, workers.WorkerPositionId)
                .SetProperty(x => x.Email, workers.Email)
                .SetProperty(x => x.NormalizedEmail, workers.NormalizedEmail)
                .SetProperty(x => x.Phone, workers.Phone)
                .SetProperty(x => x.PasswordHash, workers.PasswordHash)
                .SetProperty(x => x.WorkerStatus, workers.WorkerStatus));

        if (numUpdated == 0)
            throw new InvalidOperationException($"Worker with ID {id} not found");

        var updatedWorker = await context.Workers
            .Include(w => w.WorkerPosition)
                .ThenInclude(wp => wp.TaskGiverRelations)
                .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(w => w.WorkerPosition)
                .ThenInclude(wp => wp.TaskTakerRelations)
                .ThenInclude(r => r.WorkerPosition)
            .FirstOrDefaultAsync(w => w.Id == id);

        return updatedWorker ?? throw new InvalidOperationException($"Failed to retrieve updated worker with ID {id}");
    }

    public async Task<Workers> CreateAsync(Workers workers)
    {
        var entry = await context.Workers.AddAsync(workers);
        await context.SaveChangesAsync();
        await context.Entry(entry.Entity)
            .Reference(w => w.WorkerPosition)
            .Query()
            .Include(wp => wp.TaskGiverRelations)
                .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(wp => wp.TaskTakerRelations)
                .ThenInclude(r => r.WorkerPosition)
            .LoadAsync();

        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var numDeleted = await context.Workers
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return numDeleted != 0;
    }

    public async Task<List<Workers>> GetByFilterAsync(WorkersFilter filter)
    {
        var query = context.Workers
            .Include(w => w.WorkerPosition)
                .ThenInclude(wp => wp.TaskGiverRelations)
                .ThenInclude(r => r.SubordinateWorkerPosition)
            .Include(w => w.WorkerPosition)
                .ThenInclude(wp => wp.TaskTakerRelations)
                .ThenInclude(r => r.WorkerPosition)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(x => x.Name.Contains(filter.Name));
        }

        if (!string.IsNullOrEmpty(filter.SecondName))
        {
            query = query.Where(x => x.SecondName.Contains(filter.SecondName));
        }

        if (!string.IsNullOrEmpty(filter.ThirdName))
        {
            query = query.Where(x => x.ThirdName.Contains(filter.ThirdName));
        }

        if (!string.IsNullOrEmpty(filter.Email))
        {
            query = query.Where(x => x.Email.Contains(filter.Email));
        }
        if (filter.CanManageWorkers.HasValue)
        {
            query = query.Where(x => x.CanManageWorkers == filter.CanManageWorkers.Value);
        }

        if (filter.CanManageProjects.HasValue)
        {
            query = query.Where(x => x.CanManageProjects == filter.CanManageProjects.Value);
        }

        var workers = await query.ToListAsync();

        return workers;
    }

    public async Task<bool> IsEmailUnique(string candidateEmail)
    {
        var isExistUserWithEmail = await context.Workers.AnyAsync(x => string.Equals(x.NormalizedEmail, candidateEmail.ToUpper()));
        return !isExistUserWithEmail;
    }

    public async Task<int[]> ValidateWorkerIdsAsync(int[] workerIds)
    {
        return await context.Workers
            .Where(w => workerIds.Contains(w.Id))
            .Select(w => w.Id)
            .ToArrayAsync();
    }

    public async Task<IEnumerable<TaskDTO>> GetTasksByWorkerIdAsync(int workerId)
    {
        var tasks = await context.Tasks
            .Where(t => t.TaskExecutors.Any(te => te.WorkerId == workerId))
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskPriority)
            .Include(t => t.Project)
            .Include(t => t.Sprint)
            .Include(t => t.Creator)
            .Include(t => t.TaskExecutors).ThenInclude(te => te.Worker)
            .Include(t => t.Tags)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelatedTask).ThenInclude(t => t.Project)
            .Include(t => t.TaskRelationships).ThenInclude(tr => tr.RelationshipType)
            .ToListAsync();

        var relationshipTypes = await context.TaskRelationshipTypes
            .ToDictionaryAsync(trt => trt.Id, trt => trt.Name);

        return tasks.Select(task => MapToTaskDTO(task, relationshipTypes)).ToList();
    }

    private TaskDTO MapToTaskDTO(Tasks task, Dictionary<int, string> relationshipTypes)
    {
        var relatedTasks = new List<RelatedTaskDTO>();

        relatedTasks.AddRange(task.TaskRelationships.Select(tr => new RelatedTaskDTO
        {
            Task = MapToMinimalTaskDTO(tr.RelatedTask),
            RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name) ? name : "Unknown",
            RelationshipId = tr.Id
        }));

        relatedTasks.AddRange(task.RelatedTaskRelationships.Select(tr => new RelatedTaskDTO
        {
            Task = MapToMinimalTaskDTO(tr.Task),
            RelationshipType = relationshipTypes.TryGetValue(tr.TaskRelationshipTypeId, out var name)
                ? (name == "ParentChild" ? "Parent" : name)
                : "Unknown",
            RelationshipId = tr.Id
        }));

        var responsibleExecutor = task.TaskExecutors.FirstOrDefault(te => te.IsResponsible);

        if (responsibleExecutor == null && task.TaskExecutors.Any())
        {
            Console.WriteLine($"Warning: No responsible executor found for TaskId={task.Id}. Executors: {string.Join(", ", task.TaskExecutors.Select(te => te.WorkerId))}");
        }

        var responsibleCount = task.TaskExecutors.Count(te => te.IsResponsible);
        if (responsibleCount > 1)
        {
            Console.WriteLine($"Error: Multiple responsible executors found for TaskId={task.Id}. Executors: {string.Join(", ", task.TaskExecutors.Where(te => te.IsResponsible).Select(te => te.WorkerId))}");
            throw new InvalidOperationException("Multiple responsible executors detected for task. Only one is allowed.");
        }

        return new TaskDTO
        {
            Id = task.Id,
            ShortName = task.ShortName ?? string.Empty,
            Description = task.Description,
            CreatedOn = task.CreatedOn,
            Progress = task.Progress,
            StartOn = task.StartOn,
            ExpireOn = task.ExpireOn,
            StoryPoints = task.StoryPoints,
            Project = new ProjectDTO { Id = task.Project.Id, Name = task.Project.Name },
            Creator = new WorkerDTO { Id = task.Creator.Id, Name = task.Creator.Name },
            TaskType = new TaskTypeDTO { Id = task.TaskType.Id, Name = task.TaskType.Name },
            TaskStatus = new TaskStatusDTO { Id = task.TaskStatus.Id, Name = task.TaskStatus.Name, Color = task.TaskStatus.Color },
            TaskPriority = task.TaskPriority != null ? new TaskPriorityDTO { Id = task.TaskPriority.Id, Name = task.TaskPriority.Name, Color = task.TaskPriority.Color } : null,
            Sprint = task.Sprint != null ? new SprintDTO { Id = task.Sprint.Id, Title = task.Sprint.Title } : null,
            Executors = task.TaskExecutors.Select(te => new WorkerDTO
            {
                Id = te.Worker.Id,
                Name = te.Worker.Name,
                SecondName = te.Worker.SecondName,
                Email = te.Worker.Email,
                CreatedOn = te.Worker.CreatedOn,
                WorkerPositionId = te.Worker.WorkerPositionId,
                CanManageWorkers = te.Worker.CanManageWorkers,
                CanManageProjects = te.Worker.CanManageProjects
            }).ToList(),
            Observers = task.TaskObservers.Select(to => new WorkerDTO
            {
                Id = to.Worker.Id,
                Name = to.Worker.Name,
                SecondName = to.Worker.SecondName,
                Email = to.Worker.Email,
                CreatedOn = to.Worker.CreatedOn,
                WorkerPositionId = to.Worker.WorkerPositionId,
                CanManageWorkers = to.Worker.CanManageWorkers,
                CanManageProjects = to.Worker.CanManageProjects
            }).ToList(),
            ResponsibleWorker = responsibleExecutor != null ? new WorkerDTO
            {
                Id = responsibleExecutor.Worker.Id,
                Name = responsibleExecutor.Worker.Name,
                SecondName = responsibleExecutor.Worker.SecondName,
                Email = responsibleExecutor.Worker.Email,
                CreatedOn = responsibleExecutor.Worker.CreatedOn,
                WorkerPositionId = responsibleExecutor.Worker.WorkerPositionId,
                CanManageWorkers = responsibleExecutor.Worker.CanManageWorkers,
                CanManageProjects = responsibleExecutor.Worker.CanManageProjects
            } : null,
            RelatedTasks = relatedTasks,
            TagDTOs = task.Tags.Select(t => new FullTagDTO { Id = t.Id, Name = t.Name, Color = t.Color }).ToList(),
        };
    }

    private TaskDTO MapToMinimalTaskDTO(Tasks task)
    {
        return new TaskDTO
        {
            Id = task.Id,
            ShortName = task.ShortName ?? string.Empty,
            StoryPoints = task.StoryPoints,
            Sprint = task.Sprint != null ? new SprintDTO { Id = task.Sprint.Id, Title = task.Sprint.Title } : null,
            Project = new ProjectDTO
            {
                Id = task.Project.Id,
                Name = task.Project.Name,
                Description = task.Project.Description,
                Goal = task.Project.Goal,
                ManagerId = task.Project.ManagerId,
                Progress = task.Project.Progress,
                ProjectStatusName = task.Project.ProjectStatus?.Name,
            },
            RelatedTasks = new List<RelatedTaskDTO>()
        };
    }


}