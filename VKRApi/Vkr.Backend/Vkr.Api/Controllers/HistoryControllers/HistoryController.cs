
using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.API.Controllers.HistoryControllers;

[ApiController]
[Route("api/history")]
public class HistoryController(IHistoryService service) : ControllerBase
{

    //получить историю сущности
    [HttpGet("{entityType}/{entityId}")]
    public async Task<ActionResult<IEnumerable<HistoryDTO>>> GetEntityHistory([FromRoute] HistoryEntityType entityType, [FromRoute] int entityId)
    {
        var res = await service.GetHistoryAsync(entityId, entityType);
        return Ok(res);
    }

    //удалить всю историю сущности
    [HttpDelete("{entityType}/{entityId}")]
    public async Task<ActionResult<bool>> DeleteEntityHistory([FromRoute] HistoryEntityType entityType, [FromRoute] int entityId)
    {
        var res = await service.DeleteHistoryAsync(entityId, entityType);
        return Ok(res);
    }

    //удалить 1 запись в истории сущности
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteHistoryItem([FromRoute] int id)
    {
        var res = await service.DeleteHistoryItemAsync(id);
        return Ok(res);
    }
    
}