using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.TagRepositories;
using Vkr.Application.Interfaces.Services;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Notification;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public TagService(
        ITagRepository tagRepository,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
    }

    public async Task AddExistingTagToProjectAsync(int projectId, int tagId, int creatorId)
    {
        if (tagId <= 0)
            throw new ArgumentException("ID тега должен быть положительным.", nameof(tagId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        await _tagRepository.AddExistingTagToProjectAsync(projectId, tagId);

        // Add history record
        var tag = await _tagRepository.GetTagByIdAsync(tagId)
            ?? throw new KeyNotFoundException($"Тег с ID {tagId} не найден");
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Тег '{tag.Name}' добавлен к проекту с ID {projectId}.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(projectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == projectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Тег '{tag.Name}' добавлен к проекту.",
                RelatedEntityName = "Тег проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task AddExistingTagToTaskAsync(int taskId, int tagId, int creatorId)
    {
        if (tagId <= 0)
            throw new ArgumentException("ID тега должен быть положительным.", nameof(tagId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        await _tagRepository.AddExistingTagToTaskAsync(taskId, tagId);

        // Add history record
        var tag = await _tagRepository.GetTagByIdAsync(tagId)
            ?? throw new KeyNotFoundException($"Тег с ID {tagId} не найден");
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Тег '{tag.Name}' добавлен к задаче с ID {taskId}.",
            RelatedEntityId = taskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Тег '{tag.Name}' добавлен к задаче {taskId}.",
                RelatedEntityName = "Тег задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task AddExistingTagToTemplateAsync(int templateId, int tagId)
    {
        if (tagId <= 0)
            throw new ArgumentException("ID тега должен быть положительным.", nameof(tagId));

        await _tagRepository.AddExistingTagToTemplateAsync(templateId, tagId);
    }

    public async Task<Tags> AddNewTagToProjectAsync(int projectId, string name, string color, int creatorId)
    {
        ValidateTagInput(name, color);
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var tag = await _tagRepository.AddNewTagToProjectAsync(projectId, name, color);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Новый тег '{name}' добавлен к проекту с ID {projectId}.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(projectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == projectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Новый тег '{name}' добавлен к проекту.",
                RelatedEntityName = "Тег проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return tag;
    }

    public async Task<Tags> AddNewTagToTaskAsync(int taskId, string name, string color, int creatorId)
    {
        ValidateTagInput(name, color);
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var tag = await _tagRepository.AddNewTagToTaskAsync(taskId, name, color);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Новый тег '{name}' добавлен к задаче с ID {taskId}.",
            RelatedEntityId = taskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Новый тег '{name}' добавлен к задаче {taskId}.",
                RelatedEntityName = "Тег задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return tag;
    }

    public async Task<Tags> AddNewTagToTemplateAsync(int templateId, string name, string color)
    {
        ValidateTagInput(name, color);
        return await _tagRepository.AddNewTagToTemplateAsync(templateId, name, color);
    }

    public async Task DeleteTagFromProjectAsync(int projectId, int tagId, int creatorId)
    {
        if (tagId <= 0)
            throw new ArgumentException("ID тега должен быть положительным.", nameof(tagId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var tag = await _tagRepository.GetTagByIdAsync(tagId)
            ?? throw new KeyNotFoundException($"Тег с ID {tagId} не найден");

        await _tagRepository.DeleteTagFromProjectAsync(projectId, tagId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Тег '{tag.Name}' удален из проекта с ID {projectId}.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(projectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == projectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Тег '{tag.Name}' удален из проекта.",
                RelatedEntityName = "Тег проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task DeleteTagFromTaskAsync(int taskId, int tagId, int creatorId)
    {
        if (tagId <= 0)
            throw new ArgumentException("ID тега должен быть положительным.", nameof(tagId));
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        var tag = await _tagRepository.GetTagByIdAsync(tagId)
            ?? throw new KeyNotFoundException($"Тег с ID {tagId} не найден");

        await _tagRepository.DeleteTagFromTaskAsync(taskId, tagId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Тег '{tag.Name}' удален из задачи с ID {taskId}.",
            RelatedEntityId = taskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Тег '{tag.Name}' удален из задачи {taskId}.",
                RelatedEntityName = "Тег задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task DeleteTagFromTemplateAsync(int templateId, int tagId)
    {
        if (tagId <= 0)
            throw new ArgumentException("ID тега должен быть положительным.", nameof(tagId));

        await _tagRepository.DeleteTagFromTemplateAsync(templateId, tagId);
    }

    public async Task<List<Tags>> GetAllProjectTagsAsync(int projectId)
    {
        return await _tagRepository.GetAllProjectTagsAsync(projectId);
    }

    public async Task<List<Tags>> GetAllTaskTagsAsync(int taskId)
    {
        return await _tagRepository.GetAllTaskTagsAsync(taskId);
    }

    public async Task<List<Tags>> GetAllTemplateTagsAsync(int templateId)
    {
        return await _tagRepository.GetAllTemplateTagsAsync(templateId);
    }

    public async Task AddTagsToProjectAsync(int projectId, List<int> existingTagIds, List<(string Name, string Color)> newTags, int creatorId)
    {
        if (existingTagIds == null || newTags == null)
            throw new ArgumentNullException("Списки тегов не могут быть пустыми.");
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        foreach (var tagId in existingTagIds)
            if (tagId <= 0)
                throw new ArgumentException("Все ID существующих тегов должны быть положительными.", nameof(existingTagIds));

        foreach (var (name, color) in newTags)
            ValidateTagInput(name, color);

        await _tagRepository.AddTagsToProjectAsync(projectId, existingTagIds, newTags);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Теги добавлены к проекту с ID {projectId}.",
            RelatedEntityId = projectId,
            RelatedEntityType = HistoryEntityType.Project,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(projectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == projectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Теги добавлены к проекту.",
                RelatedEntityName = "Теги проекта",
                RelatedEntityId = projectId,
                RelatedEntityType = EntityType.Project,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task AddTagsToTaskAsync(int taskId, List<int> existingTagIds, List<(string Name, string Color)> newTags, int creatorId)
    {
        if (existingTagIds == null || newTags == null)
            throw new ArgumentNullException("Списки тегов не могут быть пустыми.");
        if (creatorId <= 0)
            throw new ArgumentException("ID создателя должен быть положительным.", nameof(creatorId));

        foreach (var tagId in existingTagIds)
            if (tagId <= 0)
                throw new ArgumentException("Все ID существующих тегов должны быть положительными.", nameof(existingTagIds));

        foreach (var (name, color) in newTags)
            ValidateTagInput(name, color);

        await _tagRepository.AddTagsToTaskAsync(taskId, existingTagIds, newTags);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Теги добавлены к задаче с ID {taskId}.",
            RelatedEntityId = taskId,
            RelatedEntityType = HistoryEntityType.Task,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(taskId, EntityType.Task);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Task && s.EntityId == taskId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Теги добавлены к задаче {taskId}.",
                RelatedEntityName = "Теги задачи",
                RelatedEntityId = taskId,
                RelatedEntityType = EntityType.Task,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task AddTagsToTemplateAsync(int templateId, List<int> existingTagIds, List<(string Name, string Color)> newTags)
    {
        if (existingTagIds == null || newTags == null)
            throw new ArgumentNullException("Списки тегов не могут быть пустыми.");

        foreach (var tagId in existingTagIds)
            if (tagId <= 0)
                throw new ArgumentException("Все ID существующих тегов должны быть положительными.", nameof(existingTagIds));

        foreach (var (name, color) in newTags)
            ValidateTagInput(name, color);

        await _tagRepository.AddTagsToTemplateAsync(templateId, existingTagIds, newTags);
    }

    public async Task<List<Tags>> GetAllProjectTagsAsync()
    {
        return await _tagRepository.GetAllTagsForProjectsAsync();
    }

    public async Task<List<Tags>> GetAllTaskTagsAsync()
    {
        return await _tagRepository.GetAllTagsForTasksAsync();
    }

    public async Task<List<Tags>> GetAllTemplateTagsAsync()
    {
        return await _tagRepository.GetAllTagsForTemplatesAsync();
    }

    private void ValidateTagInput(string name, string color)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя тега не может быть пустым.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Имя тега не может превышать 100 символов.", nameof(name));

        if (string.IsNullOrWhiteSpace(color))
            throw new ArgumentException("Цвет тега не может быть пустым.", nameof(color));

        if (color.Length > 20)
            throw new ArgumentException("Цвет тега не может превышать 20 символов.", nameof(color));

        if (!Regex.IsMatch(color, @"^#[0-9A-Fa-f]{6}$") && !IsValidColorName(color))
            throw new ArgumentException("Цвет тега должен быть валидным шестнадцатеричным кодом (например, #FF0000) или общепринятым названием цвета.", nameof(color));
    }

    private bool IsValidColorName(string color)
    {
        var validColorNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "red", "blue", "green", "yellow", "black", "white", "gray", "purple", "orange", "pink"
        };
        return validColorNames.Contains(color.ToLower());
    }
}