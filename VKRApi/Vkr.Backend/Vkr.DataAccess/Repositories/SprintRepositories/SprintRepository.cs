using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.SprintRepositories;
using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.Entities.Sprint;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Worker;

namespace Vkr.DataAccess.Repositories.SprintRepositories;

public class SprintRepository(ApplicationDbContext context) : ISprintRepository
{
    public async Task<int> AddAsync(SprintCreateDTO sprint)
    {
        ArgumentNullException.ThrowIfNull(sprint);

        if (string.IsNullOrWhiteSpace(sprint.Title))
            throw new ArgumentException("Sprint title is required");

        if (sprint.ExpireOn <= sprint.StartsOn)
            throw new ArgumentException("Expire date must be after start date");

        if (!await context.Projects.AnyAsync(p => p.Id == sprint.ProjectId))
            throw new ArgumentException("Invalid project ID");

        if (!await context.SprintStatus.AnyAsync(s => s.Id == sprint.SprintStatusId))
            throw new ArgumentException("Invalid sprint status ID");

        var newSprint = new Sprints
        {
            Title = sprint.Title,
            Goal = sprint.Goal,
            CreatedOn = DateTime.UtcNow,
            StartsOn = sprint.StartsOn,
            ExpireOn = sprint.ExpireOn,
            ProjectId = sprint.ProjectId,
            SprintStatusId = sprint.SprintStatusId
        };

        await context.Sprints.AddAsync(newSprint);
        await context.SaveChangesAsync();
        return newSprint.Id;
    }

    public async Task DeleteAsync(int sprintId)
    {
        var sprint = await context.Sprints
            .Include(s => s.TasksList)
            .FirstOrDefaultAsync(s => s.Id == sprintId);

        if (sprint == null)
            throw new ArgumentException("Sprint not found");

        if (sprint.TasksList != null)
        {
            foreach (var task in sprint.TasksList)
            {
                task.SprintId = null;
            }
        }

        context.Sprints.Remove(sprint);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SprintUpdateDTO sprint)
    {
        var existingSprint = await context.Sprints
            .FirstOrDefaultAsync(s => s.Id == sprint.Id);

        if (existingSprint == null)
            throw new ArgumentException("Sprint not found");

        if (string.IsNullOrWhiteSpace(sprint.Title))
            throw new ArgumentException("Sprint title is required");

        if (sprint.ExpireOn <= sprint.StartsOn)
            throw new ArgumentException("Expire date must be after start date");

        if (!await context.SprintStatus.AnyAsync(s => s.Id == sprint.SprintStatusId))
            throw new ArgumentException("Invalid sprint status ID");

        existingSprint.Title = sprint.Title;
        existingSprint.Goal = sprint.Goal;
        existingSprint.StartsOn = sprint.StartsOn;
        existingSprint.ExpireOn = sprint.ExpireOn;
        existingSprint.SprintStatusId = sprint.SprintStatusId;

        await context.SaveChangesAsync();
    }

    public async Task<Sprints> GetByIdAsync(int id)
    {
        var sprint = await context.Sprints
            .Include(s => s.Project)
            .Include(s => s.SprintStatus)
            .Include(s => s.TasksList)
                .ThenInclude(t => t.Project)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sprint == null)
            throw new KeyNotFoundException($"Sprint with ID {id} not found");

        return sprint;
    }

    public async Task<IEnumerable<Sprints>> GetByProjectAsync(int projectId)
    {
        if (!await context.Projects.AnyAsync(p => p.Id == projectId))
            throw new KeyNotFoundException($"Project with ID {projectId} not found");

        return await context.Sprints
            .Include(s => s.Project)
            .Include(s => s.SprintStatus)
            .Include(s => s.TasksList)
                .ThenInclude(t => t.Project)
            .Where(s => s.ProjectId == projectId)
            .OrderBy(s => s.StartsOn)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sprints>> GetByWorkerAsync(int workerId)
    {
        if (!await context.Workers.AnyAsync(w => w.Id == workerId))
            throw new KeyNotFoundException($"Worker with ID {workerId} not found");

        return await context.Sprints
            .Include(s => s.Project)
            .Include(s => s.SprintStatus)
            .Include(s => s.TasksList)
                .ThenInclude(t => t.Project)
            .Where(s => s.Project.WorkersList.Any(w => w.Id == workerId))
            .OrderBy(s => s.StartsOn)
            .ToListAsync();
    }
    public async Task<IEnumerable<TaskDTO>> GetTasksBySprintIdAsync(int sprintId)
    {
        var tasks = await context.Tasks
            .Where(t => t.SprintId == sprintId)
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
            Sprint = task.Sprint != null ? new SprintDTO { Id = task.Sprint.Id, Title = task.Sprint.Title } : null,
            StoryPoints = task.StoryPoints,
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