using Vkr.Application.Filters;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Application.Interfaces.Repositories.WorkerRepositories;

public interface IWorkersRepository
{
    Task<Workers?> GetByIdAsync(int id);

    Task<List<Workers>> GetByFilterAsync(WorkersFilter filter);

    Task<Workers> CreateAsync(Workers workers);

    Task<Workers> UpdateAsync(int id, Workers workers);

    Task<bool> DeleteAsync(int id);

    Task<bool> IsEmailUnique(string candidateEmail);

    Task<Workers> GetByEmail(string email);

    Task<int[]> ValidateWorkerIdsAsync(int[] workerIds);
}