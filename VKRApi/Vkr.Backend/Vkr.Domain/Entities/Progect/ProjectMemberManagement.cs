using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Progect;

public class ProjectMemberManagement
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public int ProjectId { get; set; }
    public int SubordinateId { get; set; }
    public Workers Worker { get; set; }
    public Workers Subordinate { get; set; }
    public Projects Project { get; set; }
}