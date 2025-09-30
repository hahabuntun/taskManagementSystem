namespace Vkr.Domain.DTO.Worker;

public class WorkerPositionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<WorkerPositionSummary> TaskGivers { get; set; } // Id and Title
    public List<WorkerPositionSummary> TaskTakers { get; set; } // Id and Title
}

public class WorkerPositionSummary
{
    public int Id { get; set; }
    public string Title { get; set; }
}