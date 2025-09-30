using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces.Services;

public interface ITaskTemplateService
{
    Task<TaskTemplates> CreateTemplateAsync(CreateTaskTemplateDTO templateDto);
    Task<TaskTemplates> UpdateTemplateAsync(int templateId, UpdateTaskTemplateDTO templateDto);
    Task DeleteTemplateAsync(int templateId);
    Task<TaskTemplateDTO> GetTemplateByIdAsync(int templateId);
    Task<List<TaskTemplateDTO>> GetAllTemplatesAsync();
    Task<List<FullTagDTO>> GetAvailableTags(int templateId);
    Task<List<FullTagDTO>> GetAllTags();
}