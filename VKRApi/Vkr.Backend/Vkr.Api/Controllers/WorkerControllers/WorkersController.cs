using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vkr.API.Contracts.WorkersControllerContracts;
using Vkr.API.Controllers.JournalControllers;
using Vkr.Application.Filters;
using Vkr.Application.Interfaces.Services.JournalServices;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Domain.Entities.Board;
using Vkr.Domain.Entities.Worker;

namespace Vkr.API.Controllers.WorkerControllers;

[ApiController]
[Authorize]
[Route("api/workers")]
public class WorkersController : BaseAuditController<Workers>
{
    private readonly IMapper _mapper;
    private readonly IWorkersService _workersService;
    private readonly IAuditService _auditService;

    public WorkersController(
        IMapper mapper, 
        IWorkersService workersService, 
        IAuditService auditService) 
        : base(auditService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _workersService = workersService ?? throw new ArgumentNullException(nameof(workersService));
        _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
    }

    [HttpPost("")]
    public async Task<ActionResult<WorkerResponse>> CreateUserAsync(
        [FromBody] CreateWorkerRequest request,
        [FromServices] IValidator<CreateWorkerRequest> validator)
    { 
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(error => new { error.PropertyName, error.ErrorMessage })
            });
        }

        var workerEntity = _mapper.Map<Workers>(request);
        var creatorId = int.Parse(User.FindFirst("userId")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
        var worker = await _workersService.CreateWorker(workerEntity, request.Password, creatorId);
        var response = _mapper.Map<WorkerResponse>(worker);

        return Ok(response);
    }

    [HttpGet("")]
    public async Task<ActionResult<List<WorkerResponse>>> GetWorkersAsync([FromQuery] WorkersFilter filter)
    {
        var workers = await _workersService.GetWorkersByFilterAsync(filter);
        var response = workers.Select(_mapper.Map<WorkerResponse>);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkerResponse>> GetWorkerByIdAsync(int id)
    {
        var worker = await _workersService.GetWorkerByIdAsync(id);
        var response = _mapper.Map<WorkerResponse>(worker);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<object>> DeleteWorkerAsync(int id)
    {
        var creatorId = int.Parse(User.FindFirst("userId")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
        var result = await _workersService.DeleteWorkerAsync(id, creatorId);
        return result ? NoContent() : NotFound();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WorkerResponse>> UpdateWorkerAsync(
        [FromRoute] int id,
        [FromBody] UpdateWorkerRequest request,
        [FromServices] IValidator<UpdateWorkerRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(error => new { error.PropertyName, error.ErrorMessage })
            });
        }

        var worker = _mapper.Map<Workers>(request);
        var creatorId = int.Parse(User.FindFirst("userId")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
        var updatedWorker = await _workersService.UpdateWorkerAsync(id, worker, creatorId);
        var response = _mapper.Map<WorkerResponse>(updatedWorker);

        return Ok(response);
    }
}