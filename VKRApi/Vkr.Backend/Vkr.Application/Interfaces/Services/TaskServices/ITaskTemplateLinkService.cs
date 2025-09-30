using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces
{
    public interface ITaskTemplateLinkService
    {

        Task<int> AddLinkAsync(int taskTemplateId, string link, string? description);


        Task<bool> UpdateLinkAsync(int linkId, string link, string? description);


        Task<bool> DeleteLinkAsync(int linkId);


        Task<IEnumerable<TaskTemplateLink>> GetLinksByTaskTemplateIdAsync(int taskTemplateId);
    }
}