using Microsoft.EntityFrameworkCore;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Repositories;

namespace Vkr.DataAccess.Repositories
{
    public class TaskTemplateLinkRepository : ITaskTemplateLinkRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskTemplateLinkRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> AddLinkAsync(TaskTemplateLink taskTemplateLink)
        {
            _context.TaskTemplateLinks.Add(taskTemplateLink);
            await _context.SaveChangesAsync();
            return taskTemplateLink.Id;
        }

        public async Task<bool> UpdateLinkAsync(int linkId, string link, string? description)
        {
            var taskTemplateLink = await _context.TaskTemplateLinks
                .FirstOrDefaultAsync(pl => pl.Id == linkId);

            if (taskTemplateLink == null)
                return false;

            taskTemplateLink.Link = link;
            taskTemplateLink.Description = description;

            _context.TaskTemplateLinks.Update(taskTemplateLink);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLinkAsync(int linkId)
        {
            var taskTemplateLink = await _context.TaskTemplateLinks
                .FirstOrDefaultAsync(pl => pl.Id == linkId);

            if (taskTemplateLink == null)
                return false;

            _context.TaskTemplateLinks.Remove(taskTemplateLink);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskTemplateLink>> GetLinksByTaskTemplateIdAsync(int taskTemplateId)
        {
            return await _context.TaskTemplateLinks
                .Where(pl => pl.TaskTemplateId == taskTemplateId).ToListAsync();
        }
    }
}