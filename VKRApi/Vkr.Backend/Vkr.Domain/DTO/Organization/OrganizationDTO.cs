using Vkr.Domain.DTO.Worker;

namespace Vkr.Domain.DTO;

public class OrganizationDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public WorkerDTO Owner { get; set; } = null!;
    public DateTime? CreatedOn { get; set; }
}