using Vkr.Domain.Entities.Worker;

namespace Vkr.Application.Interfaces;

public interface IProjectMemberManagementService
{
    Task<IEnumerable<ProjectMemberDTO>> GetAllMembersAsync(int projectId);
    Task<ProjectMemberDTO> GetMemberAsync(int projectId, int memberId);
    Task<bool> AddMemberAsync(int projectId, int workerId, int creatorId);
    Task<bool> RemoveMemberAsync(int projectId, int workerId, int creatorId);
    Task<IEnumerable<Workers>> GetMemberSubordinatesAsync(int projectId, int memberId);
    Task<IEnumerable<Workers>> GetMemberDirectors(int projectId, int memberId);
    Task<bool> AddSubordinateToMember(int projectId, int memberId, int subordinateId, int creatorId);
    Task<bool> RemoveSubordinateFromMember(int projectId, int memberId, int subordinateId, int creatorId);
}