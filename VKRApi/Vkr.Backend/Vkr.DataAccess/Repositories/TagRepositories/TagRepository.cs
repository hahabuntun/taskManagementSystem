using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vkr.Application.Interfaces.Repositories.TagRepositories;
using Vkr.DataAccess;
using Vkr.Domain.Entities;

namespace Vkr.DataAccess.Repositories.TagRepositories;

public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddExistingTagToProjectAsync(int projectId, int tagId)
    {
        var project = await _context.Projects
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == projectId)
            ?? throw new Exception($"Project with ID {projectId} not found");

        var tag = await _context.Tags.FindAsync(tagId)
            ?? throw new Exception($"Tag with ID {tagId} not found");

        if (project.Tags.Any(t => t.Id == tagId))
            throw new Exception($"Tag with ID {tagId} is already associated with project {projectId}");

        project.Tags.Add(tag);
        await _context.SaveChangesAsync();
    }

    public async Task AddExistingTagToTaskAsync(int taskId, int tagId)
    {
        var task = await _context.Tasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new Exception($"Task with ID {taskId} not found");

        var tag = await _context.Tags.FindAsync(tagId)
            ?? throw new Exception($"Tag with ID {tagId} not found");

        if (task.Tags.Any(t => t.Id == tagId))
            throw new Exception($"Tag with ID {tagId} is already associated with task {taskId}");

        task.Tags.Add(tag);
        await _context.SaveChangesAsync();
    }

    public async Task AddExistingTagToTemplateAsync(int templateId, int tagId)
    {
        var template = await _context.TaskTemplates
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == templateId)
            ?? throw new Exception($"Task template with ID {templateId} not found");

        var tag = await _context.Tags.FindAsync(tagId)
            ?? throw new Exception($"Tag with ID {tagId} not found");

        if (template.Tags.Any(t => t.Id == tagId))
            throw new Exception($"Tag with ID {tagId} is already associated with task template {templateId}");

        template.Tags.Add(tag);
        await _context.SaveChangesAsync();
    }

    public async Task<Tags> AddNewTagToProjectAsync(int projectId, string name, string color)
    {
        var project = await _context.Projects
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == projectId)
            ?? throw new Exception($"Project with ID {projectId} not found");

        if (await _context.Tags.AnyAsync(t => t.Name == name))
            throw new Exception($"Tag with name '{name}' already exists");

        var newTag = new Tags { Name = name, Color = color };
        _context.Tags.Add(newTag);
        project.Tags.Add(newTag);
        await _context.SaveChangesAsync();
        return newTag;
    }

    public async Task<Tags> AddNewTagToTaskAsync(int taskId, string name, string color)
    {
        var task = await _context.Tasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new Exception($"Task with ID {taskId} not found");

        if (await _context.Tags.AnyAsync(t => t.Name == name))
            throw new Exception($"Tag with name '{name}' already exists");

        var newTag = new Tags { Name = name, Color = color };
        _context.Tags.Add(newTag);
        task.Tags.Add(newTag);
        await _context.SaveChangesAsync();
        return newTag;
    }

    public async Task<Tags> AddNewTagToTemplateAsync(int templateId, string name, string color)
    {
        var template = await _context.TaskTemplates
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == templateId)
            ?? throw new Exception($"Task template with ID {templateId} not found");

        if (await _context.Tags.AnyAsync(t => t.Name == name))
            throw new Exception($"Tag with name '{name}' already exists");

        var newTag = new Tags { Name = name, Color = color };
        _context.Tags.Add(newTag);
        template.Tags.Add(newTag);
        await _context.SaveChangesAsync();
        return newTag;
    }

    public async Task DeleteTagFromProjectAsync(int projectId, int tagId)
    {
        var project = await _context.Projects
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == projectId)
            ?? throw new Exception($"Project with ID {projectId} not found");

        var tag = project.Tags.FirstOrDefault(t => t.Id == tagId)
            ?? throw new Exception($"Tag with ID {tagId} is not associated with project {projectId}");

        project.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTagFromTaskAsync(int taskId, int tagId)
    {
        var task = await _context.Tasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new Exception($"Task with ID {taskId} not found");

        var tag = task.Tags.FirstOrDefault(t => t.Id == tagId)
            ?? throw new Exception($"Tag with ID {tagId} is not associated with task {taskId}");

        task.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTagFromTemplateAsync(int templateId, int tagId)
    {
        var template = await _context.TaskTemplates
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == templateId)
            ?? throw new Exception($"Task template with ID {templateId} not found");

        var tag = template.Tags.FirstOrDefault(t => t.Id == tagId)
            ?? throw new Exception($"Tag with ID {tagId} is not associated with task template {templateId}");

        template.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Tags>> GetAllProjectTagsAsync(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == projectId)
            ?? throw new Exception($"Project with ID {projectId} not found");

        return project.Tags.ToList();
    }

    public async Task<List<Tags>> GetAllTaskTagsAsync(int taskId)
    {
        var task = await _context.Tasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new Exception($"Task with ID {taskId} not found");

        return task.Tags.ToList();
    }

    public async Task<List<Tags>> GetAllTemplateTagsAsync(int templateId)
    {
        var template = await _context.TaskTemplates
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == templateId)
            ?? throw new Exception($"Task template with ID {templateId} not found");

        return template.Tags.ToList();
    }

    public async Task AddTagsToProjectAsync(int projectId, List<int> existingTagIds, List<(string Name, string Color)> newTags)
    {
        var project = await _context.Projects
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == projectId)
            ?? throw new Exception($"Project with ID {projectId} not found");

        await AddTagsToEntityAsync(project.Tags, existingTagIds, newTags);
        await _context.SaveChangesAsync();
    }

    public async Task AddTagsToTaskAsync(int taskId, List<int> existingTagIds, List<(string Name, string Color)> newTags)
    {
        var task = await _context.Tasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new Exception($"Task with ID {taskId} not found");

        await AddTagsToEntityAsync(task.Tags, existingTagIds, newTags);
        await _context.SaveChangesAsync();
    }

    public async Task AddTagsToTemplateAsync(int templateId, List<int> existingTagIds, List<(string Name, string Color)> newTags)
    {
        var template = await _context.TaskTemplates
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == templateId)
            ?? throw new Exception($"Task template with ID {templateId} not found");

        await AddTagsToEntityAsync(template.Tags, existingTagIds, newTags);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTagsForProjectAsync(int projectId, List<int> existingTagIds, List<(string Name, string Color)> newTags)
    {
        var project = await _context.Projects
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == projectId)
            ?? throw new Exception($"Project with ID {projectId} not found");

        project.Tags.Clear();
        await AddTagsToEntityAsync(project.Tags, existingTagIds, newTags);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTagsForTaskAsync(int taskId, List<int> existingTagIds, List<(string Name, string Color)> newTags)
    {
        var task = await _context.Tasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new Exception($"Task with ID {taskId} not found");

        task.Tags.Clear();
        await AddTagsToEntityAsync(task.Tags, existingTagIds, newTags);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTagsForTemplateAsync(int templateId, List<int> existingTagIds, List<(string Name, string Color)> newTags)
    {
        var template = await _context.TaskTemplates
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == templateId)
            ?? throw new Exception($"Task template with ID {templateId} not found");

        template.Tags.Clear();
        await AddTagsToEntityAsync(template.Tags, existingTagIds, newTags);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Tags>> GetAvailableTagsForProjectAsync(int projectId)
    {
        var projectTagIds = await _context.Projects
            .Where(p => p.Id == projectId)
            .SelectMany(p => p.Tags)
            .Select(t => t.Id)
            .ToListAsync();

        return await _context.Tags
            .Where(t => !projectTagIds.Contains(t.Id))
            .ToListAsync();
    }

    public async Task<List<Tags>> GetAvailableTagsForTaskAsync(int taskId)
    {
        var taskTagIds = await _context.Tasks
            .Where(t => t.Id == taskId)
            .SelectMany(t => t.Tags)
            .Select(t => t.Id)
            .ToListAsync();

        return await _context.Tags
            .Where(t => !taskTagIds.Contains(t.Id))
            .ToListAsync();
    }

    public async Task<List<Tags>> GetAvailableTagsForTemplateAsync(int templateId)
    {
        var templateTagIds = await _context.TaskTemplates
            .Where(t => t.Id == templateId)
            .SelectMany(t => t.Tags)
            .Select(t => t.Id)
            .ToListAsync();

        return await _context.Tags
            .Where(t => !templateTagIds.Contains(t.Id))
            .ToListAsync();
    }

    public async Task<List<Tags>> GetAllTagsForProjectsAsync()
    {
        return await _context.Tags
            .Where(t => t.Projects.Any())
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<Tags>> GetAllTagsForTasksAsync()
    {
        return await _context.Tags
            .Where(t => t.Tasks.Any())
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<Tags>> GetAllTagsForTemplatesAsync()
    {
        return await _context.Tags
            .Where(t => t.TaskTemplates.Any())
            .Distinct()
            .ToListAsync();
    }

    public async Task<Tags?> GetTagByIdAsync(int tagId)
    {
        return await _context.Tags.FindAsync(tagId);
    }

    private async Task AddTagsToEntityAsync(ICollection<Tags> entityTags, List<int> existingTagIds, List<(string Name, string Color)> newTags)
    {
        // Check uniqueness of new tag names
        var existingTagNames = await _context.Tags
            .Select(t => t.Name)
            .ToListAsync();

        foreach (var (name, color) in newTags)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Tag name cannot be empty");

            if (existingTagNames.Contains(name))
                throw new Exception($"Tag with name '{name}' already exists");

            var newTag = new Tags { Name = name, Color = color };
            _context.Tags.Add(newTag);
            entityTags.Add(newTag);
            existingTagNames.Add(name); // Update list for uniqueness check
        }

        // Add existing tags
        if (existingTagIds.Any())
        {
            var tags = await _context.Tags
                .Where(t => existingTagIds.Contains(t.Id))
                .ToListAsync();

            if (tags.Count != existingTagIds.Count)
                throw new Exception("One or more existing tag IDs are invalid");

            foreach (var tag in tags)
            {
                if (!entityTags.Any(t => t.Id == tag.Id))
                    entityTags.Add(tag);
            }
        }
    }
}