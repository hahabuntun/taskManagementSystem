using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vkr.Application.Interfaces.Services.TaskServices;
using Vkr.Domain.DTO.Task;

namespace Vkr.API.Controllers.TaskControllers;

[ApiController]
[Authorize]
[Route("api/task-messages")]
public class TaskMessageController : ControllerBase
{
    private readonly ITaskMessageService _taskMessageService;

    public TaskMessageController(ITaskMessageService taskMessageService)
    {
        _taskMessageService = taskMessageService ?? throw new ArgumentNullException(nameof(taskMessageService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskMessageDTO>>> GetMessages(int taskId)
    {
        try
        {
            var messages = await _taskMessageService.GetMessagesByTaskAsync(taskId);
            return Ok(messages);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetMessageCount(int taskId)
    {
        try
        {
            var count = await _taskMessageService.GetMessageCountByTaskAsync(taskId);
            return Ok(count);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<TaskMessageDTO>> CreateMessage([FromBody] TaskMessageCreateDTO dto)
    {
        try
        {
            var senderId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            dto.SenderId = senderId;
            var message = await _taskMessageService.CreateMessageAsync(dto);
            return CreatedAtAction(nameof(GetMessages), new { taskId = dto.RelatedTaskId }, message);
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

    [HttpPut("{messageId}")]
    public async Task<IActionResult> UpdateMessage(int messageId, [FromBody] string newText)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _taskMessageService.UpdateMessageAsync(messageId, newText, creatorId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
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

    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessage(int messageId)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _taskMessageService.DeleteMessageAsync(messageId, creatorId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
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