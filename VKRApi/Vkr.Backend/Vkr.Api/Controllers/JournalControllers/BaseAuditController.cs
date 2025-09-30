using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Interfaces.Services.JournalServices;
using static Vkr.API.Utils.Response;

namespace Vkr.API.Controllers.JournalControllers;

[ApiController]
public abstract class BaseAuditController<T>(IAuditService auditService) : ControllerBase
{
    [HttpGet("{id:int}/audit")]
    public async Task<ActionResult> GetAudit(int id)
    {
        var logs = await auditService.GetLogsAsync<T>(id);
        return Success(logs);
    }
 
    [HttpGet("{id:int}/history/summary")]
    public async Task<ActionResult<string>> GetHistorySummary(int id)
    {
        // проверяем что чек-лист существует
        var summary = await auditService.GetHistorySummaryAsync<T>(id);
        return Ok(summary);
    }
}