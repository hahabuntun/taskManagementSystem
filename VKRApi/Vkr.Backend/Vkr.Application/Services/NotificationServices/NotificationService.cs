using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Application.Interfaces;
using Vkr.Domain.Entities.Notification;
using Vkr.Infrastructure.Repositories;

namespace Vkr.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<IEnumerable<SubscriptionDto>> GetSubscriptionsAsync(int workerId)
    {
        if (workerId < 0) throw new ArgumentException("Invalid worker ID");
        return await _notificationRepository.GetSubscriptionsAsync(workerId);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetEntitySubscriptions(int entityId, EntityType entityType)
    {
        return await _notificationRepository.GetEntitySubscriptions(entityId, entityType);
    }

    public async Task<bool> IsWorkerSubscribedAsync(int workerId, int entityId, EntityType entityType)
    {
        if (workerId <= 0 || entityId <= 0) throw new ArgumentException("Invalid worker ID or entity ID");
        return await _notificationRepository.IsWorkerSubscribedAsync(workerId, entityId, entityType);
    }

    public async Task<bool> SubscribeToEntityAsync(int workerId, int entityId, EntityType entityType)
    {
        if (workerId <= 0 || entityId <= 0) throw new ArgumentException("Invalid worker ID or entity ID");

        var isSubscribed = await _notificationRepository.IsWorkerSubscribedAsync(workerId, entityId, entityType);
        if (isSubscribed) return false; // Already subscribed, no action needed

        var subscription = new Subscription
        {
            WorkerId = workerId,
            EntityId = entityId,
            EntityType = entityType
        };

        await _notificationRepository.AddSubscriptionAsync(subscription);
        return true; // Successfully subscribed
    }

    public async Task<bool> UnsubscribeFromEntityAsync(int workerId, int entityId, EntityType entityType)
    {
        if (workerId <= 0 || entityId <= 0) throw new ArgumentException("Invalid worker ID or entity ID");

        var isSubscribed = await _notificationRepository.IsWorkerSubscribedAsync(workerId, entityId, entityType);
        if (!isSubscribed) return false; // Not subscribed, no action needed

        await _notificationRepository.RemoveSubscriptionAsync(workerId, entityId, entityType);
        return true; // Successfully unsubscribed
    }

    public async Task<IEnumerable<NotificationDto>> GetNotificationsAsync(int workerId, bool includeRead = false)
    {
        if (workerId <= 0) throw new ArgumentException("Invalid worker ID");
        return await _notificationRepository.GetNotificationsAsync(workerId, includeRead);
    }

    public async Task<int> GetNotificationsCountAsync(int workerId, bool includeRead = false)
    {
        if (workerId <= 0) throw new ArgumentException("Invalid worker ID");
        return await _notificationRepository.GetNotificationsCountAsync(workerId, includeRead);
    }

    public async Task<bool> ReadNotificationAsync(int notificationId, int workerId)
    {
        if (notificationId <= 0 || workerId <= 0) throw new ArgumentException("Invalid notification ID or worker ID");

        var exists = await _notificationRepository.ExistsWorkerNotificationAsync(notificationId, workerId);
        if (!exists) return false; // Notification not found for this worker

        await _notificationRepository.MarkNotificationAsReadAsync(notificationId, workerId);
        return true; // Successfully marked as read
    }

    public async Task<bool> ReadAllNotificationsAsync(int workerId)
    {
        if (workerId <= 0) throw new ArgumentException("Invalid worker ID");

        await _notificationRepository.MarkAllNotificationsAsReadAsync(workerId);
        return true; // Successfully marked all as read
    }

    public async Task<bool> DeleteNotificationAsync(int notificationId, int workerId)
    {
        if (notificationId <= 0 || workerId <= 0) throw new ArgumentException("Invalid notification ID or worker ID");

        var exists = await _notificationRepository.ExistsWorkerNotificationAsync(notificationId, workerId);
        if (!exists) return false; // Notification not found for this worker

        await _notificationRepository.DeleteNotificationAsync(notificationId, workerId);
        return true; // Successfully deleted
    }

    public async Task<bool> CreateNotificationAsync(CreateNotificationDto notificationDto)
    {
        if (notificationDto == null || string.IsNullOrWhiteSpace(notificationDto.Text) || 
            string.IsNullOrWhiteSpace(notificationDto.RelatedEntityName) || 
            notificationDto.RelatedEntityId <= 0 || notificationDto.CreatorId <= 0)
        {
            throw new ArgumentException("Invalid notification data");
        }

        var notification = new Notification
        {
            Text = notificationDto.Text,
            RelatedEntityName = notificationDto.RelatedEntityName,
            RelatedEntityId = notificationDto.RelatedEntityId,
            RelatedEntityType = notificationDto.RelatedEntityType,
            CreatorId = notificationDto.CreatorId,
            CreatedOn = DateTime.UtcNow,
            IsRead = false
        };

        await _notificationRepository.AddNotificationAsync(notification, notificationDto.WorkerIds);
        return true; // Successfully created
    }
}