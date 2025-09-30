using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Filters;
using Vkr.Application.Interfaces.Services.JournalServices;
using Vkr.Application.Interfaces.Services.ProjectServices;
using Vkr.Domain.DTO.Project;
using Vkr.Domain.Entities.Progect;

namespace Vkr.API.Controllers.ProjectControllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IAuditService _auditService;

    public ProjectController(IProjectService projectService, IAuditService auditService)
    {
        _projectService = projectService;
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
            return NotFound();
        return Ok(project);
    }

    [HttpGet("organization/{organizationId:int}")]
    public async Task<IActionResult> GetProjectsByOrganization(int organizationId)
    {
        var projects = await _projectService.GetProjectsByOrganizationAsync(organizationId);
        return Ok(projects);
    }

    [HttpGet("status/{statusId:int}")]
    public async Task<IActionResult> GetProjectsByStatus(int statusId)
    {
        var projects = await _projectService.GetProjectsByStatusAsync(statusId);
        return Ok(projects);
    }

    [HttpGet("manager/{managerId:int}")]
    public async Task<IActionResult> GetProjectsByManager(int managerId)
    {
        var projects = await _projectService.GetProjectsByManagerAsync(managerId);
        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDTO projectDto)
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            var projectId = await _projectService.CreateProjectAsync(projectDto, creatorId);
            return Ok(projectId);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectUpdateDto projectDto)
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            await _projectService.UpdateProjectAsync(id, projectDto, creatorId);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var creatorId = int.Parse(userIdClaim);
            await _projectService.DeleteProjectAsync(id, creatorId);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetProjectsByFilter([FromQuery] ProjectsFilter filter)
    {
        var projects = await _projectService.GetProjectsByFilterAsync(filter);
        return Ok(projects);
    }

    [HttpGet("{projectId}/available-tags")]
    public async Task<IActionResult> GetAvailableTags(int projectId)
    {
        var tags = await _projectService.GetAvailableTags(projectId);
        return Ok(tags);
    }

    [HttpGet("all-tags")]
    public async Task<IActionResult> GetAllProjectTags()
    {
        var tags = await _projectService.GetAllTags();
        return Ok(tags);
    }
}