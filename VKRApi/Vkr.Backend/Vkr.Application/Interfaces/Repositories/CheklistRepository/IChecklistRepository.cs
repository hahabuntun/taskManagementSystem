using Vkr.Domain.Entities.CheckLists;
using Vkr.Domain.Enums.CheckLists;

namespace Vkr.Application.Interfaces.Repositories.CheklistRepository;

public interface IChecklistRepository
{
    Task<Checklist>     CreateAsync(Checklist checklist);
    Task<Checklist?>    GetByIdAsync(int id);
    Task<IEnumerable<Checklist>> GetByOwnerAsync(
        ChecklistOwnerType ownerType, int ownerId);

    Task<ChecklistItem> CreateItemAsync(ChecklistItem item);
    Task<ChecklistItem?> GetItemByIdAsync(int id);
    Task UpdateItemAsync(ChecklistItem item);
    Task DeleteItemAsync(int id);
}