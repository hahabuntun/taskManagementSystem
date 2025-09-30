using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vkr.Application.Interfaces;
using Vkr.Application.Services.TaskExecutorServices;
using Vkr.Domain.DTO.Worker;

namespace Vkr.API.Controllers.TaskControllers;

[ApiController]
[Authorize]
[Route("api/tasks/{taskId:int}/executors")]
public class TaskExecutorController : ControllerBase
{
    private readonly ITaskExecutorService _executorService;

    public TaskExecutorController(ITaskExecutorService executorService)
    {
        _executorService = executorService ?? throw new ArgumentNullException(nameof(executorService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkerDTO>>> GetExecutors(int taskId)
    {
        try
        {
            var executors = await _executorService.GetExecutorsAsync(taskId);
            return Ok(executors);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddExecutor(int taskId, [FromBody] AddExecutorRequest request)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _executorService.AddExecutorAsync(taskId, request.WorkerId, request.IsResponsible, creatorId);
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
    public async Task<IActionResult> AddExecutors(int taskId, [FromBody] AddExecutorsRequest request)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _executorService.AddExecutorsAsync(taskId, request.WorkerIds, request.ResponsibleWorkerId, creatorId);
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
    public async Task<IActionResult> RemoveExecutor(int taskId, int workerId)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _executorService.RemoveExecutorAsync(taskId, workerId, creatorId);
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

    [HttpPatch("responsible")]
    public async Task<IActionResult> UpdateResponsibleExecutor(int taskId, [FromBody] UpdateResponsibleExecutorRequest request)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _executorService.UpdateResponsibleExecutorAsync(taskId, request.WorkerId, creatorId);
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

public class AddExecutorRequest
{
    public int WorkerId { get; set; }
    public bool IsResponsible { get; set; }
}

public class AddExecutorsRequest
{
    public int[] WorkerIds { get; set; } = Array.Empty<int>();
    public int? ResponsibleWorkerId { get; set; }
}

public class UpdateResponsibleExecutorRequest
{
    public int? WorkerId { get; set; }
}