namespace Vkr.API.Contracts.WorkersControllerContracts
{
    public record CreateWorkerRequest(
            string Name,
            string SecondName,
            string ThirdName,
            string Email,
            string Password,
            bool CanManageWorkers,
            bool CanManageProjects,
            int WorkerStatus,
            int WorkerPosition
        );

}
