using Microsoft.EntityFrameworkCore;
using Vkr.DataAccess;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.History;


public class HistoryRepository(ApplicationDbContext ctx) : Vkr.Application.Interfaces.Repositories.HistoryRepository.IHistoryRepository
{
    public async Task<bool> AddHistoryItemAsync(CreateHistoryDTO data)
    {
        var item = new History
        {
            CreatorId = data.CreatorId,
            RelatedEntityId = data.RelatedEntityId,
            RelatedEntityType = data.RelatedEntityType,
            Text = data.Text,
            CreatedOn = DateTime.UtcNow
        };
        await ctx.History.AddAsync(item);
        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteHistoryAsync(int itemId, HistoryEntityType entityType)
    {
        var items = ctx.History.Where(h => h.RelatedEntityId == itemId && h.RelatedEntityType == entityType);
        if (items != null)
        {
            ctx.History.RemoveRange(items);
            await ctx.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteHistoryItemAsync(int id)
    {
        var item = await ctx.History.FindAsync(id);

        if (item != null)
        {
            ctx.History.Remove(item);
            await ctx.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public  async Task<IEnumerable<HistoryDTO>> GetHistoryAsync(int itemId, HistoryEntityType entityType)
    {
        return await ctx.History
         .Where(h => h.RelatedEntityId == itemId && h.RelatedEntityType == entityType)
         .Select(h => new HistoryDTO
         {
            Id = h.Id,
            Text = h.Text,
            CreatedOn = h.CreatedOn,
            CreatorId = h.CreatorId,
            RelatedEntityId = h.RelatedEntityId,
            RelatedEntityType = h.RelatedEntityType,
            Creator = new SimpleWorkerDTO
            {
                Id = h.Creator.Id,
                Email = h.Creator.Email,
                Name = h.Creator.Name,
                SecondName = h.Creator.SecondName,
                ThirdName = h.Creator.ThirdName
            }
         }
         ).ToListAsync();
    }
}