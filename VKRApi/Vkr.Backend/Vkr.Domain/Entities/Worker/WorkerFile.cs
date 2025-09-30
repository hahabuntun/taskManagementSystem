
namespace Vkr.Domain.Entities.Worker
{
    public class WorkerFile
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; }
        public int Size { get; set; }
        public DateTime CreatedAt { get; set; }
        public Workers Creator { get; set; }
        public int WorkerId { get; set; }
        public Workers Workers { get; set; }
    }
}