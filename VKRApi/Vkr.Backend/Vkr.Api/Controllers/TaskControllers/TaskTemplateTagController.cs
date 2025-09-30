using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vkr.Application.Interfaces.Services;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.Entities;

namespace Vkr.API.Controllers.TagControllers;

[ApiController]
[Route("api/task-templates/{templateId}/tags")]
public class TaskTemplateTagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TaskTemplateTagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    /// <summary>
    /// Adds an existing tag to a task template by tag ID.
    /// </summary>
    /// <param name="templateId">The ID of the task template.</param>
    /// <param name="tagDTO">The tag ID to add.</param>
    /// <returns>OK if successful.</returns>
    [HttpPost("add-existing")]
    public async Task<IActionResult> AddExistingTag(int templateId, [FromBody] AddTagDTO tagDTO)
    {
        if (tagDTO == null || tagDTO.TagId <= 0)
            return BadRequest("Invalid tag ID.");

        try
        {
            await _tagService.AddExistingTagToTemplateAsync(templateId, tagDTO.TagId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Adds a new tag to a task template by name and color.
    /// </summary>
    /// <param name="templateId">The ID of the task template.</param>
    /// <param name="createTagDTO">The name and color of the new tag.</param>
    /// <returns>The created tag.</returns>
    [HttpPost("add-new")]
    public async Task<IActionResult> AddNewTag(int templateId, [FromBody] CreateTagDTO createTagDTO)
    {
        if (createTagDTO == null || string.IsNullOrWhiteSpace(createTagDTO.Name) || string.IsNullOrWhiteSpace(createTagDTO.Color))
            return BadRequest("Tag name and color are required.");

        try
        {
            var newTag = await _tagService.AddNewTagToTemplateAsync(templateId, createTagDTO.Name, createTagDTO.Color);
            return Ok(new FullTagDTO { Id = newTag.Id, Name = newTag.Name, Color = newTag.Color });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a tag from a task template.
    /// </summary>
    /// <param name="templateId">The ID of the task template.</param>
    /// <param name="tagId">The ID of the tag to remove.</param>
    /// <returns>OK if successful.</returns>
    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTag(int templateId, int tagId)
    {
        try
        {
            await _tagService.DeleteTagFromTemplateAsync(templateId, tagId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets all tags associated with a task template.
    /// </summary>
    /// <param name="templateId">The ID of the task template.</param>
    /// <returns>List of tags.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllTags(int templateId)
    {
        try
        {
            var tags = await _tagService.GetAllTemplateTagsAsync(templateId);
            var result = tags.Select(t => new FullTagDTO { Id = t.Id, Name = t.Name, Color = t.Color }).ToList();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Adds a combination of existing and new tags to a task template.
    /// </summary>
    /// <param name="templateId">The ID of the task template.</param>
    /// <param name="addTagsDTO">Lists of existing tag IDs and new tags with names and colors.</param>
    /// <returns>OK if successful.</returns>
    [HttpPost("add-tags")]
    public async Task<IActionResult> AddTags(int templateId, [FromBody] AddTagsDTO addTagsDTO)
    {
        if (addTagsDTO == null || (addTagsDTO.ExistingTagIds.Count == 0 && addTagsDTO.NewTags.Count == 0))
            return BadRequest("No tags provided.");

        try
        {
            var newTags = addTagsDTO.NewTags.Select(t => (t.Name, t.Color)).ToList();
            await _tagService.AddTagsToTemplateAsync(templateId, addTagsDTO.ExistingTagIds, newTags);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}