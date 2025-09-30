using Vkr.API.Contracts.WorkersControllerContracts;

namespace Vkr.API.Contracts.FilesContracts
{
    public class FileResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long Size { get; set; }
        public WorkerResponse User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
