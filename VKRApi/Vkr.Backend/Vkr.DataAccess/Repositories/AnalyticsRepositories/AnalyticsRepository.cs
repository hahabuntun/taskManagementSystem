
using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.AnalyticsRepositories;
using Vkr.DataAccess;
using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Task;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly ApplicationDbContext _context;

    public AnalyticsRepository(ApplicationDbContext context)
{
    _context = context;
}

public async Task<OrganizationAnalyticsData> GetOrganizationAnalyticsAsync(int organizationId)
{
    var projectIds = await _context.Projects
        .Where(p => p.OrganizationId == organizationId)
        .Select(p => p.Id)
        .ToListAsync();

    var tasks = await _context.Tasks
        .Include(t => t.TaskStatus)
        .Include(t => t.TaskPriority)
        .Include(t => t.Tags)
        .Include(t => t.TaskExecutors)
        .ThenInclude(te => te.Worker)
        .Include(t => t.Project)
        .Where(t => projectIds.Contains(t.ProjectId))
        .ToListAsync();

    var tasksByStatus = tasks
        .GroupBy(t => t.TaskStatus)
        .Select(g => new StatusCountDTO
        {
            Status = new TaskStatusDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByPriority = tasks
        .Where(t => t.TaskPriority != null)
        .GroupBy(t => t.TaskPriority)
        .Select(g => new PriorityCountDTO
        {
            Priority = new TaskPriorityDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByTag = tasks
        .SelectMany(t => t.Tags, (t, tag) => new { Task = t, Tag = tag })
        .GroupBy(x => x.Tag)
        .Select(g => new TagCountDTO
        {
            Tag = new FullTagDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByProject = tasks
        .GroupBy(t => t.Project)
        .Select(g => (
            ProjectName: g.Key.Name,
            Completed: g.Count(t => t.TaskStatus.Name == "Завершена"),
            Overdue: g.Count(t => t.ExpireOn < DateTime.UtcNow && t.TaskStatus.Name != "Завершена")
        ))
        .ToArray();

    var tasksByEmployee = tasks
        .SelectMany(t => t.TaskExecutors, (t, te) => new { Task = t, Worker = te.Worker })
        .GroupBy(x => x.Worker)
        .Select(g => (
            Worker: new WorkerDTO
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                SecondName = g.Key.SecondName,
                Email = g.Key.Email
            },
            Statuses: g
                .GroupBy(x => x.Task.TaskStatus)
                .Select(s => new StatusCountDTO
                {
                    Status = new TaskStatusDTO { Id = s.Key.Id, Name = s.Key.Name, Color = s.Key.Color },
                    Count = s.Count()
                })
                .ToArray()
        ))
        .ToArray();

    var overdueTasks = tasks
        .Count(t => t.ExpireOn < DateTime.UtcNow && t.TaskStatus.Name != "Completed");

    return new OrganizationAnalyticsData
    {
        TasksByStatus = tasksByStatus,
        TasksByPriority = tasksByPriority,
        TasksByTag = tasksByTag,
        TasksByProject = tasksByProject,
        TasksByEmployee = tasksByEmployee,
        OverdueTasks = overdueTasks
    };
}

public async Task<WorkerAnalyticsData> GetWorkerAnalyticsAsync(int workerId)
{
    var tasks = await _context.Tasks
        .Include(t => t.TaskStatus)
        .Include(t => t.TaskPriority)
        .Include(t => t.Tags)
        .Include(t => t.Project)
        .Include(t => t.Sprint).ThenInclude(s => s.SprintStatus)
        .Where(t => t.TaskExecutors.Any(te => te.WorkerId == workerId))
        .ToListAsync();

    var tasksByProject = tasks
        .GroupBy(t => t.Project)
        .Select(g => (
            ProjectId: g.Key.Id,
            ProjectName: g.Key.Name,
            Count: g.Count(),
            Statuses: g
                .GroupBy(t => t.TaskStatus)
                .Select(s => new StatusCountDTO
                {
                    Status = new TaskStatusDTO { Id = s.Key.Id, Name = s.Key.Name, Color = s.Key.Color },
                    Count = s.Count()
                })
                .ToArray(),
            Overdue: g.Count(t => t.ExpireOn < DateTime.UtcNow && t.TaskStatus.Name != "Completed")
        ))
        .ToArray();

    var tasksBySprint = tasks
        .Where(t => t.Sprint != null)
        .GroupBy(t => t.Sprint)
        .Select(g => (SprintId: g.Key.Id, Count: g.Count()))
        .ToArray();

    var tasksByTag = tasks
        .SelectMany(t => t.Tags, (t, tag) => new { Task = t, Tag = tag })
        .GroupBy(x => x.Tag)
        .Select(g => new TagCountDTO
        {
            Tag = new FullTagDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByStatus = tasks
        .GroupBy(t => t.TaskStatus)
        .Select(g => new StatusCountDTO
        {
            Status = new TaskStatusDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByPriority = tasks
        .Where(t => t.TaskPriority != null)
        .GroupBy(t => t.TaskPriority)
        .Select(g => new PriorityCountDTO
        {
            Priority = new TaskPriorityDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var overdueTasks = tasks
        .Count(t => t.ExpireOn < DateTime.UtcNow && t.TaskStatus.Id != 4);

    return new WorkerAnalyticsData
    {
        TasksByProject = tasksByProject,
        TasksBySprint = tasksBySprint,
        TasksByTag = tasksByTag,
        TasksByStatus = tasksByStatus,
        TasksByPriority = tasksByPriority,
        OverdueTasks = overdueTasks
    };
}

public async Task<SprintAnalyticsData> GetSprintAnalyticsAsync(int sprintId)
{
    var sprint = await _context.Sprints
        .Include(s => s.TasksList)
        .ThenInclude(t => t.TaskStatus)
        .Include(s => s.TasksList)
        .ThenInclude(t => t.TaskPriority)
        .Include(s => s.TasksList)
        .ThenInclude(t => t.Tags)
        .Include(s => s.TasksList)
        .ThenInclude(t => t.TaskExecutors)
        .ThenInclude(te => te.Worker)
        .FirstOrDefaultAsync(s => s.Id == sprintId);

    if (sprint == null)
        throw new KeyNotFoundException($"Sprint with ID {sprintId} not found");

    var tasks = sprint.TasksList ?? new List<Tasks>();

    var tasksByStatus = tasks
        .GroupBy(t => t.TaskStatus)
        .Select(g => new StatusCountDTO
        {
            Status = new TaskStatusDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByPriority = tasks
        .Where(t => t.TaskPriority != null)
        .GroupBy(t => t.TaskPriority)
        .Select(g => new PriorityCountDTO
        {
            Priority = new TaskPriorityDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByTag = tasks
        .SelectMany(t => t.Tags, (t, tag) => new { Task = t, Tag = tag })
        .GroupBy(x => x.Tag)
        .Select(g => new TagCountDTO
        {
            Tag = new FullTagDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByEmployee = tasks
        .SelectMany(t => t.TaskExecutors, (t, te) => new { Task = t, Worker = te.Worker })
        .GroupBy(x => x.Worker)
        .Select(g => (
            Worker: new WorkerDTO
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                SecondName = g.Key.SecondName,
                Email = g.Key.Email
            },
            Statuses: g
                .GroupBy(x => x.Task.TaskStatus)
                .Select(s => new StatusCountDTO
                {
                    Status = new TaskStatusDTO { Id = s.Key.Id, Name = s.Key.Name, Color = s.Key.Color },
                    Count = s.Count()
                })
                .ToArray()
        ))
        .ToArray();

    var totalTasks = tasks.Count;

    var overdueTasks = tasks
        .Count(t => t.ExpireOn < DateTime.UtcNow && t.TaskStatus.Name != "Completed");

    return new SprintAnalyticsData
    {
        TasksByStatus = tasksByStatus,
        TasksByPriority = tasksByPriority,
        TasksByTag = tasksByTag,
        TasksByEmployee = tasksByEmployee,
        TotalTasks = totalTasks,
        OverdueTasks = overdueTasks
    };
}

public async Task<ProjectAnalyticsData> GetProjectAnalyticsAsync(int projectId)
{
    var tasks = await _context.Tasks
        .Include(t => t.TaskStatus)
        .Include(t => t.TaskPriority)
        .Include(t => t.Tags)
        .Include(t => t.TaskExecutors)
            .ThenInclude(te => te.Worker)
        .Include(t => t.Sprint)
            .ThenInclude(s => s.SprintStatus)
        .Where(t => t.ProjectId == projectId)
        .ToListAsync();

    var tasksByStatus = tasks
        .GroupBy(t => t.TaskStatus)
        .Select(g => new StatusCountDTO
        {
            Status = new TaskStatusDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByPriority = tasks
        .Where(t => t.TaskPriority != null)
        .GroupBy(t => t.TaskPriority)
        .Select(g => new PriorityCountDTO
        {
            Priority = new TaskPriorityDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByTag = tasks
        .SelectMany(t => t.Tags, (t, tag) => new { Task = t, Tag = tag })
        .GroupBy(x => x.Tag)
        .Select(g => new TagCountDTO
        {
            Tag = new FullTagDTO { Id = g.Key.Id, Name = g.Key.Name, Color = g.Key.Color },
            Count = g.Count()
        })
        .ToArray();

    var tasksByEmployee = tasks
        .SelectMany(t => t.TaskExecutors, (t, te) => new { Task = t, Worker = te.Worker })
        .GroupBy(x => x.Worker)
        .Select(g => (
            Worker: new WorkerDTO
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                SecondName = g.Key.SecondName,
                Email = g.Key.Email
            },
            Statuses: g
                .GroupBy(x => x.Task.TaskStatus)
                .Select(s => new StatusCountDTO
                {
                    Status = new TaskStatusDTO { Id = s.Key.Id, Name = s.Key.Name, Color = s.Key.Color },
                    Count = s.Count()
                })
                .ToArray()
        ))
        .ToArray();

    var tasksBySprint = tasks
        .Where(t => t.Sprint != null)
        .GroupBy(t => t.Sprint)
        .Select(g => new SprintCountDTO
        {
            Sprint = new SprintDTO
            {
                Id = g.Key.Id,
                Title = g.Key.Title,
                Goal = g.Key.Goal,
                CreatedOn = g.Key.CreatedOn,
                StartsOn = g.Key.StartsOn,
                ExpireOn = g.Key.ExpireOn,
                ProjectId = g.Key.ProjectId,
                ProjectName = g.Key.Project?.Name ?? string.Empty,
                SprintStatusId = g.Key.SprintStatusId,
                SprintStatus = new SprintStatusDTO
                {
                    Id = g.Key.SprintStatus.Id,
                    Name = g.Key.SprintStatus.Name
                }
            },
            completedTasksNum = g.Count(t => t.TaskStatus.Name == "Завершена")
        })
        .ToArray();

    var overdueTasks = tasks
        .Count(t => t.ExpireOn < DateTime.UtcNow && t.TaskStatus.Name != "Завершена");

    return new ProjectAnalyticsData
    {
        TasksByStatus = tasksByStatus,
        TasksByPriority = tasksByPriority,
        TasksByTag = tasksByTag,
        TasksByEmployee = tasksByEmployee,
        TasksBySprint = tasksBySprint,
        OverdueTasks = overdueTasks
    };
}

   
}

