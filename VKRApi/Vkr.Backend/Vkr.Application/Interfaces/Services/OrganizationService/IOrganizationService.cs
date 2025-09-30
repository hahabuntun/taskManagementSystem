using Vkr.Domain.DTO;
using Vkr.Domain.Entities.Organization;
namespace Vkr.Application.Interfaces.Services.OrganizationServices;
public interface IOrganizationService
{
   Task<IEnumerable<OrganizationDTO>> GetOrganizationsAsync();
}