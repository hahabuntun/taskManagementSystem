using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Interfaces.Services;

namespace Vkr.Web.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("organization/{organizationId}")]
    public async Task<ActionResult<OrganizationAnalyticsData>> GetOrganizationAnalytics(int organizationId)
    {
        try
        {
            var analytics = await _analyticsService.GetOrganizationAnalyticsAsync(organizationId);
            return Ok(analytics);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("worker/{workerId}")]
    public async Task<ActionResult<WorkerAnalyticsData>> GetWorkerAnalytics(int workerId)
    {
        try
        {
            var analytics = await _analyticsService.GetWorkerAnalyticsAsync(workerId);
            return Ok(analytics);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("sprint/{sprintId}")]
    public async Task<ActionResult<SprintAnalyticsData>> GetSprintAnalytics(int sprintId)
    {
        try
        {
            var analytics = await _analyticsService.GetSprintAnalyticsAsync(sprintId);
            return Ok(analytics);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<ProjectAnalyticsData>> GetProjectAnalytics(int projectId)
    {
        try
        {
            var analytics = await _analyticsService.GetProjectAnalyticsAsync(projectId);
            return Ok(analytics);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}