namespace Vkr.Application.Filters;

public class ProjectsFilter
{
    public string? Name { get; set; }
    public int? StatusId { get; set; }
    public int? ManagerId { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTill { get; set; }
    public DateTime? EndDateFrom { get; set; }
    public DateTime? EndDateTill { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTill { get; set; }
}