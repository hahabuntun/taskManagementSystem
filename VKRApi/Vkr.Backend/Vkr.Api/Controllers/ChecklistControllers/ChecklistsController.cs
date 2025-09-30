using Microsoft.AspNetCore.Mvc;
using Vkr.API.Contracts.ChecklistsConrtacts;
using Vkr.Application.Interfaces.Services.ChecklistServices;
using Vkr.Domain.DTO.Checklist;
using Vkr.Domain.Enums.CheckLists;

namespace Vkr.API.Controllers;

[ApiController]
[Route("api/checklists")]
public class ChecklistsController : ControllerBase
{
    private readonly IChecklistService _service;

    public ChecklistsController(IChecklistService service)
    {
        _service = service;
    }

    /// <summary>
    /// Получить все чек-листы для заданного владельца
    /// </summary>
    /// <param name="ownerType">Тип владельца: Project или Task</param>
    /// <param name="ownerId">Id проекта или задачи</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChecklistDto>>> GetByOwner(
        [FromQuery] ChecklistOwnerType ownerType,
        [FromQuery] int ownerId)
    {
        var list = await _service.GetByOwnerAsync(ownerType, ownerId);
        return Ok(list);
    }

    /// <summary>
    /// Получить один чек-лист по его Id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ChecklistDto>> GetById(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        return Ok(dto);
    }

    /// <summary>
    /// Создать новый чек-лист для проекта или задачи
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ChecklistDto>> Create(
        [FromQuery] ChecklistOwnerType ownerType,
        [FromQuery] int ownerId,
        [FromBody] CreateChecklistRequest request)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Идентификатор пользователя не найден в токене.");

        var creatorId = int.Parse(userIdClaim);
        var created = await _service.CreateAsync(ownerType, ownerId, request.Title, creatorId);
        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    /// <summary>
    /// Добавить пункт в чек-лист
    /// </summary>
    [HttpPost("{checklistId:int}/items")]
    public async Task<ActionResult<ChecklistItemDto>> AddItem(
        [FromRoute] int checklistId,
        [FromBody] CreateChecklistItemRequest request)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Идентификатор пользователя не найден в токене.");

        var creatorId = int.Parse(userIdClaim);
        var item = await _service.AddItemAsync(checklistId, request.Title, creatorId);
        return CreatedAtAction(
            nameof(GetById),
            new { id = checklistId },
            item);
    }

    /// <summary>
    /// Обновить пункт (заголовок и/или признак выполнения)
    /// </summary>
    [HttpPatch("items/{itemId:int}")]
    public async Task<IActionResult> UpdateItem(
        [FromRoute] int itemId,
        [FromBody] UpdateChecklistItemRequest request)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Идентификатор пользователя не найден в токене.");

        var creatorId = int.Parse(userIdClaim);

        if (request.Title is not null)
            await _service.UpdateItemAsync(itemId, request.Title, creatorId);

        if (request.IsCompleted.HasValue)
            await _service.ToggleItemAsync(itemId, request.IsCompleted.Value, creatorId);

        return NoContent();
    }

    /// <summary>
    /// Удалить пункт из чек-листа
    /// </summary>
    [HttpDelete("items/{itemId:int}")]
    public async Task<IActionResult> DeleteItem([FromRoute] int itemId)
    {
        var userIdClaim = HttpContext.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized("Идентификатор пользователя не найден в токене.");

        var creatorId = int.Parse(userIdClaim);
        await _service.DeleteItemAsync(itemId, creatorId);
        return NoContent();
    }
}