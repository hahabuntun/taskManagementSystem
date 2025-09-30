namespace Vkr.Domain.Entities.Progect
{
    public class ProjectFile
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; }
        public int FileSize { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ProjectId { get; set; }
        public ProjectStatus Project { get; set; }
        public int CreatorId { get; set; }
        public Worker.Workers Creator { get; set; }
    }
}