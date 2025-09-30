using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.DTO;

public class ProjectDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty;
    public int ManagerId { get; set; }
    public WorkerDTO Manager { get; set; } = null!;
    public int Progress { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? ProjectStatusId { get; set; }
    public string ProjectStatusName { get; set; } = string.Empty;
    public List<WorkerDTO> Workers { get; set; } = new List<WorkerDTO>();
    public List<ProjectChecklist> ProjectChecklists { get; set; } = new List<ProjectChecklist>();
    public List<Tags> Tags { get; set; } = new List<Tags>();
}