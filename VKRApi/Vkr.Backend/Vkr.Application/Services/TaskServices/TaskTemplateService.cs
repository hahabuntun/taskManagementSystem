using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vkr.Application.Interfaces.Repositories.TaskTemplateRepositories;
using Vkr.Application.Interfaces.Repositories.TagRepositories;
using Vkr.Application.Interfaces.Services;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Services;

public class TaskTemplateService : ITaskTemplateService
{
    private readonly ITaskTemplateRepository _repository;
    private readonly ITagRepository _tagRepository;

    public TaskTemplateService(ITaskTemplateRepository repository, ITagRepository tagRepository)
    {
        _repository = repository;
        _tagRepository = tagRepository;
    }

    private TaskTemplateDTO MapToDTO(TaskTemplates template)
    {
        return new TaskTemplateDTO
        {
            Id = template.Id,
            TemplateName = template.TemplateName,
            TaskName = template.TaskName,
            Description = template.Description,
            TaskStatus = template.TaskStatus != null ? new TaskStatusDTO
            {
                Id = template.TaskStatus.Id,
                Name = template.TaskStatus.Name,
                Color = template.TaskStatus.Color
            } : null,
            TaskPriority = template.TaskPriority != null ? new TaskPriorityDTO
            {
                Id = template.TaskPriority.Id,
                Name = template.TaskPriority.Name,
                Color = template.TaskPriority.Color
            } : null,
            TaskType = template.TaskType != null ? new TaskTypeDTO
            {
                Id = template.TaskType.Id,
                Name = template.TaskType.Name
            } : null,
            StartDate = template.StartDate,
            EndDate = template.EndDate,
            CreatedOn = template.CreatedOn,
            Progress = template.Progress,
            StoryPoints = template.StoryPoints,
            Tags = template.Tags?.Select(t => new FullTagDTO
            {
                Id = t.Id,
                Name = t.Name,
                Color = t.Color
            }).ToList() ?? new List<FullTagDTO>()
        };
    }

    public async Task<TaskTemplates> CreateTemplateAsync(CreateTaskTemplateDTO templateDto)
    {
        if (string.IsNullOrWhiteSpace(templateDto.TemplateName))
            throw new ArgumentException("Template name is required");

        var template = new TaskTemplates
        {
            TemplateName = templateDto.TemplateName,
            TaskName = templateDto.TaskName,
            Description = templateDto.Description,
            TaskStatusId = templateDto.TaskStatusId,
            TaskPriorityId = templateDto.TaskPriorityId,
            TaskTypeId = templateDto.TaskTypeId,
            StartDate = templateDto.StartDate,
            EndDate = templateDto.EndDate,
            Progress = templateDto.Progress,
            StoryPoints = templateDto.StoryPoints,
            CreatedOn = DateTime.UtcNow,
            Tags = templateDto.TagIds != null
                ? templateDto.TagIds.Select(id => new Tags { Id = id }).ToList()
                : new List<Tags>(),
            TaskTempateLinks = templateDto.Links != null
                ? templateDto.Links.Select(link => new TaskTemplateLink
                {
                    Link = link.Link,
                    Description = link.Description,
                    CreatedOn = DateTime.UtcNow
                }).ToList()
                : new List<TaskTemplateLink>()
        };

        var createdTemplate = await _repository.CreateTemplateAsync(template);
        return createdTemplate;
    }

    public async Task<TaskTemplates> UpdateTemplateAsync(int templateId, UpdateTaskTemplateDTO templateDto)
    {
        if (string.IsNullOrWhiteSpace(templateDto.TemplateName))
            throw new ArgumentException("Template name is required");

        var template = new TaskTemplates
        {
            Id = templateId,
            TemplateName = templateDto.TemplateName,
            TaskName = templateDto.TaskName,
            Description = templateDto.Description,
            TaskStatusId = templateDto.TaskStatusId,
            TaskPriorityId = templateDto.TaskPriorityId,
            TaskTypeId = templateDto.TaskTypeId,
            StartDate = templateDto.StartDate,
            EndDate = templateDto.EndDate,
            Progress = templateDto.Progress,
            StoryPoints = templateDto.StoryPoints,
        };

        var updatedTemplate = await _repository.UpdateTemplateAsync(templateId, template);
        return updatedTemplate;
    }

    public async Task DeleteTemplateAsync(int templateId)
    {
        await _repository.DeleteTemplateAsync(templateId);
    }

    public async Task<TaskTemplateDTO> GetTemplateByIdAsync(int templateId)
    {
        var template = await _repository.GetTemplateByIdAsync(templateId);
        return MapToDTO(template);
    }

    public async Task<List<TaskTemplateDTO>> GetAllTemplatesAsync()
    {
        var templates = await _repository.GetAllTemplatesAsync();
        return templates.Select(MapToDTO).ToList();
    }

    public async Task<List<FullTagDTO>> GetAvailableTags(int templateId)
    {
        var tags = await _tagRepository.GetAvailableTagsForTemplateAsync(templateId);
        return tags.Select(t => new FullTagDTO
        {
            Id = t.Id,
            Name = t.Name,
            Color = t.Color
        }).ToList();
    }

    public async Task<List<FullTagDTO>> GetAllTags()
    {
        var tags = await _tagRepository.GetAllTagsForTemplatesAsync();
        return tags.Select(t => new FullTagDTO
        {
            Id = t.Id,
            Name = t.Name,
            Color = t.Color
        }).ToList();
    }
}