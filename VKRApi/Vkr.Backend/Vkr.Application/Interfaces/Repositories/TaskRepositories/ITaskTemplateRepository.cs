using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces.Repositories.TaskTemplateRepositories;

public interface ITaskTemplateRepository
{
    Task<TaskTemplates> CreateTemplateAsync(TaskTemplates template);
    Task<TaskTemplates> UpdateTemplateAsync(int templateId, TaskTemplates template);
    Task DeleteTemplateAsync(int templateId);
    Task<TaskTemplates> GetTemplateByIdAsync(int templateId);
    Task<List<TaskTemplates>> GetAllTemplatesAsync();
}