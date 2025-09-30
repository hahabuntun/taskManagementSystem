using Vkr.API.Contracts.WorkerPositionsControllerContracts;

namespace Vkr.API.Contracts.WorkersControllerContracts
{
    public class WorkerResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Email { get; set; }
        public bool CanManageWorkers { get; set; }
        public bool CanManageProjects { get; set; }
        public WorkerStatusResponse WorkerStatus { get; set; }
        public WorkerPositionResponse? WorkerPosition { get; set; }
    }
}
