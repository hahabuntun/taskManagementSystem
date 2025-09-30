using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.TaskTemplateRepositories;
using Vkr.Domain.Entities.Task;

namespace Vkr.DataAccess.Repositories.TaskTemplateRepositories;

public class TaskTemplateRepository : ITaskTemplateRepository
{
    private readonly ApplicationDbContext _context;

    public TaskTemplateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskTemplates> CreateTemplateAsync(TaskTemplates template)
    {
        _context.TaskTemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task<TaskTemplates> UpdateTemplateAsync(int templateId, TaskTemplates template)
    {
        var existingTemplate = await _context.TaskTemplates
            .Include(t => t.Tags)
            .Include(t => t.TaskTempateLinks)
            .FirstOrDefaultAsync(t => t.Id == templateId)
            ?? throw new Exception("Template not found");

        // Update scalar properties
        existingTemplate.TemplateName = template.TemplateName;
        existingTemplate.TaskName = template.TaskName;
        existingTemplate.Description = template.Description;
        existingTemplate.TaskStatusId = template.TaskStatusId;
        existingTemplate.TaskPriorityId = template.TaskPriorityId;
        existingTemplate.TaskTypeId = template.TaskTypeId;
        existingTemplate.StartDate = template.StartDate;
        existingTemplate.EndDate = template.EndDate;
        existingTemplate.Progress = template.Progress;
        existingTemplate.StoryPoints = template.StoryPoints;


        await _context.SaveChangesAsync();
        return existingTemplate;
    }

    public async Task DeleteTemplateAsync(int templateId)
    {
        var template = await _context.TaskTemplates.FindAsync(templateId)
            ?? throw new Exception("Template not found");
        
        _context.TaskTemplates.Remove(template);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskTemplates> GetTemplateByIdAsync(int templateId)
    {
        return await _context.TaskTemplates
            .Include(t => t.Tags)
            .Include(t => t.TaskTempateLinks)
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskPriority)
            .Include(t => t.TaskType)
            .FirstOrDefaultAsync(t => t.Id == templateId)
            ?? throw new Exception("Template not found");
    }

    public async Task<List<TaskTemplates>> GetAllTemplatesAsync()
    {
        return await _context.TaskTemplates
            .Include(t => t.Tags)
            .Include(t => t.TaskStatus)
            .Include(t => t.TaskPriority)
            .Include(t => t.TaskType)
            .ToListAsync();
    }

}