using Vkr.Domain.DTO;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Task;

namespace Vkr.Application.Interfaces.Repositories.TaskRepositories;

public interface ITaskRepository
{
    Task<TaskDTO> GetByIdAsync(int id);
    Task<IEnumerable<TaskDTO>> GetAllAsync();
    Task<IEnumerable<TaskDTO>> GetBySprintIdAsync(int sprintId);
    Task<IEnumerable<TaskDTO>> GetByAssigneeIdAsync(int workerId);
    Task<Tasks> GetEntityByIdAsync(int id);
    Task<IEnumerable<RelatedTaskDTO>> GetRelatedTasksAsync(int taskId);
    Task<IEnumerable<TaskDTO>> GetAvailableRelatedTasksAsync(int taskId);
    Task AddAsync(Tasks task, int[] existingTagIds, (string Name, string Color)[] newTags, CreateLinkDTO[] links);
    Task UpdateAsync(Tasks task, int[] existingTagIds, (string Name, string Color)[] newTags);
    Task DeleteAsync(int id);
    Task AddTaskRelationshipAsync(int taskId, int relatedTaskId, int relationshipTypeId);
    Task RemoveTaskRelationshipAsync(int relationshipId);
    Task RemoveTaskRelationshipByTaskIdsAsync(int taskId, int relatedTaskId);
    Task<TaskRelationship?> GetTaskRelationshipByIdAsync(int relationshipId);
}