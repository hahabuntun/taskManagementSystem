using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Worker;

public class OrganizationAnalyticsData
{
    public StatusCountDTO[] TasksByStatus { get; set; }
    public PriorityCountDTO[] TasksByPriority { get; set; }
    public TagCountDTO[] TasksByTag { get; set; }
    public (string projectName, int completed, int overdue)[] TasksByProject { get; set; }
    public (WorkerDTO Worker, StatusCountDTO[] statuses)[] TasksByEmployee { get; set; }
    public int OverdueTasks { get; set; }
}

public class WorkerAnalyticsData
{
    public (int projectId, string projectName, int count, StatusCountDTO[] statuses, int overdue)[] TasksByProject { get; set; }
    public (int sprintId, int count)[] TasksBySprint { get; set; }
    public TagCountDTO[] TasksByTag { get; set; }
    public StatusCountDTO[] TasksByStatus { get; set; }
    public PriorityCountDTO[] TasksByPriority { get; set; }
    public int OverdueTasks { get; set; }
}

public class SprintAnalyticsData
{
    public StatusCountDTO[] TasksByStatus { get; set; }
    public PriorityCountDTO[] TasksByPriority { get; set; }
    public TagCountDTO[] TasksByTag { get; set; }
    public (WorkerDTO Worker, StatusCountDTO[] statuses)[] TasksByEmployee { get; set; }
    public int TotalTasks { get; set; }
    public int OverdueTasks { get; set; }
}

public class ProjectAnalyticsData
{
    public StatusCountDTO[] TasksByStatus { get; set; }
    public PriorityCountDTO[] TasksByPriority { get; set; }
    public TagCountDTO[] TasksByTag { get; set; }
    public (WorkerDTO Worker, StatusCountDTO[] statuses)[] TasksByEmployee { get; set; }
    public SprintCountDTO[] TasksBySprint { get; set; } // Изменено на SprintCountDTO[]
    public int OverdueTasks { get; set; }
}

public class SprintCountDTO
{
    public SprintDTO Sprint { get; set; }
    public int completedTasksNum { get; set; }
}

public class StatusCountDTO
{
    public TaskStatusDTO Status { get; set; }
    public int Count { get; set; }
}

public class PriorityCountDTO
{
    public TaskPriorityDTO Priority { get; set; }
    public int Count { get; set; }
}


public class TagCountDTO
{
    public FullTagDTO Tag { get; set; }
    public int Count { get; set; }
}