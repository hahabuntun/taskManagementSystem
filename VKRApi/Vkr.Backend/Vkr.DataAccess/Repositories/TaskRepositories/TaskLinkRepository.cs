using Microsoft.EntityFrameworkCore;
using Vkr.Domain.Entities.Task;
using Vkr.Domain.Repositories;

namespace Vkr.DataAccess.Repositories
{
    public class TaskLinkRepository : ITaskLinkRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskLinkRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> AddLinkAsync(TaskLink taskLink)
        {
            _context.TaskLinks.Add(taskLink);
            await _context.SaveChangesAsync();
            return taskLink.Id;
        }

        public async Task<bool> UpdateLinkAsync(int linkId, string link, string? description)
        {
            var taskLink = await _context.TaskLinks
                .FirstOrDefaultAsync(pl => pl.Id == linkId);

            if (taskLink == null)
                return false;

            taskLink.Link = link;
            taskLink.Description = description;

            _context.TaskLinks.Update(taskLink);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLinkAsync(int linkId)
        {
            var taskLink = await _context.TaskLinks
                .FirstOrDefaultAsync(pl => pl.Id == linkId);

            if (taskLink == null)
                return false;

            _context.TaskLinks.Remove(taskLink);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskLink>> GetLinksByTaskIdAsync(int taskId)
        {
            return await _context.TaskLinks
                .Where(pl => pl.TaskId == taskId).ToListAsync();
        }

        public async Task<TaskLink?> GetLinkByIdAsync(int linkId)
        {
            return await _context.TaskLinks.FindAsync(linkId);
        }
    }
}