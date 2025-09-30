using Vkr.Domain.Entities.History;

namespace Vkr.Application.Interfaces.Services.HistoryServices;

public interface IHistoryService
{
    Task<IEnumerable<HistoryDTO>> GetHistoryAsync(int itemId, HistoryEntityType entityType);
    Task<bool> DeleteHistoryItemAsync(int id);
    Task<bool> DeleteHistoryAsync(int itemId, HistoryEntityType entityType);
    Task<bool> AddHistoryItemAsync(CreateHistoryDTO data);
}