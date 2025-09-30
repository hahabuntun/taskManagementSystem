using Vkr.Domain.Entities;

namespace Vkr.Application.Interfaces.Services;

public interface ITagService
{
    Task AddExistingTagToProjectAsync(int projectId, int tagId, int creatorId);
    Task AddExistingTagToTaskAsync(int taskId, int tagId, int creatorId);
    Task AddExistingTagToTemplateAsync(int templateId, int tagId);
    Task<Tags> AddNewTagToProjectAsync(int projectId, string name, string color, int creatorId);
    Task<Tags> AddNewTagToTaskAsync(int taskId, string name, string color, int creatorId);
    Task<Tags> AddNewTagToTemplateAsync(int templateId, string name, string color);
    Task DeleteTagFromProjectAsync(int projectId, int tagId, int creatorId);
    Task DeleteTagFromTaskAsync(int taskId, int tagId, int creatorId);
    Task DeleteTagFromTemplateAsync(int templateId, int tagId);
    Task<List<Tags>> GetAllProjectTagsAsync(int projectId);
    Task<List<Tags>> GetAllTaskTagsAsync(int taskId);
    Task<List<Tags>> GetAllTemplateTagsAsync(int templateId);
    Task AddTagsToProjectAsync(int projectId, List<int> existingTagIds, List<(string Name, string Color)> newTags, int creatorId);
    Task AddTagsToTaskAsync(int taskId, List<int> existingTagIds, List<(string Name, string Color)> newTags, int creatorId);
    Task AddTagsToTemplateAsync(int templateId, List<int> existingTagIds, List<(string Name, string Color)> newTags);
    Task<List<Tags>> GetAllProjectTagsAsync();
    Task<List<Tags>> GetAllTaskTagsAsync();
    Task<List<Tags>> GetAllTemplateTagsAsync();
}