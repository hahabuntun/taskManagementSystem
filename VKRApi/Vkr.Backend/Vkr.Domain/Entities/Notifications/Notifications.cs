using System;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Domain.Entities.Notification;

public enum EntityType
{
    Task,
    Project,
    Sprint,
    Board,
    Organization
}

public class Subscription
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public Workers Worker { get; set; } = null!;
    public int EntityId { get; set; }
    public EntityType EntityType { get; set; }
}

public class Notification
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string RelatedEntityName { get; set; } = string.Empty;
    public int RelatedEntityId { get; set; }
    public EntityType RelatedEntityType { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatorId { get; set; }
    public Workers Creator { get; set; } = null!;
    public bool IsRead { get; set; } = false;
    public List<WorkerNotification> WorkerNotifications { get; set; } = new();
}

public class WorkerNotification
{
    public int NotificationId { get; set; }
    public Notification Notification { get; set; } = null!;
    public int WorkerId { get; set; }
    public Workers Worker { get; set; } = null!;
    public bool IsRead { get; set; } = false;
}