using Vkr.Application.Filters;
using Vkr.Domain.Entities.Worker;
using Task = System.Threading.Tasks.Task;

namespace Vkr.Application.Interfaces.Services.WorkerServices;

public interface IWorkersService
{
    Task<Workers> CreateWorker(Workers workers, string password, int creatorId);
    Task<bool> DeleteWorkerAsync(int id, int creatorId);
    Task<Workers> GetWorkerByIdAsync(int id);
    Task<List<Workers>> GetWorkersByFilterAsync(WorkersFilter filter);
    Task<bool> IsEmailUnique(string candidateEmail);
    Task<bool> IsEmailUniqueForUpdate(int workerId, string email);
    Task<string> Login(string email, string password);
    Task<Workers> UpdateWorkerAsync(int id, Workers workers, int creatorId);
}