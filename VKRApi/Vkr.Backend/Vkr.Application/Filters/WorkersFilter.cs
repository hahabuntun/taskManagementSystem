namespace Vkr.Application.Filters
{
    public class WorkersFilter
    {

        public string? Name { get; set; }
        public bool? CanManageWorkers { get; set; }

        public bool? CanManageProjects { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string? Email { get; set; }
    }
}
