using Vkr.Domain.DTO.Checklist;
using Vkr.Domain.Enums.CheckLists;

namespace Vkr.Application.Interfaces.Services.ChecklistServices;

public interface IChecklistService
{
    Task<ChecklistDto> CreateAsync(
        ChecklistOwnerType ownerType,
        int ownerId,
        string title,
        int creatorId); // Added creatorId

    Task<IEnumerable<ChecklistDto>> GetByOwnerAsync(
        ChecklistOwnerType ownerType,
        int ownerId);

    Task<ChecklistDto> GetByIdAsync(int id);

    Task<ChecklistItemDto> AddItemAsync(
        int checklistId,
        string title,
        int creatorId); // Added creatorId

    Task ToggleItemAsync(
        int itemId,
        bool isCompleted,
        int creatorId); // Added creatorId

    Task<ChecklistItemDto> UpdateItemAsync(
        int itemId,
        string newTitle,
        int creatorId); // Added creatorId

    Task DeleteItemAsync(
        int itemId,
        int creatorId); // Added creatorId
}