using Vkr.Application.Interfaces;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Repositories;

namespace Vkr.Application.Services
{
    public class TaskTemplateLinkService : ITaskTemplateLinkService
    {
        private readonly ITaskTemplateLinkRepository _taskTemplateLinkRepository;

        public TaskTemplateLinkService(ITaskTemplateLinkRepository taskTemplateLinkRepository)
        {
            _taskTemplateLinkRepository = taskTemplateLinkRepository ?? throw new ArgumentNullException(nameof(taskTemplateLinkRepository));
        }

        public async Task<int> AddLinkAsync(int taskTemplateId, string link, string? description)
        {
            // Валидация входных данных
            if (taskTemplateId <= 0)
                throw new ArgumentException("Идентификатор проекта должен быть больше 0.", nameof(taskTemplateId));
            if (string.IsNullOrWhiteSpace(link))
                throw new ArgumentException("Ссылка не может быть пустой.", nameof(link));

            var taskTemplateLink = new TaskTemplateLink
            {
                TaskTemplateId = taskTemplateId,
                Link = link,
                Description = description,
                CreatedOn = DateTime.UtcNow
            };

            return await _taskTemplateLinkRepository.AddLinkAsync(taskTemplateLink);
        }

        public async Task<bool> UpdateLinkAsync(int linkId, string link, string? description)
        {
            if (linkId <= 0)
                throw new ArgumentException("Идентификатор ссылки должен быть больше 0.", nameof(linkId));
            if (string.IsNullOrWhiteSpace(link))
                throw new ArgumentException("Ссылка не может быть пустой.", nameof(link));

            return await _taskTemplateLinkRepository.UpdateLinkAsync(linkId, link, description);
        }

        public async Task<bool> DeleteLinkAsync(int linkId)
        {
            if (linkId <= 0)
                throw new ArgumentException("Идентификатор ссылки должен быть больше 0.", nameof(linkId));

            return await _taskTemplateLinkRepository.DeleteLinkAsync(linkId);
        }

        public async Task<IEnumerable<TaskTemplateLink>> GetLinksByTaskTemplateIdAsync(int taskTemplateId)
        {
            if (taskTemplateId <= 0)
                throw new ArgumentException("Идентификатор шаблона должен быть 1000.", nameof(taskTemplateId));

            return await _taskTemplateLinkRepository.GetLinksByTaskTemplateIdAsync(taskTemplateId);
        }
    }
}