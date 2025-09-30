using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Application.Interfaces;
using Vkr.Domain.Entities.Notification;

namespace Vkr.API.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("subscriptions/{workerId}")]
    public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetSubscriptions(int workerId)
    {
        try
        {
            var subscriptions = await _notificationService.GetSubscriptionsAsync(workerId);
            return Ok(subscriptions);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("is-subscribed/{workerId}/{entityId}/{entityType}")]
    public async Task<ActionResult<bool>> IsWorkerSubscribedToEntity(int workerId, int entityId, EntityType entityType)
    {
        try
        {
            var isSubscribed = await _notificationService.IsWorkerSubscribedAsync(workerId, entityId, entityType);
            return Ok(isSubscribed);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult<bool>> SubscribeToEntity([FromBody] CreateSubscriptionDto subscriptionDto)
    {
        try
        {
            await _notificationService.SubscribeToEntityAsync(subscriptionDto.WorkerId, subscriptionDto.EntityId, subscriptionDto.EntityType);
            return Ok(true);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("unsubscribe")]
    public async Task<ActionResult<bool>> UnsubscribeFromEntity([FromBody] CreateSubscriptionDto subscriptionDto)
    {
        try
        {
            await _notificationService.UnsubscribeFromEntityAsync(subscriptionDto.WorkerId, subscriptionDto.EntityId, subscriptionDto.EntityType);
            return Ok(true);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{workerId}")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications(int workerId, [FromQuery] bool includeRead = false)
    {
        try
        {
            var notifications = await _notificationService.GetNotificationsAsync(workerId, includeRead);
            return Ok(notifications);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("count/{workerId}")]
    public async Task<ActionResult<int>> GetNotificationsCount(int workerId, [FromQuery] bool includeRead = false)
    {
        try
        {
            var count = await _notificationService.GetNotificationsCountAsync(workerId, includeRead);
            return Ok(count);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("read/{notificationId}/{workerId}")]
    public async Task<ActionResult<bool>> ReadNotification(int notificationId, int workerId)
    {
        try
        {
            await _notificationService.ReadNotificationAsync(notificationId, workerId);
            return Ok(true);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{notificationId}/{workerId}")]
    public async Task<ActionResult<bool>> DeleteNotification(int notificationId, int workerId)
    {
        try
        {
            await _notificationService.DeleteNotificationAsync(notificationId, workerId);
            return Ok(true);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CreateNotification([FromBody] CreateNotificationDto notificationDto)
    {
        try
        {
            await _notificationService.CreateNotificationAsync(notificationDto);
            return Ok(true);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}