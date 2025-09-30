using Microsoft.AspNetCore.Mvc;
using Vkr.Application.Interfaces.Services.OrganizationServices;
using Vkr.Domain.DTO;
using Vkr.Domain.Entities.Organization;

namespace Vkr.API.Controllers.OrganizationControllers;
[ApiController]
[Route("api/organization")]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _service;

    public OrganizationController(IOrganizationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all organizations with their owners.
    /// </summary>
    /// <returns>A list of organization DTOs.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrganizationDTO>>> GetOrganizations()
    {
        var organizations = await _service.GetOrganizationsAsync();
        return Ok(organizations);
    }
}