
using Vkr.Domain.Entities.Worker;

public class UpdateWorkerRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public string? ThirdName { get; set; }
    public string Email { get; set; } = string.Empty;
    public int WorkerPosition { get; set; }
    public int WorkerStatus { get; set; }
    public string? Password { get; set; }
    public bool CanManageWorkers { get; set; }
    public bool CanManageProjects { get; set; }
    public string? Phone { get; set; }
}