using Microsoft.EntityFrameworkCore;
using Vkr.Domain.Entities.Progect;
using Vkr.Domain.Repositories;

namespace Vkr.DataAccess.Repositories
{
    public class ProjectLinkRepository : IProjectLinkRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectLinkRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> AddLinkAsync(ProjectLink projectLink)
        {
            _context.ProjectLinks.Add(projectLink);
            await _context.SaveChangesAsync();
            return projectLink.Id;
        }

        public async Task<bool> UpdateLinkAsync(int linkId, string link, string? description)
        {
            var projectLink = await _context.ProjectLinks
                .FirstOrDefaultAsync(pl => pl.Id == linkId);

            if (projectLink == null)
                return false;

            projectLink.Link = link;
            projectLink.Description = description;

            _context.ProjectLinks.Update(projectLink);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLinkAsync(int linkId)
        {
            var projectLink = await _context.ProjectLinks
                .FirstOrDefaultAsync(pl => pl.Id == linkId);

            if (projectLink == null)
                return false;

            _context.ProjectLinks.Remove(projectLink);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProjectLink>> GetLinksByProjectIdAsync(int projectId)
        {
            return await _context.ProjectLinks
                .Where(pl => pl.ProjectId == projectId).ToListAsync();
        }

        public async Task<ProjectLink?> GetLinkByIdAsync(int linkId)
        {
            return await _context.ProjectLinks.FindAsync(linkId);
        }
    }
}