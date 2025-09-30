using Vkr.Domain.Entities.Task;

namespace Vkr.Domain.Repositories
{
    public interface ITaskLinkRepository
    {

        Task<int> AddLinkAsync(TaskLink taskLink);

        Task<bool> UpdateLinkAsync(int linkId, string link, string? description);


        Task<bool> DeleteLinkAsync(int linkId);


        Task<IEnumerable<TaskLink>> GetLinksByTaskIdAsync(int taskId);

        Task<TaskLink?> GetLinkByIdAsync(int linkId);
    }
}