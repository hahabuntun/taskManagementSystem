using System;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Notification;

namespace Vkr.Application.DTOs.NotificationDTOs;

public record SubscriptionDto
{
    public int Id { get; init; }
    public int WorkerId { get; init; }
    public int EntityId { get; init; }
    public EntityType EntityType { get; init; }
    public string EntityName { get; init; } = "Не известная сущность"; // Default value
}

public record CreateSubscriptionDto
{
    public int WorkerId { get; init; }
    public int EntityId { get; init; }
    public EntityType EntityType { get; init; }
}

public record NotificationDto
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public string RelatedEntityName { get; init; } = string.Empty;
    public int RelatedEntityId { get; init; }
    public EntityType RelatedEntityType { get; init; }
    public DateTime CreatedOn { get; init; }
    public int CreatorId { get; init; }
    public SimpleWorkerDTO Creator { get; init; }
    public bool IsRead { get; init; }
}

public record CreateNotificationDto
{
    public string Text { get; init; } = string.Empty;
    public string RelatedEntityName { get; init; } = string.Empty;
    public int RelatedEntityId { get; init; }
    public EntityType RelatedEntityType { get; init; }
    public int CreatorId { get; init; }
    public IEnumerable<int> WorkerIds { get; init; } = new List<int>();
}