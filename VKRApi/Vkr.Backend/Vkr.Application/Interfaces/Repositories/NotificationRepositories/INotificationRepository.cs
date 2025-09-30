using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.Entities.Notification;

namespace Vkr.Infrastructure.Repositories;

public interface INotificationRepository
{
    Task<IEnumerable<SubscriptionDto>> GetSubscriptionsAsync(int workerId);
    Task<IEnumerable<SubscriptionDto>> GetEntitySubscriptions(int entityId, EntityType entityType);
    Task<bool> IsWorkerSubscribedAsync(int workerId, int entityId, EntityType entityType);
    Task AddSubscriptionAsync(Subscription subscription);
    Task RemoveSubscriptionAsync(int workerId, int entityId, EntityType entityType);
    Task<IEnumerable<NotificationDto>> GetNotificationsAsync(int workerId, bool includeRead = false);
    Task<int> GetNotificationsCountAsync(int workerId, bool includeRead = false);
    Task MarkNotificationAsReadAsync(int notificationId, int workerId);
    Task MarkAllNotificationsAsReadAsync(int workerId);
    Task DeleteNotificationAsync(int notificationId, int workerId);
    Task AddNotificationAsync(Notification notification, IEnumerable<int> workerIds);
    Task<bool> ExistsWorkerNotificationAsync(int notificationId, int workerId);
}