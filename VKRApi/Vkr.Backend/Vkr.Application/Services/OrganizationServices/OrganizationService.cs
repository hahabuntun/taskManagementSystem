using Vkr.Application.Interfaces.Repositories.OrganizationRepositories;
using Vkr.Application.Interfaces.Services.OrganizationServices;
using Vkr.Domain.DTO;
using Vkr.Domain.Entities.Organization;

namespace Vkr.Application.Services.OrganizationServices;
public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _repository;

    public OrganizationService(IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrganizationDTO>> GetOrganizationsAsync()
    {
        return await _repository.GetOrganizationsAsync();
    }
}