using Vkr.Domain.DTO.Worker;

namespace Vkr.API.Contracts.WorkerPositionsControllerContracts
{
    public class WorkerPositionResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<WorkerPositionSummary> TaskGivers { get; set; } // Positions that assign tasks to this position
        public List<WorkerPositionSummary> TaskTakers { get; set; } // Positions this position assigns tasks to
    }

}