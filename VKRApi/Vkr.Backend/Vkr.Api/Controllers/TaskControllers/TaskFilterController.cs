using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Interfaces.Services;
using Vkr.Domain.DTO.Task;

namespace Vkr.API.Controllers;

[ApiController]
[Route("api/task-filters")]
public class TaskFilterController : ControllerBase
{
    private readonly ITaskFilterService _taskFilterService;

    public TaskFilterController(ITaskFilterService taskFilterService)
    {
        _taskFilterService = taskFilterService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskFilterDTO>>> GetTaskFilters()
    {
        var filters = await _taskFilterService.GetTaskFiltersAsync();
        return Ok(filters);
    }

    [HttpPost]
    public async Task<IActionResult> AddTaskFilter([FromBody] AddTaskFilterRequest request)
    {
        await _taskFilterService.AddTaskFilterAsync(request.Name, request.Options);
        return NoContent();
    }

    [HttpPut("{name}")]
    public async Task<IActionResult> EditTaskFilter(string name, [FromBody] EditTaskFilterRequest request)
    {
        await _taskFilterService.EditTaskFilterAsync(name, request.Options);
        return NoContent();
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> RemoveTaskFilter(string name)
    {
        await _taskFilterService.RemoveTaskFilterAsync(name);
        return NoContent();
    }
}

public class AddTaskFilterRequest
{
    public string Name { get; set; } = string.Empty;
    public TaskFilterOptionsDTO Options { get; set; } = new TaskFilterOptionsDTO();
}

public class EditTaskFilterRequest
{
    public TaskFilterOptionsDTO Options { get; set; } = new TaskFilterOptionsDTO();
}