using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.DTO.Worker;

public class WorkerDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public string? ThirdName { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string? Phone { get; set; }
    public WorkerPositionDto WorkerPosition { get; set; } = null!;
    public int WorkerPositionId { get; set; }
    public bool CanManageWorkers { get; set; }
    public bool CanManageProjects { get; set; }
    public WorkerStatusDto WorkerStatus { get; set; } = null!;
}

public class WorkerStatusDto
{
    public int Id { get; set; }
    public string Name { get; set; } = WorkerStatus.Active.ToString();
}