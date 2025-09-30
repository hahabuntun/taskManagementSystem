using Vkr.Domain.DTO.Worker;

public class ProjectMemberDTO
{
    public WorkerDTO Worker { get; set; } = new WorkerDTO();
    public List<WorkerDTO> TaskGivers { get; set; } = new List<WorkerDTO>();
    public List<WorkerDTO> TaskTakers { get; set; } = new List<WorkerDTO>();
    public DateTime CreatedAt { get; set; }
}