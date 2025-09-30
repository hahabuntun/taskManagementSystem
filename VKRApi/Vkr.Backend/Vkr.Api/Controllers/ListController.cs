using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vkr.DataAccess;
using Vkr.Domain.Entities;
using Vkr.API.Contracts.WorkersControllerContracts;
using Vkr.Domain.Entities.Worker;

namespace Vkr.API.Controllers;

[Route("api/list")]
[ApiController]
public class ListController(ApplicationDbContext applicationDbContext) : ControllerBase
{
    /// <summary>
    /// Получает список статусов сотрудников.
    /// </summary>
    /// <returns>Список статусов сотрудников в виде объектов <see cref="WorkerStatusResponse"/>, где:
    /// <list type="bullet">
    /// <item><description>0 - Активный</description></item>
    /// <item><description>1 - Заблокирован</description></item>
    /// </list>
    /// </returns>ы
    [HttpGet("workers-statuses")]
    [Authorize]
    public async Task<ActionResult<List<WorkerStatusResponse>>> GetStatuses()
    {
        var statuses = Enum.GetValues(typeof(WorkerStatus))
                   .Cast<WorkerStatus>()
                   .Select(status => new WorkerStatusResponse((int)status))
                   .ToList();

        return Ok(statuses);
    }

    /// <summary>
    /// Получить цвета 
    /// </summary>
    /// <returns></returns>
    [HttpGet("colors")]
    public async Task<ActionResult<List<ColorInfo>>> GetColors(){
        return Ok(await applicationDbContext.Colors.AsNoTracking().ToListAsync());
    }

    /// <summary>
    /// Поулучить список статусов проектов
    /// </summary>
    /// <returns></returns>
    [HttpGet("project-statuses")]
    public async Task<ActionResult> GetProjectStatuses()
    {
        var response = await applicationDbContext
            .ProjectStatus
            .Include(p => p.RelatedColor)
            .AsNoTracking()
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                color = new { name = x.RelatedColor!.Name, code = x.RelatedColor!.Code }
            }).ToListAsync();

        return Ok(response);
    }


    /// <summary>
    /// Получить список статусов задач
    /// </summary>
    /// <returns></returns>
    [HttpGet("task-statuses")]
    public async Task<ActionResult> GetTaskStatuses()
    {
        var response = await applicationDbContext
            .TaskStatuses
            .AsNoTracking()
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                color = x.Color
            }).ToListAsync();

        return Ok(response);
    }

    /// <summary>
    /// Получить список возможных приоритетов задач
    /// </summary>
    [HttpGet("task-priorities")]
    public async Task<ActionResult> GetTaskPriorities()
    {
        var response = await applicationDbContext
            .TaskPriorities
            .AsNoTracking()
            .Select(x => new
            {
                id = x.Id,
                name = x.Name,
                color = x.Color
            }).ToListAsync();

        return Ok(response);
    }
}