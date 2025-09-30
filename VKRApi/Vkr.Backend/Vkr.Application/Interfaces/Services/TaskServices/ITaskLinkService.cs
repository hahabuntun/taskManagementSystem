using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces;

public interface ITaskLinkService
{
    Task<int> AddLinkAsync(int taskId, string link, string? description, int creatorId);
    Task<bool> UpdateLinkAsync(int linkId, string link, string? description, int creatorId);
    Task<bool> DeleteLinkAsync(int linkId, int creatorId);
    Task<IEnumerable<TaskLink>> GetLinksByTaskIdAsync(int taskId);
}