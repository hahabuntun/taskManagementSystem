using AutoMapper;
using Vkr.Application.Interfaces.Repositories.CheklistRepository;
using Vkr.Application.Interfaces.Services.ChecklistServices;
using Vkr.Domain.DTO.Checklist;
using Vkr.Domain.Entities.CheckLists;
using Vkr.Domain.Enums.CheckLists;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services.CheckListServices;

public class ChecklistService : IChecklistService
{
    private readonly IChecklistRepository _repo;
    private readonly IMapper _mapper;
    private readonly IHistoryService _historyService;

    public ChecklistService(
        IChecklistRepository repo,
        IMapper mapper,
        IHistoryService historyService)
    {
        _repo = repo;
        _mapper = mapper;
        _historyService = historyService;
    }

    public async Task<ChecklistDto> CreateAsync(
        ChecklistOwnerType ownerType,
        int ownerId,
        string title, int creatorId)
    {
        var entity = new Checklist
        {
            OwnerType = ownerType,
            OwnerId = ownerId,
            Title = title
        };
        var result = await _repo.CreateAsync(entity);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Чек-лист '{title}' создан",
            RelatedEntityId = ownerId,
            RelatedEntityType = MapChecklistOwnerTypeToHistoryEntityType(ownerType),
            CreatorId = creatorId // Assuming creator ID is not available in this context
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        return _mapper.Map<ChecklistDto>(result);
    }

    public async Task<IEnumerable<ChecklistDto>> GetByOwnerAsync(
        ChecklistOwnerType ownerType,
        int ownerId)
    {
        var list = await _repo.GetByOwnerAsync(ownerType, ownerId);
        return _mapper.Map<IEnumerable<ChecklistDto>>(list);
    }

    public async Task<ChecklistDto> GetByIdAsync(int id)
    {
        var ent = await _repo.GetByIdAsync(id)
                  ?? throw new KeyNotFoundException("Чек-лист не найден");
        return _mapper.Map<ChecklistDto>(ent);
    }

    public async Task<ChecklistItemDto> AddItemAsync(int checklistId, string title, int creatorId)
    {
        var item = new ChecklistItem
        {
            ChecklistId = checklistId,
            Title = title
        };
        var result = await _repo.CreateItemAsync(item);

        // Add history record
        var checklist = await _repo.GetByIdAsync(checklistId);
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Элемент '{title}' добавлен в чек-лист '{checklist?.Title}'.",
            RelatedEntityId = checklist?.OwnerId ?? checklistId,
            RelatedEntityType = checklist != null ? MapChecklistOwnerTypeToHistoryEntityType(checklist.OwnerType) : HistoryEntityType.Task,
            CreatorId = creatorId // Assuming creator ID is not available
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        return _mapper.Map<ChecklistItemDto>(result);
    }

    public async Task ToggleItemAsync(int itemId, bool isCompleted, int creatorId)
    {
        var item = await _repo.GetItemByIdAsync(itemId)
                   ?? throw new KeyNotFoundException("Элемент не найден");
        item.IsCompleted = isCompleted;
        await _repo.UpdateItemAsync(item);

        // Add history record
        var checklist = await _repo.GetByIdAsync(item.ChecklistId);
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Элемент '{item.Title}' в чек-листе '{checklist?.Title}' помечен как {(isCompleted ? "выполненный" : "невыполненный")}.",
            RelatedEntityId = checklist?.OwnerId ?? item.ChecklistId,
            RelatedEntityType = checklist != null ? MapChecklistOwnerTypeToHistoryEntityType(checklist.OwnerType) : HistoryEntityType.Task,
            CreatorId = creatorId // Assuming creator ID is not available
        };
        await _historyService.AddHistoryItemAsync(historyDto);
    }

    public async Task<ChecklistItemDto> UpdateItemAsync(int itemId, string newTitle, int creatorId)
    {
        var item = await _repo.GetItemByIdAsync(itemId)
                   ?? throw new KeyNotFoundException("Элемент не найден");
        item.Title = newTitle;
        await _repo.UpdateItemAsync(item);

        // Add history record
        var checklist = await _repo.GetByIdAsync(item.ChecklistId);
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Элемент чек-листа '{checklist?.Title}' обновлен: новое название '{newTitle}'.",
            RelatedEntityId = checklist?.OwnerId ?? item.ChecklistId,
            RelatedEntityType = checklist != null ? MapChecklistOwnerTypeToHistoryEntityType(checklist.OwnerType) : HistoryEntityType.Task,
            CreatorId = creatorId // Assuming creator ID is not available
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        return _mapper.Map<ChecklistItemDto>(item);
    }

    public async Task DeleteItemAsync(int itemId, int creatorId)
    {
        var item = await _repo.GetItemByIdAsync(itemId)
                   ?? throw new KeyNotFoundException("Элемент не найден");
        var checklist = await _repo.GetByIdAsync(item.ChecklistId);

        await _repo.DeleteItemAsync(itemId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Элемент '{item.Title}' удален из чек-листа '{checklist?.Title}'.",
            RelatedEntityId = checklist?.OwnerId ?? item.ChecklistId,
            RelatedEntityType = checklist != null ? MapChecklistOwnerTypeToHistoryEntityType(checklist.OwnerType) : HistoryEntityType.Task,
            CreatorId = creatorId // Assuming creator ID is not available
        };
        await _historyService.AddHistoryItemAsync(historyDto);
    }

    private HistoryEntityType MapChecklistOwnerTypeToHistoryEntityType(ChecklistOwnerType ownerType)
    {
        return ownerType switch
        {
            ChecklistOwnerType.Task => HistoryEntityType.Task,
            ChecklistOwnerType.Project => HistoryEntityType.Project,
            _ => throw new ArgumentException($"Неизвестный тип владельца чек-листа: {ownerType}")
        };
    }
}