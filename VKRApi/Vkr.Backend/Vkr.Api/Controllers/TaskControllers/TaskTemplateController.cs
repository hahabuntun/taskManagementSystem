using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Vkr.Application.Interfaces.Services;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.Entities.Task;

namespace Vkr.WebApi.Controllers;

[ApiController]
[Route("api/task-templates")]
public class TaskTemplateController : ControllerBase
{
    private readonly ITaskTemplateService _service;

    public TaskTemplateController(ITaskTemplateService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateTaskTemplateDTO templateDto)
    {
        try
        {
            var template = await _service.CreateTemplateAsync(templateDto);
            return Ok(MapToDTO(template));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{templateId}")]
    public async Task<IActionResult> UpdateTemplate(int templateId, [FromBody] UpdateTaskTemplateDTO templateDto)
    {
        try
        {
            var template = await _service.UpdateTemplateAsync(templateId, templateDto);
            return Ok(MapToDTO(template));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{templateId}")]
    public async Task<IActionResult> DeleteTemplate(int templateId)
    {
        try
        {
            await _service.DeleteTemplateAsync(templateId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{templateId}")]
    public async Task<IActionResult> GetTemplateById(int templateId)
    {
        try
        {
            var template = await _service.GetTemplateByIdAsync(templateId);
            return Ok(template);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTemplates()
    {
        try
        {
            var templates = await _service.GetAllTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{templateId}/available-tags")]
    public async Task<IActionResult> GetAvailableTags(int templateId)
    {
        try
        {
            var tags = await _service.GetAvailableTags(templateId);
            return Ok(tags);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("all-tags")]
    public async Task<IActionResult> GetAllTemplateTags()
    {
        try
        {
            var tags = await _service.GetAllTags();
            return Ok(tags);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
}