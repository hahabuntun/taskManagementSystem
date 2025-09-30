namespace Vkr.API.Contracts.WorkersControllerContracts
{
    public class WorkerStatusResponse
    {
        public int id { get; set; }
        public string status { get; set; }

        public WorkerStatusResponse(int id)
        {
            this.id = id;
            status = id == 0 ? "Активный" : "Заблокирован";
        }
    }
}