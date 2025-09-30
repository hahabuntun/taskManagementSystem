using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Domain.Entities;

namespace Vkr.Application.Interfaces.Repositories.TagRepositories;

public interface ITagRepository
{
    /// <summary>
    /// Adds an existing tag to a project by ID.
    /// </summary>
    Task AddExistingTagToProjectAsync(int projectId, int tagId);

    /// <summary>
    /// Adds an existing tag to a task by ID.
    /// </summary>
    Task AddExistingTagToTaskAsync(int taskId, int tagId);

    /// <summary>
    /// Adds an existing tag to a task template by ID.
    /// </summary>
    Task AddExistingTagToTemplateAsync(int templateId, int tagId);

    /// <summary>
    /// Adds a new tag to a project by name and color.
    /// </summary>
    Task<Tags> AddNewTagToProjectAsync(int projectId, string name, string color);

    /// <summary>
    /// Adds a new tag to a task by name and color.
    /// </summary>
    Task<Tags> AddNewTagToTaskAsync(int taskId, string name, string color);

    /// <summary>
    /// Adds a new tag to a task template by name and color.
    /// </summary>
    Task<Tags> AddNewTagToTemplateAsync(int templateId, string name, string color);

    /// <summary>
    /// Deletes a tag from a project.
    /// </summary>
    Task DeleteTagFromProjectAsync(int projectId, int tagId);

    /// <summary>
    /// Deletes a tag from a task.
    /// </summary>
    Task DeleteTagFromTaskAsync(int taskId, int tagId);

    /// <summary>
    /// Deletes a tag from a task template.
    /// </summary>
    Task DeleteTagFromTemplateAsync(int templateId, int tagId);

    /// <summary>
    /// Gets all tags associated with a project.
    /// </summary>
    Task<List<Tags>> GetAllProjectTagsAsync(int projectId);

    /// <summary>
    /// Gets all tags associated with a task.
    /// </summary>
    Task<List<Tags>> GetAllTaskTagsAsync(int taskId);

    /// <summary>
    /// Gets all tags associated with a task template.
    /// </summary>
    Task<List<Tags>> GetAllTemplateTagsAsync(int templateId);

    /// <summary>
    /// Adds existing and new tags to a project.
    /// </summary>
    Task AddTagsToProjectAsync(int projectId, List<int> existingTagIds, List<(string Name, string Color)> newTags);

    /// <summary>
    /// Adds existing and new tags to a task.
    /// </summary>
    Task AddTagsToTaskAsync(int taskId, List<int> existingTagIds, List<(string Name, string Color)> newTags);

    /// <summary>
    /// Adds existing and new tags to a task template.
    /// </summary>
    Task AddTagsToTemplateAsync(int templateId, List<int> existingTagIds, List<(string Name, string Color)> newTags);

    /// <summary>
    /// Updates tags for a project, replacing existing tags with the provided set.
    /// </summary>
    Task UpdateTagsForProjectAsync(int projectId, List<int> existingTagIds, List<(string Name, string Color)> newTags);

    /// <summary>
    /// Updates tags for a task, replacing existing tags with the provided set.
    /// </summary>
    Task UpdateTagsForTaskAsync(int taskId, List<int> existingTagIds, List<(string Name, string Color)> newTags);

    /// <summary>
    /// Updates tags for a task template, replacing existing tags with the provided set.
    /// </summary>
    Task UpdateTagsForTemplateAsync(int templateId, List<int> existingTagIds, List<(string Name, string Color)> newTags);

    /// <summary>
    /// Gets all available tags not associated with a project.
    /// </summary>
    Task<List<Tags>> GetAvailableTagsForProjectAsync(int projectId);

    /// <summary>
    /// Gets all available tags not associated with a task.
    /// </summary>
    Task<List<Tags>> GetAvailableTagsForTaskAsync(int taskId);

    /// <summary>
    /// Gets all available tags not associated with a task template.
    /// </summary>
    Task<List<Tags>> GetAvailableTagsForTemplateAsync(int templateId);

    /// <summary>
    /// Gets all tags associated with any project.
    /// </summary>
    Task<List<Tags>> GetAllTagsForProjectsAsync();

    /// <summary>
    /// Gets all tags associated with any task.
    /// </summary>
    Task<List<Tags>> GetAllTagsForTasksAsync();

    /// <summary>
    /// Gets all tags associated with any task template.
    /// </summary>
    Task<List<Tags>> GetAllTagsForTemplatesAsync();

    /// <summary>
    /// Gets a tag by its ID.
    /// </summary>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>The tag if found, otherwise null.</returns>
    Task<Tags?> GetTagByIdAsync(int tagId);
}