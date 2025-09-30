using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vkr.API.Contracts.WorkersControllerContracts;
using Vkr.API.Contracts.WorkersManagmentContracts;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Domain.DTO.Worker;

namespace Vkr.API.Controllers.WorkerControllers;

[Route("api/workers-management")]
[ApiController]
[Authorize]
public class WorkerManagementController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IWorkersManagementService _workersManagementService;

    public WorkerManagementController(
        IMapper mapper, 
        IWorkersManagementService workersManagementService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _workersManagementService = workersManagementService ?? throw new ArgumentNullException(nameof(workersManagementService));
    }

    [HttpPost("")]
    public async Task<ActionResult<bool>> CreateConnectionManagerToEmployee(
        [FromBody] CreateManagerToEmployeeRequest request,
        [FromServices] IValidator<CreateManagerToEmployeeRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(error => new { error.PropertyName, error.ErrorMessage })
            });
        }

        var connection = _mapper.Map<WorkersManagmentDTO>(request);
        var creatorId = int.Parse(User.FindFirst("userId")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
        var result = await _workersManagementService.SetConnection(connection, creatorId);
        return Ok(result);
    }

    [HttpDelete("")]
    public async Task<ActionResult<object>> DeleteConnectionManagerToEmployee(
        [FromQuery] int managerId,
        [FromQuery] int subordinateId)
    {
        var creatorId = int.Parse(User.FindFirst("userId")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
        var result = await _workersManagementService.DeleteConnection(managerId, subordinateId, creatorId);
        return result ? NoContent() : NotFound();
    }

    [HttpPut("")]
    public async Task<ActionResult<int>> UpdateConnectionManagerToEmployee(
        [FromBody] CreateManagerToEmployeeRequest request,
        [FromServices] IValidator<CreateManagerToEmployeeRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(error => new { error.PropertyName, error.ErrorMessage })
            });
        }

        var connection = _mapper.Map<WorkersManagmentDTO>(request);
        var creatorId = int.Parse(User.FindFirst("userId")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
        var result = await _workersManagementService.UpdateConnection(connection, creatorId);
        return Ok(result);
    }

    [HttpGet("my-employees/{id:int}")]
    public async Task<ActionResult<List<WorkerResponse>>> GetAllEmployeesOfManager(int id)
    {
        var subordinates = await _workersManagementService.GetSubordinates(id);
        var response = subordinates.Select(_mapper.Map<WorkerResponse>);
        return Ok(response);
    }

    [HttpGet("my-managers/{id:int}")]
    public async Task<ActionResult<List<WorkerResponse>>> GetAllManagersOfEmployee(int id)
    {
        var managers = await _workersManagementService.GetManagers(id);
        var response = managers.Select(_mapper.Map<WorkerResponse>);
        return Ok(response);
    }
}