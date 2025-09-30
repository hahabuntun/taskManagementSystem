using Vkr.Domain.DTO;
using Vkr.Domain.Entities.Organization;


namespace Vkr.Application.Interfaces.Repositories.OrganizationRepositories;
public interface IOrganizationRepository
{
    Task<IEnumerable<OrganizationDTO>> GetOrganizationsAsync();
}