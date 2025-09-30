using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vkr.Application.Interfaces;
using Vkr.Domain.Entities.Task;

namespace Vkr.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/tasks/{taskId}/links")]
public class TaskLinkController : ControllerBase
{
    private readonly ITaskLinkService _taskLinkService;

    public TaskLinkController(ITaskLinkService taskLinkService)
    {
        _taskLinkService = taskLinkService ?? throw new ArgumentNullException(nameof(taskLinkService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskLink>>> GetLinks(int taskId)
    {
        try
        {
            var links = await _taskLinkService.GetLinksByTaskIdAsync(taskId);
            return Ok(links);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> AddLink(int taskId, [FromBody] AddTaskLinkRequest request)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            var linkId = await _taskLinkService.AddLinkAsync(taskId, request.Link, request.Description, creatorId);
            return CreatedAtAction(nameof(GetLinks), new { taskId }, new { LinkId = linkId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{linkId}")]
    public async Task<ActionResult> UpdateLink(int taskId, int linkId, [FromBody] UpdateTaskLinkRequest request)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            var updated = await _taskLinkService.UpdateLinkAsync(linkId, request.Link, request.Description, creatorId);
            if (!updated)
                return NotFound("Link not found.");
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{linkId}")]
    public async Task<ActionResult> DeleteLink(int taskId, int linkId)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            var deleted = await _taskLinkService.DeleteLinkAsync(linkId, creatorId);
            if (!deleted)
                return NotFound("Link not found.");
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error.");
        }
    }
}
public class AddTaskLinkRequest
{
    public string Link { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateTaskLinkRequest
{
    public string Link { get; set; } = null!;
    public string? Description { get; set; }
}