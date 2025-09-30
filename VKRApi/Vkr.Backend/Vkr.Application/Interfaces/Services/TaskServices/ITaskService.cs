using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities;

namespace Vkr.Application.Interfaces.Services.TaskServices;

public interface ITaskService
{
    Task<TaskDTO> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskDTO>> GetAllTasksAsync();
    Task<IEnumerable<TaskDTO>> GetTasksBySprintAsync(int sprintId);
    Task<IEnumerable<TaskDTO>> GetTasksByAssigneeAsync(int workerId);
    Task<int> CreateTaskAsync(CreateTaskDTO options); // CreatorId already in DTO
    Task UpdateTaskAsync(int id, CreateTaskDTO options, int creatorId);
    Task DeleteTaskAsync(int id, int creatorId);
    Task AddTaskRelationshipAsync(int taskId, int relatedTaskId, int relationshipTypeId, int creatorId);
    Task RemoveTaskRelationshipAsync(int relationshipId, int creatorId);
    Task RemoveTaskRelationshipByTaskIdsAsync(int taskId, int relatedTaskId, int creatorId);
    Task<IEnumerable<RelatedTaskDTO>> GetRelatedTasksAsync(int taskId);
    Task<IEnumerable<TaskDTO>> GetAvailableRelatedTasksAsync(int taskId);
    Task<List<Tags>> GetAvailableTags(int taskId);
    Task<List<Tags>> GetAllTags();
}