using Vkr.Domain.Entities.Progect;

namespace Vkr.Application.Interfaces;

public interface IProjectLinkService
{
    Task<int> AddLinkAsync(int projectId, string link, string? description, int creatorId);
    Task<bool> UpdateLinkAsync(int linkId, string link, string? description, int creatorId);
    Task<bool> DeleteLinkAsync(int linkId, int creatorId);
    Task<IEnumerable<ProjectLink>> GetLinksByProjectIdAsync(int projectId);
}