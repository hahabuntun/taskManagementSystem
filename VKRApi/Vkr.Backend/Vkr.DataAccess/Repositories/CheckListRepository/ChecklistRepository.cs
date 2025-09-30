using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.CheklistRepository;
using Vkr.Domain.Entities.CheckLists;
using Vkr.Domain.Enums.CheckLists;

namespace Vkr.DataAccess.Repositories.CheckListRepository;

public class ChecklistRepository(ApplicationDbContext ctx) : IChecklistRepository
{
    public async Task<Checklist> CreateAsync(Checklist checklist)
    {
        ctx.Checklists.Add(checklist);
        await ctx.SaveChangesAsync();
        return checklist;
    }

    public Task<Checklist?> GetByIdAsync(int id)
        => ctx.Checklists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<IEnumerable<Checklist>> GetByOwnerAsync(
        ChecklistOwnerType ownerType, int ownerId)
        => ctx.Checklists
            .Include(c => c.Items)
            .Where(c => c.OwnerType == ownerType && c.OwnerId == ownerId)
            .AsNoTracking()
            .ToListAsync()
            .ContinueWith(t => (IEnumerable<Checklist>)t.Result);

    public async Task<ChecklistItem> CreateItemAsync(ChecklistItem item)
    {
        ctx.ChecklistItems.Add(item);
        await ctx.SaveChangesAsync();
        return item;
    }

    public Task<ChecklistItem?> GetItemByIdAsync(int id)
        => ctx.ChecklistItems.FindAsync(id).AsTask();

    public async Task UpdateItemAsync(ChecklistItem item)
    {
        ctx.ChecklistItems.Update(item);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteItemAsync(int id)
    {
        var item = await ctx.ChecklistItems.FindAsync(id);
        if (item != null)
        {
            ctx.ChecklistItems.Remove(item);
            await ctx.SaveChangesAsync();
        }
    }
}