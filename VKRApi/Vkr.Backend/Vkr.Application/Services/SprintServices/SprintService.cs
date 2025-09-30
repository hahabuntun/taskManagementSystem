using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Repositories.SprintRepositories;
using Vkr.Application.Interfaces.Services.SprintServices;
using Vkr.Domain.DTO;
using Vkr.Domain.DTO.Sprint;
using Vkr.Domain.DTO.Tag;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Notification;
using Vkr.Domain.Entities.Sprint;
using Vkr.Domain.Entities.Task;
using Vkr.Application.Interfaces.Services.HistoryServices;
using Vkr.Domain.Entities.History;

namespace Vkr.Application.Services.SprintServices;

public class SprintService : ISprintService
{
    private readonly ISprintRepository _repo;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService;

    public SprintService(
        ISprintRepository repo,
        INotificationService notificationService,
        IHistoryService historyService)
    {
        _repo = repo;
        _notificationService = notificationService;
        _historyService = historyService;
    }

    public async Task<int> CreateAsync(SprintCreateDTO dto, int userId)
    {
        var sprintId = await _repo.AddAsync(dto);
        var sprint = await _repo.GetByIdAsync(sprintId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Спринт '{sprint.Title}' создан в проекте с ID {sprint.ProjectId}.",
            RelatedEntityId = sprintId,
            RelatedEntityType = HistoryEntityType.Sprint,
            CreatorId = userId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(sprint.ProjectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == sprint.ProjectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Спринт '{sprint.Title}' создан в проекте с ID {sprint.ProjectId}.",
                RelatedEntityName = sprint.Title,
                RelatedEntityId = sprintId,
                RelatedEntityType = EntityType.Sprint,
                CreatorId = userId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return sprintId;
    }

    public async Task DeleteAsync(int sprintId, int userId)
    {
        var sprint = await _repo.GetByIdAsync(sprintId)
            ?? throw new KeyNotFoundException($"Спринт с ID {sprintId} не найден");

        await _repo.DeleteAsync(sprintId);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Спринт '{sprint.Title}' удален из проекта с ID {sprint.ProjectId}.",
            RelatedEntityId = sprintId,
            RelatedEntityType = HistoryEntityType.Sprint,
            CreatorId = userId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(sprint.ProjectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == sprint.ProjectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Спринт '{sprint.Title}' удален из проекта с ID {sprint.ProjectId}.",
                RelatedEntityName = sprint.Title,
                RelatedEntityId = sprintId,
                RelatedEntityType = EntityType.Sprint,
                CreatorId = userId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task UpdateAsync(SprintUpdateDTO dto)
    {
        var sprint = await _repo.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Спринт с ID {dto.Id} не найден");

        await _repo.UpdateAsync(dto);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Спринт '{sprint.Title}' обновлен в проекте с ID {sprint.ProjectId}.",
            RelatedEntityId = dto.Id,
            RelatedEntityType = HistoryEntityType.Sprint,
            CreatorId = 0 // Assuming creator ID is not available
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification
        var subscriptions = await _notificationService.GetEntitySubscriptions(sprint.ProjectId, EntityType.Project);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Project && s.EntityId == sprint.ProjectId)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Спринт '{sprint.Title}' обновлен в проекте с ID {sprint.ProjectId}.",
                RelatedEntityName = sprint.Title,
                RelatedEntityId = dto.Id,
                RelatedEntityType = EntityType.Sprint,
                CreatorId = 0, // Assuming creator ID is not available
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }
    }

    public async Task<SprintDTO> GetByIdAsync(int id)
    {
        var sprint = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Спринт с ID {id} не найден");
        return MapToSprintDTO(sprint);
    }

    public async Task<IEnumerable<SprintDTO>> GetByProjectAsync(int projectId)
    {
        var sprints = await _repo.GetByProjectAsync(projectId);
        return sprints.Select(MapToSprintDTO).ToList();
    }

    public async Task<IEnumerable<SprintDTO>> GetByWorkerAsync(int workerId)
    {
        var sprints = await _repo.GetByWorkerAsync(workerId);
        return sprints.Select(MapToSprintDTO).ToList();
    }

    private SprintDTO MapToSprintDTO(Sprints sprint)
    {
        return new SprintDTO
        {
            Id = sprint.Id,
            Title = sprint.Title,
            Goal = sprint.Goal,
            CreatedOn = sprint.CreatedOn,
            StartsOn = sprint.StartsOn,
            ExpireOn = sprint.ExpireOn,
            ProjectId = sprint.ProjectId,
            SprintStatusId = sprint.SprintStatusId,
            SprintStatus = new SprintStatusDTO
            {
                Id = sprint.SprintStatus.Id,
                Name = sprint.SprintStatus.Name,
                Color = sprint.SprintStatus.Color
            },
        };
    }
}