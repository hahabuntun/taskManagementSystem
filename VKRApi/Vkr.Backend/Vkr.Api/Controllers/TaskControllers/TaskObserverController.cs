using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vkr.Application.Interfaces;
using Vkr.Application.Services.TaskObserverServices;
using Vkr.Domain.DTO.Worker;

namespace Vkr.API.Controllers.TaskControllers;

[ApiController]
[Authorize]
[Route("api/tasks/{taskId:int}/observers")]
public class TaskObserverController : ControllerBase
{
    private readonly ITaskObserverService _observerService;

    public TaskObserverController(ITaskObserverService observerService)
    {
        _observerService = observerService ?? throw new ArgumentNullException(nameof(observerService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkerDTO>>> GetObservers(int taskId)
    {
        try
        {
            var observers = await _observerService.GetObserversAsync(taskId);
            return Ok(observers);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddObserver(int taskId, [FromBody] AddObserverRequest request)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _observerService.AddObserverAsync(taskId, request.WorkerId, creatorId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
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

    [HttpPost("bulk")]
    public async Task<IActionResult> AddObservers(int taskId, [FromBody] AddObserversRequest request)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _observerService.AddObserversAsync(taskId, request.WorkerIds, creatorId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
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

    [HttpDelete("{workerId:int}")]
    public async Task<IActionResult> RemoveObserver(int taskId, int workerId)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _observerService.RemoveObserverAsync(taskId, workerId, creatorId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
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
public class AddObserverRequest
{
    public int WorkerId { get; set; }
}

public class AddObserversRequest
{
    public int[] WorkerIds { get; set; } = Array.Empty<int>();
}