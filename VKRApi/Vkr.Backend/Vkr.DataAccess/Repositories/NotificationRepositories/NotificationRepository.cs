using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.DataAccess;
using Vkr.Domain.DTO.Worker;
using Vkr.Domain.Entities.Notification;

namespace Vkr.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SubscriptionDto>> GetSubscriptionsAsync(int workerId)
    {
        // Fetch subscriptions
        var subscriptions = await _context.Subscriptions
            .Where(s => s.WorkerId == workerId)
            .Select(s => new SubscriptionDto
            {
                Id = s.Id,
                WorkerId = s.WorkerId,
                EntityId = s.EntityId,
                EntityType = s.EntityType,
                EntityName = ""
            })
            .ToListAsync();

        // Build SubscriptionDto list with EntityName
        var subscriptionDtos = new List<SubscriptionDto>();
        foreach (var s in subscriptions)
        {
            string entityName = "Unknown Entity";

            if (s.EntityType == EntityType.Task)
            {
                entityName = await _context.Tasks
                    .Where(t => t.Id == s.EntityId)
                    .Select(t => t.ShortName)
                    .FirstOrDefaultAsync() ?? "Unknown Task";
            }
            else if (s.EntityType == EntityType.Project)
            {
                entityName = await _context.Projects
                    .Where(p => p.Id == s.EntityId)
                    .Select(p => p.Name)
                    .FirstOrDefaultAsync() ?? "Unknown Project";
            }
            else if (s.EntityType == EntityType.Sprint)
            {
                entityName = await _context.Sprints
                    .Where(sp => sp.Id == s.EntityId)
                    .Select(sp => sp.Title)
                    .FirstOrDefaultAsync() ?? "Unknown Sprint";
            }
            else if (s.EntityType == EntityType.Board)
            {
                entityName = await _context.Boards
                    .Where(b => b.Id == s.EntityId)
                    .Select(b => b.Name)
                    .FirstOrDefaultAsync() ?? "Unknown Board";
            }
            else if (s.EntityType == EntityType.Organization)
            {
                entityName = await _context.Organizations
                    .Where(o => o.Id == s.EntityId)
                    .Select(o => o.Name)
                    .FirstOrDefaultAsync() ?? "Unknown Organization";
            }

            subscriptionDtos.Add(new SubscriptionDto
            {
                Id = s.Id,
                WorkerId = s.WorkerId,
                EntityId = s.EntityId,
                EntityType = s.EntityType,
                EntityName = entityName
            });
        }

        return subscriptionDtos;
    }

    public async Task<bool> IsWorkerSubscribedAsync(int workerId, int entityId, EntityType entityType)
    {
        return await _context.Subscriptions
            .AnyAsync(s => s.WorkerId == workerId && s.EntityId == entityId && s.EntityType == entityType);
    }

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveSubscriptionAsync(int workerId, int entityId, EntityType entityType)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.WorkerId == workerId && s.EntityId == entityId && s.EntityType == entityType);
        if (subscription != null)
        {
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<NotificationDto>> GetNotificationsAsync(int workerId, bool includeRead = false)
    {
        var query = _context.WorkerNotifications
            .Where(wn => wn.WorkerId == workerId)
            .Join(_context.Notifications,
                wn => wn.NotificationId,
                n => n.Id,
                (wn, n) => new NotificationDto
                {
                    Id = n.Id,
                    Text = n.Text,
                    RelatedEntityName = n.RelatedEntityName,
                    RelatedEntityId = n.RelatedEntityId,
                    RelatedEntityType = n.RelatedEntityType,
                    CreatedOn = n.CreatedOn,
                    Creator = new SimpleWorkerDTO
                    {
                        Id = n.Creator.Id,
                        Name = n.Creator.Name,
                        SecondName = n.Creator.SecondName,
                        Email = n.Creator.Email,
                        ThirdName = n.Creator.ThirdName
                    },
                    CreatorId = n.CreatorId,
                    IsRead = wn.IsRead
                });

        if (!includeRead)
        {
            query = query.Where(n => !n.IsRead);
        }

        return await query.OrderByDescending(n => n.CreatedOn).ToListAsync();
    }

    public async Task<int> GetNotificationsCountAsync(int workerId, bool includeRead = false)
    {
        var query = _context.WorkerNotifications
            .Where(wn => wn.WorkerId == workerId);

        if (!includeRead)
        {
            query = query.Where(wn => !wn.IsRead);
        }

        return await query.CountAsync();
    }

    public async Task MarkNotificationAsReadAsync(int notificationId, int workerId)
    {
        var workerNotification = await _context.WorkerNotifications
            .FirstOrDefaultAsync(wn => wn.NotificationId == notificationId && wn.WorkerId == workerId);
        if (workerNotification != null)
        {
            workerNotification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllNotificationsAsReadAsync(int workerId)
    {
        var workerNotifications = await _context.WorkerNotifications
            .Where(wn => wn.WorkerId == workerId && !wn.IsRead)
            .ToListAsync();

        if (workerNotifications.Any())
        {
            foreach (var wn in workerNotifications)
            {
                wn.IsRead = true;
            }
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteNotificationAsync(int notificationId, int workerId)
    {
        var workerNotification = await _context.WorkerNotifications
            .FirstOrDefaultAsync(wn => wn.NotificationId == notificationId && wn.WorkerId == workerId);
        if (workerNotification != null)
        {
            _context.WorkerNotifications.Remove(workerNotification);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddNotificationAsync(Notification notification, IEnumerable<int> workerIds)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        var workerNotifications = workerIds.Select(workerId => new WorkerNotification
        {
            NotificationId = notification.Id,
            WorkerId = workerId,
            IsRead = false
        }).ToList();

        await _context.WorkerNotifications.AddRangeAsync(workerNotifications);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsWorkerNotificationAsync(int notificationId, int workerId)
    {
        return await _context.WorkerNotifications
            .AnyAsync(wn => wn.NotificationId == notificationId && wn.WorkerId == workerId);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetEntitySubscriptions(int entityId, EntityType entityType)
    {
        return await _context.Subscriptions
        .Where(s => s.EntityId == entityId || s.EntityType == entityType)
            .Select(s => new SubscriptionDto
            {
                Id = s.Id,
                EntityId = s.EntityId,
                EntityName = "",
                EntityType = s.EntityType,
                WorkerId = s.WorkerId,
            })
            .ToListAsync();
    }
}