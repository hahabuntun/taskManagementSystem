using Vkr.Application.Interfaces.Repositories.HistoryRepository;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services.HistoryServices;

public class HistoryService(
    IHistoryRepository repo) : IHistoryService
{
    public async Task<bool> DeleteHistoryAsync(int itemId, HistoryEntityType entityType)
    {
        return await repo.DeleteHistoryAsync(itemId, entityType);
    }

    public async Task<bool> DeleteHistoryItemAsync(int id)
    {
        return await repo.DeleteHistoryItemAsync(id);
    }

    public Task<IEnumerable<HistoryDTO>> GetHistoryAsync(int itemId, HistoryEntityType entityType)
    {
        return repo.GetHistoryAsync(itemId, entityType);
    }

    public async Task<bool> AddHistoryItemAsync(CreateHistoryDTO data)
    {
        return await repo.AddHistoryItemAsync(data);
    }
}