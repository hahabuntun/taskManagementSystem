using Vkr.Domain.Entities.Task;

namespace Vkr.Domain.Repositories
{
    public interface ITaskTemplateLinkRepository
    {

        Task<int> AddLinkAsync(TaskTemplateLink taskTemplateLink);


        Task<bool> UpdateLinkAsync(int linkId, string link, string? description);


        Task<bool> DeleteLinkAsync(int linkId);


        Task<IEnumerable<TaskTemplateLink>> GetLinksByTaskTemplateIdAsync(int taskTemplateId);
    }
}