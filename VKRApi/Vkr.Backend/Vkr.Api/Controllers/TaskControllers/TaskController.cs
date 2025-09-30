using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vkr.API.Contracts.TaskControllerContracts;
using Vkr.API.Controllers.JournalControllers;
using Vkr.Application.Interfaces.Services.JournalServices;
using Vkr.Application.Interfaces.Services.TaskServices;
using Vkr.Domain.DTO.Task;
using static Vkr.API.Utils.Response;

namespace Vkr.API.Controllers.TaskControllers;

[ApiController]
[Authorize]
[Route("api/tasks")]
public class TaskController : BaseAuditController<Task>
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService, IAuditService auditService)
        : base(auditService)
    {
        _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return Success(task);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Success(tasks);
    }

    [HttpGet("sprint/{sprintId}")]
    public async Task<IActionResult> GetBySprint(int sprintId)
    {
        var tasks = await _taskService.GetTasksBySprintAsync(sprintId);
        return Success(tasks);
    }

    [HttpGet("assignee/{workerId}")]
    public async Task<IActionResult> GetByAssignee(int workerId)
    {
        try
        {
            var tasks = await _taskService.GetTasksByAssigneeAsync(workerId);
            return Success(tasks);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            var options = new CreateTaskDTO
            {
                Name = request.Name,
                Description = request.Description,
                Progress = request.Progress,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ProjectId = request.ProjectId,
                CreatorId = creatorId,
                TaskTypeId = request.TaskTypeId,
                TaskStatusId = request.TaskStatusId,
                TaskPriorityId = request.TaskPriorityId,
                SprintId = request.SprintId,
                ExistingTagIds = request.ExistingTagIds,
                NewTags = request.NewTags,
                Links = request.Links,
                StoryPoints = request.StoryPoints,
            };

            var taskId = await _taskService.CreateTaskAsync(options);
            return CreatedAtAction(nameof(GetById), new { id = taskId }, new { TaskId = taskId });
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request)
    {

        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            var options = new CreateTaskDTO
            {
                Name = request.Name,
                Description = request.Description,
                StoryPoints = request.StoryPoints,
                Progress = request.Progress,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TaskTypeId = request.TaskTypeId,
                TaskStatusId = request.TaskStatusId,
                TaskPriorityId = request.TaskPriorityId,
                SprintId = request.SprintId,
            };

            await _taskService.UpdateTaskAsync(id, options, creatorId);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _taskService.DeleteTaskAsync(id, creatorId);
            return Success();
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
            return StatusCode(500, new { Error = "Internal server error", Details = ex });
        }
    }

    [HttpPost("{taskId}/relationships")]
    public async Task<IActionResult> AddTaskRelationship(int taskId, [FromBody] AddTaskRelationshipRequest request)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _taskService.AddTaskRelationshipAsync(taskId, request.RelatedTaskId, request.RelationshipTypeId, creatorId);
            return Success();
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

    [HttpDelete("relationships/{relationshipId}")]
    public async Task<IActionResult> RemoveTaskRelationship(int relationshipId)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _taskService.RemoveTaskRelationshipAsync(relationshipId, creatorId);
            return Success();
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

    [HttpDelete("{taskId}/relationships/{relatedTaskId}")]
    public async Task<IActionResult> RemoveTaskRelationshipByTaskIds(int taskId, int relatedTaskId)
    {
        try
        {
            var creatorId = int.Parse(User.FindFirst("userId")?.Value 
                ?? throw new InvalidOperationException("User ID not found in token."));
            await _taskService.RemoveTaskRelationshipByTaskIdsAsync(taskId, relatedTaskId, creatorId);
            return Success();
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

    [HttpGet("{taskId}/related")]
    public async Task<IActionResult> GetRelatedTasks(int taskId)
    {
        try
        {
            var relatedTasks = await _taskService.GetRelatedTasksAsync(taskId);
            return Success(relatedTasks);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
        }
    }

    [HttpGet("{taskId}/available-related")]
    public async Task<IActionResult> GetAvailableRelatedTasks(int taskId)
    {
        try
        {
            var availableTasks = await _taskService.GetAvailableRelatedTasksAsync(taskId);
            return Success(availableTasks);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Internal server error", Details = ex.Message });
        }
    }

    [HttpGet("{taskId}/available-tags")]
    public async Task<IActionResult> GetAvailableTags(int taskId)
    {
        var tags = await _taskService.GetAvailableTags(taskId);
        return Ok(tags);
    }

    [HttpGet("all-tags")]
    public async Task<IActionResult> GetAllTaskTags()
    {
        var tags = await _taskService.GetAllTags();
        return Ok(tags);
    }
}