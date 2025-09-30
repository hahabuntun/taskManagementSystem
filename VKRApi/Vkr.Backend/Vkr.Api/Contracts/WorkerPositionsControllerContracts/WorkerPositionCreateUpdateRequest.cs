namespace Vkr.API.Contracts.WorkerPositionsControllerContracts
{
    public record WorkerPositionCreateUpdateRequest(string Title, int[] taskTakerIds, int[] taskGiverIds);

}
