using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Interfaces;
using Vkr.Domain.Entities.Worker;

namespace Vkr.WebApi.Controllers.ProjectControllers;

[ApiController]
[Route("api/project-member-management")]
public class ProjectMemberManagementController : ControllerBase
{
    private readonly IProjectMemberManagementService _service;

    public ProjectMemberManagementController(IProjectMemberManagementService service)
    {
        _service = service;
    }

    [HttpGet("{projectId}/members")]
    public async Task<ActionResult<IEnumerable<Workers>>> GetAllMembers(int projectId)
    {
        var members = await _service.GetAllMembersAsync(projectId);
        if (!members.Any())
            return Ok(new List<Workers>());
        return Ok(members);
    }

    [HttpGet("{projectId}/members/{memberId}")]
    public async Task<ActionResult<Workers>> GetMember(int projectId, int memberId)
    {
        var member = await _service.GetMemberAsync(projectId, memberId);
        return Ok(member);
    }

    [HttpPost("{projectId}/members")]
    public async Task<ActionResult> AddMemberToProject([FromRoute] int projectId, [FromBody] AddMemberRequest req)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var creatorId = int.Parse(userIdClaim);
        var result = await _service.AddMemberAsync(projectId, req.WorkerId, creatorId);
        if (!result)
            return BadRequest("Failed to add member to project");
        return Ok();
    }

    [HttpDelete("{projectId}/members/{memberId}")]
    public async Task<ActionResult> RemoveMemberFromProject([FromRoute] int projectId, [FromRoute] int memberId)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var creatorId = int.Parse(userIdClaim);
        var result = await _service.RemoveMemberAsync(projectId, memberId, creatorId);
        if (!result)
            return BadRequest("Failed to remove member from project");
        return Ok();
    }

    [HttpGet("{projectId}/members/{memberId}/subordinates")]
    public async Task<ActionResult<IEnumerable<Workers>>> GetMemberSubordinates(int projectId, int memberId)
    {
        var subordinates = await _service.GetMemberSubordinatesAsync(projectId, memberId);
        if (!subordinates.Any())
            return NotFound();
        return Ok(subordinates);
    }

    [HttpGet("{projectId}/members/{memberId}/directors")]
    public async Task<ActionResult<IEnumerable<Workers>>> GetMemberDirectors(int projectId, int memberId)
    {
        var directors = await _service.GetMemberDirectors(projectId, memberId);
        if (!directors.Any())
            return NotFound();
        return Ok(directors);
    }

    [HttpPost("{projectId}/members/{memberId}/subordinates")]
    public async Task<ActionResult> AddSubordinateToMember([FromRoute] int projectId, [FromRoute] int memberId, [FromBody] AddSubRequest req)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var creatorId = int.Parse(userIdClaim);
        var result = await _service.AddSubordinateToMember(projectId, memberId, req.WorkerId, creatorId);
        if (!result)
            return BadRequest("Failed to add subordinate or relation already exists");
        return Ok();
    }

    [HttpDelete("{projectId}/members/{memberId}/subordinates/{subordinateId}")]
    public async Task<ActionResult> RemoveSubordinateFromMember(int projectId, int memberId, int subordinateId)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var creatorId = int.Parse(userIdClaim);
        var result = await _service.RemoveSubordinateFromMember(projectId, memberId, subordinateId, creatorId);
        if (!result)
            return NotFound("Relation not found");
        return Ok();
    }
}

public record AddMemberRequest(int WorkerId);
public record AddSubRequest(int WorkerId);