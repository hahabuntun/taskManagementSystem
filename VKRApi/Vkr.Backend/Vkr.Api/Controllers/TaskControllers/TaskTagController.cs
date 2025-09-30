using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vkr.Application.Interfaces.Services;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.Entities;

namespace Vkr.API.Controllers.TagControllers;

[ApiController]
[Authorize]
[Route("api/tasks/{taskId}/tags")]
public class TaskTagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TaskTagController(ITagService tagService)
    {
        _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
    }

    [HttpPost("add-existing")]
    public async Task<IActionResult> AddExistingTag(int taskId, [FromBody] AddTagDTO tagDTO)
    {
        if (tagDTO == null || tagDTO.TagId <= 0)
            return BadRequest("Invalid tag ID.");

        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _tagService.AddExistingTagToTaskAsync(taskId, tagDTO.TagId, creatorId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
        }
    }

    [HttpPost("add-new")]
    public async Task<IActionResult> AddNewTag(int taskId, [FromBody] CreateTagDTO createTagDTO)
    {
        if (createTagDTO == null || string.IsNullOrWhiteSpace(createTagDTO.Name) || string.IsNullOrWhiteSpace(createTagDTO.Color))
            return BadRequest("Tag name and color are required.");

        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            var newTag = await _tagService.AddNewTagToTaskAsync(taskId, createTagDTO.Name, createTagDTO.Color, creatorId);
            return Ok(new FullTagDTO { Id = newTag.Id, Name = newTag.Name, Color = newTag.Color });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
        }
    }

    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTag(int taskId, int tagId)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _tagService.DeleteTagFromTaskAsync(taskId, tagId, creatorId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTags(int taskId)
    {
        try
        {
            var tags = await _tagService.GetAllTaskTagsAsync(taskId);
            var result = tags.Select(t => new FullTagDTO { Id = t.Id, Name = t.Name, Color = t.Color }).ToList();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-tags")]
    public async Task<IActionResult> AddTags(int taskId, [FromBody] AddTagsDTO addTagsDTO)
    {
        if (addTagsDTO == null || (addTagsDTO.ExistingTagIds.Count == 0 && addTagsDTO.NewTags.Count == 0))
            return BadRequest("No tags provided.");

        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            var newTags = addTagsDTO.NewTags.Select(t => (t.Name, t.Color)).ToList();
            await _tagService.AddTagsToTaskAsync(taskId, addTagsDTO.ExistingTagIds, newTags, creatorId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
        }
    }
}