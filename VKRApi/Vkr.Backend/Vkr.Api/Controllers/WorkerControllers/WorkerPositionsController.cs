using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vkr.API.Contracts.WorkerPositionsControllerContracts;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Worker;

namespace Vkr.API.Controllers.WorkerControllers;

[Route("api/worker-positions")]
[ApiController]
public class WorkerPositionsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IWorkerPositionsService _workerPositionsService;

    public WorkerPositionsController(IMapper mapper, IWorkerPositionsService workerPositionsService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _workerPositionsService = workerPositionsService ?? throw new ArgumentNullException(nameof(workerPositionsService));
    }

    [HttpPost("")]
    public async Task<ActionResult<WorkerPositionDto>> CreateWorkerPosition(
        [FromBody] WorkerPositionCreateUpdateRequest request, 
        [FromServices] IValidator<WorkerPositionCreateUpdateRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }

        var workerPosition = _mapper.Map<WorkerPosition>(request);
        var creatorId = int.Parse(User.FindFirst("userId")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
        var createdPosition = await _workerPositionsService.CreateWorkerPosition(
            workerPosition, request.taskGiverIds, request.taskTakerIds, creatorId);
        
        return Ok(createdPosition);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WorkerPositionDto>> UpdateWorkerPositionAsync(
        [FromRoute] int id,
        [FromBody] WorkerPositionCreateUpdateRequest request,
        [FromServices] IValidator<WorkerPositionCreateUpdateRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }

        var workerPosition = _mapper.Map<WorkerPosition>(request);
        var creatorId = int.Parse(User.FindFirst("userId")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
        var updatedPosition = await _workerPositionsService.UpdateWorkerPositionAsync(
            id, workerPosition, request.taskGiverIds, request.taskTakerIds, creatorId);

        return updatedPosition == null ? NotFound() : Ok(updatedPosition);
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<WorkerPositionDto>>> GetWorkerPositionsAsync()
    {
        var positions = await _workerPositionsService.GetAllWorkerPositionsAsync();
        return Ok(positions);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteWorkerPositionAsync(int id)
    {
        var creatorId = int.Parse(User.FindFirst("userId")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
        var result = await _workerPositionsService.DeleteWorkerPositionAsync(id, creatorId);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkerPositionDto>> GetWorkerPositionByIdAsync(int id)
    {
        var position = await _workerPositionsService.GetWorkerPositionsByIdAsync(id);
        return position == null ? NotFound() : Ok(position);
    }
}