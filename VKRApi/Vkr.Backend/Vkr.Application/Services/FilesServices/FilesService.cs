using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Vkr.Application.Interfaces.FileStorage;
using Vkr.Application.Interfaces.Repositories.FilesRepositories;
using Vkr.Application.Interfaces.Services.FilesService;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.Enums.Files;
using Vkr.Domain.Entities.Notification;
using File = Vkr.Domain.Entities.Files.File;
using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services.FilesServices;

public class FilesService : IFilesService
{
    private readonly IFilesRepository _repo;
    private readonly IFileStorage _storage;
    private readonly IHttpContextAccessor _httpCtx;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public FilesService(
        IFilesRepository repo,
        IFileStorage storage,
        IHttpContextAccessor httpCtx,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _repo = repo;
        _storage = storage;
        _httpCtx = httpCtx;
        _notificationService = notificationService;
        _historyService = historyService;
    }

    public async Task<File> UploadAsync(
        FileOwnerType ownerType,
        int ownerId,
        IFormFile formFile,
        string title,
        string? description,
        int userId)
    {
        var (key, length) = await _storage.UploadFileAsync(formFile, title);

        var file = new File
        {
            OwnerType = ownerType,
            OwnerId = ownerId,
            Name = $"{title}{Path.GetExtension(formFile.FileName)}",
            Description = description,
            Key = key,
            FileSize = length,
            CreatedAt = DateTime.UtcNow,
            CreatorId = userId
        };

        var savedFile = await _repo.AddAsync(file);

        // Add history record
        var entityName = GetHumanReadableEntityName(ownerType);
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Файл '{file.Name}' загружен в {entityName} с ID {ownerId}.",
            RelatedEntityId = ownerId,
            RelatedEntityType = MapFileOwnerTypeToHistoryEntityType(ownerType),
            CreatorId = userId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to subscribed workers
        var subscriptions = await _notificationService.GetEntitySubscriptions(ownerId, MapFileOwnerTypeToEntityType(ownerType));
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == MapFileOwnerTypeToEntityType(ownerType) && s.EntityId == ownerId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Файл '{file.Name}' загружен в {entityName} с ID {ownerId}.",
                RelatedEntityName = file.Name,
                RelatedEntityId = ownerId,
                RelatedEntityType = MapFileOwnerTypeToEntityType(ownerType),
                CreatorId = userId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return savedFile;
    }

    public async Task<(Stream Stream, string FileName)> DownloadAsync(int fileId)
    {
        var file = await _repo.GetAsync(fileId)
                   ?? throw new KeyNotFoundException("Файл не найден");

        var stream = await _storage.DownloadFileAsync(file.Key);
        return (stream, file.Name!);
    }

    public async Task<bool> DeleteAsync(int fileId, int userId)
    {
        var file = await _repo.GetAsync(fileId);
        if (file == null) return false;

        await _repo.DeleteAsync(fileId);
        await _storage.DeleteFileAsync(file.Key);

        // Add history record
        var entityName = GetHumanReadableEntityName(file.OwnerType);
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Файл '{file.Name}' удален из {entityName} с ID {file.OwnerId}.",
            RelatedEntityId = file.OwnerId,
            RelatedEntityType = MapFileOwnerTypeToHistoryEntityType(file.OwnerType),
            CreatorId = userId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to subscribed workers
        var subscriptions = await _notificationService.GetEntitySubscriptions(file.OwnerId, MapFileOwnerTypeToEntityType(file.OwnerType));
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == MapFileOwnerTypeToEntityType(file.OwnerType) && s.EntityId == file.OwnerId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Файл '{file.Name}' удален из {entityName} с ID {file.OwnerId}.",
                RelatedEntityName = file.Name,
                RelatedEntityId = file.OwnerId,
                RelatedEntityType = MapFileOwnerTypeToEntityType(file.OwnerType),
                CreatorId = userId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return true;
    }

    public Task<IEnumerable<File>> ListByOwnerAsync(FileOwnerType ownerType, int ownerId)
        => _repo.ListByOwnerAsync(ownerType, ownerId);

    private EntityType MapFileOwnerTypeToEntityType(FileOwnerType ownerType)
    {
        return ownerType switch
        {
            FileOwnerType.Organization => EntityType.Organization,
            FileOwnerType.Project => EntityType.Project,
            FileOwnerType.Task => EntityType.Task,
            FileOwnerType.Sprint => EntityType.Sprint,
            _ => throw new ArgumentException($"Неизвестный тип владельца файла: {ownerType}")
        };
    }

    private HistoryEntityType MapFileOwnerTypeToHistoryEntityType(FileOwnerType ownerType)
    {
        return ownerType switch
        {
            FileOwnerType.Organization => HistoryEntityType.Organization,
            FileOwnerType.Project => HistoryEntityType.Project,
            FileOwnerType.Task => HistoryEntityType.Task,
            FileOwnerType.Sprint => HistoryEntityType.Sprint,
            FileOwnerType.Worker => HistoryEntityType.Worker,
            _ => throw new ArgumentException($"Неизвестный тип владельца файла: {ownerType}")
        };
    }

    private string GetHumanReadableEntityName(FileOwnerType ownerType)
    {
        return ownerType switch
        {
            FileOwnerType.Organization => "Организация",
            FileOwnerType.Project => "Проект",
            FileOwnerType.Task => "Задача",
            FileOwnerType.Sprint => "Спринт",
            FileOwnerType.Worker => "Сотрудник",
            _ => "Неизвестная сущность"
        };
    }
}