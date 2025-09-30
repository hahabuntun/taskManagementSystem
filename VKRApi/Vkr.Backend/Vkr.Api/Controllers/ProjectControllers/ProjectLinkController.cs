using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Interfaces;
using Vkr.Domain.Entities.Progect;

namespace Vkr.Web.Controllers;

[Route("api/projects/{projectId}/links")]
[ApiController]
public class ProjectLinkController : ControllerBase
{
    private readonly IProjectLinkService _projectLinkService;

    public ProjectLinkController(IProjectLinkService projectLinkService)
    {
        _projectLinkService = projectLinkService ?? throw new ArgumentNullException(nameof(projectLinkService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectLink>>> GetLinks(int projectId)
    {
        try
        {
            var links = await _projectLinkService.GetLinksByProjectIdAsync(projectId);
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
    public async Task<ActionResult<int>> AddLink(int projectId, [FromBody] AddProjectLinkRequest request)
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            var linkId = await _projectLinkService.AddLinkAsync(projectId, request.Link, request.Description, creatorId);
            return CreatedAtAction(nameof(GetLinks), new { projectId }, new { LinkId = linkId });
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

    [HttpPut("{linkId}")]
    public async Task<ActionResult> UpdateLink(int projectId, int linkId, [FromBody] UpdateProjectLinkRequest request)
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            var updated = await _projectLinkService.UpdateLinkAsync(linkId, request.Link, request.Description, creatorId);
            if (!updated)
                return NotFound("Link not found.");

            return NoContent();
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

    [HttpDelete("{linkId}")]
    public async Task<ActionResult> DeleteLink(int projectId, int linkId)
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            var deleted = await _projectLinkService.DeleteLinkAsync(linkId, creatorId);
            if (!deleted)
                return NotFound("Link not found.");

            return NoContent();
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
}

public class AddProjectLinkRequest
{
    public string Link { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateProjectLinkRequest
{
    public string Link { get; set; } = null!;
    public string? Description { get; set; }
}