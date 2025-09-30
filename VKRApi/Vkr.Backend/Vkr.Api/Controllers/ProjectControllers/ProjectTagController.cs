using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vkr.Application.Interfaces.Services;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.Entities;

namespace Vkr.API.Controllers.TagControllers;

[ApiController]
[Route("api/projects/{projectId}/tags")]
public class ProjectTagController : ControllerBase
{
    private readonly ITagService _tagService;

    public ProjectTagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpPost("add-existing")]
    public async Task<IActionResult> AddExistingTag(int projectId, [FromBody] AddTagDTO tagDTO)
    {
        if (tagDTO == null || tagDTO.TagId <= 0)
            return BadRequest("Invalid tag ID.");

        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            await _tagService.AddExistingTagToProjectAsync(projectId, tagDTO.TagId, creatorId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-new")]
    public async Task<IActionResult> AddNewTag(int projectId, [FromBody] CreateTagDTO createTagDTO)
    {
        if (createTagDTO == null || string.IsNullOrWhiteSpace(createTagDTO.Name) || string.IsNullOrWhiteSpace(createTagDTO.Color))
            return BadRequest("Tag name and color are required.");

        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            var newTag = await _tagService.AddNewTagToProjectAsync(projectId, createTagDTO.Name, createTagDTO.Color, creatorId);
            return Ok(new FullTagDTO { Id = newTag.Id, Name = newTag.Name, Color = newTag.Color });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTag(int projectId, int tagId)
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            await _tagService.DeleteTagFromProjectAsync(projectId, tagId, creatorId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTags(int projectId)
    {
        try
        {
            var tags = await _tagService.GetAllProjectTagsAsync(projectId);
            var result = tags.Select(t => new FullTagDTO { Id = t.Id, Name = t.Name, Color = t.Color }).ToList();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-tags")]
    public async Task<IActionResult> AddTags(int projectId, [FromBody] AddTagsDTO addTagsDTO)
    {
        if (addTagsDTO == null || (addTagsDTO.ExistingTagIds.Count == 0 && addTagsDTO.NewTags.Count == 0))
            return BadRequest("No tags provided.");

        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            var newTags = addTagsDTO.NewTags.Select(t => (t.Name, t.Color)).ToList();
            await _tagService.AddTagsToProjectAsync(projectId, addTagsDTO.ExistingTagIds, newTags, creatorId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}