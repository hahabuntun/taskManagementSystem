using Microsoft.AspNetCore.Mvc;
using Vkr.API.Controllers.JournalControllers;
using Vkr.Application.Interfaces.Services.JournalServices;
using Vkr.Application.Interfaces.Services.SprintServices;
using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.Entities.Sprint;
using static Vkr.API.Utils.Response;

namespace Vkr.API.Controllers.SprintControllers;

[ApiController]
[Route("api/sprints")]
public class SprintController : BaseAuditController<Sprints>
{
    private readonly ISprintService _service;

    public SprintController(ISprintService service, IAuditService auditService)
        : base(auditService)
    {
        _service = service;
    }

    /// <summary>
    /// Create a new sprint
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] SprintCreateDTO dto)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        var id = await _service.CreateAsync(dto, userId);
        return Success(new { id });
    }

    /// <summary>
    /// Delete a sprint by ID
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("User ID not found in token.");

        var userId = int.Parse(userIdClaim);
        await _service.DeleteAsync(id, userId);
        return Success();
    }

    /// <summary>
    /// Update sprint details
    /// </summary>
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] SprintUpdateDTO dto)
    {
        await _service.UpdateAsync(dto);
        return Success();
    }

    /// <summary>
    /// Get a sprint by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var sprint = await _service.GetByIdAsync(id);
        return Success(sprint);
    }

    /// <summary>
    /// Get all sprints for a project
    /// </summary>
    [HttpGet("project/{projectId:int}")]
    public async Task<ActionResult> GetByProject(int projectId)
    {
        var sprints = await _service.GetByProjectAsync(projectId);
        return Success(sprints);
    }

    /// <summary>
    /// Get all sprints for a worker
    /// </summary>
    [HttpGet("worker/{workerId:int}")]
    public async Task<ActionResult> GetByWorker(int workerId)
    {
        var sprints = await _service.GetByWorkerAsync(workerId);
        return Success(sprints);
    }
}