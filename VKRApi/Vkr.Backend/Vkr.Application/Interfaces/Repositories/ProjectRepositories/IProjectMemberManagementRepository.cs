using Vkr.Domain.DTO.Project;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Application.Interfaces.Repositories.ProjectRepositories;

public interface IProjectMemberManagementRepository
{
    Task<IEnumerable<ProjectMemberDTO>> GetAllMembersAsync(int projectId);
    Task<ProjectMemberDTO> GetMemberAsync(int projectId, int memberId);
    Task<IEnumerable<Workers>> GetMemberSubordinatesAsync(int projectId, int memberId);
    Task<IEnumerable<Workers>> GetMemberDirectors(int projectId, int memberId);
    Task<bool> AddSubordinateToMember(int projectId, int memberId, int subordinateId);
    Task<bool> RemoveSubordinateFromMember(int projectId, int memberId, int subordinateId);
    Task<bool> AddMember(int projectId, int memberId);
    Task<bool> RemoveMember(int projectId, int memberId);
}