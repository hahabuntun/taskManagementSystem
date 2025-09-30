using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.Entities.Notification;

namespace Vkr.Application.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<SubscriptionDto>> GetSubscriptionsAsync(int workerId);
    Task<IEnumerable<SubscriptionDto>> GetEntitySubscriptions(int entityId, EntityType entityType);
    Task<bool> IsWorkerSubscribedAsync(int workerId, int entityId, EntityType entityType);
    Task<bool> SubscribeToEntityAsync(int workerId, int entityId, EntityType entityType);
    Task<bool> UnsubscribeFromEntityAsync(int workerId, int entityId, EntityType entityType);
    Task<IEnumerable<NotificationDto>> GetNotificationsAsync(int workerId, bool includeRead = false);
    Task<int> GetNotificationsCountAsync(int workerId, bool includeRead = false);
    Task<bool> ReadNotificationAsync(int notificationId, int workerId);
    Task<bool> ReadAllNotificationsAsync(int workerId);
    Task<bool> DeleteNotificationAsync(int notificationId, int workerId);
    Task<bool> CreateNotificationAsync(CreateNotificationDto notificationDto);
}