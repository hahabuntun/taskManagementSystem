import { filterNotifications } from './../dummyData/filters';
import { NotificationOwnerEnum } from "../../enums/ownerEntities/NotificationOwnerEnum";
import { INotificationFilterOptions } from "../options/filterOptions/INotificationFilterOptions";
import { INotification } from '../../interfaces/INotification';

import dayjs from "dayjs";
import { INotificationSubscription } from '../../interfaces/INotificationSubscriptions';
import { apiClient } from '../../config/axiosConfig';


// Map numeric backend EntityType to frontend NotificationOwnerEnum
const mapEntityTypeToNotificationOwner = (entityType: number): NotificationOwnerEnum => {
    switch (entityType) {
        case 0: // Task
            return NotificationOwnerEnum.TASK;
        case 1: // Project
            return NotificationOwnerEnum.PROJECT;
        case 3: // Board
            return NotificationOwnerEnum.BOARD;
        case 2: // Sprint
            return NotificationOwnerEnum.SPRINT;
        case 4: // Organization
            return NotificationOwnerEnum.ORGANIZATION;
        default:
            throw new Error(`Unsupported EntityType: ${entityType}`);
    }
};

// Map frontend NotificationOwnerEnum to numeric backend EntityType
const mapNotificationOwnerToEntityType = (ownerType: NotificationOwnerEnum): number => {
    switch (ownerType) {
        case NotificationOwnerEnum.TASK:
            return 0;
        case NotificationOwnerEnum.PROJECT:
            return 1;
        case NotificationOwnerEnum.BOARD:
            return 3;
        case NotificationOwnerEnum.SPRINT:
            return 2;
        case NotificationOwnerEnum.ORGANIZATION:
            return 4;
        default:
            throw new Error(`Unsupported NotificationOwnerEnum: ${ownerType}`);
    }
};

// Transform backend NotificationDto to frontend INotification
const transformNotificationDto = (dto: any): INotification => ({
    id: dto.Id,
    relatedEntity: {
        id: dto.RelatedEntityId,
        name: dto.RelatedEntityName,
        type: mapEntityTypeToNotificationOwner(dto.RelatedEntityType),
    },
    responsibleWorker: {
        id: dto.Creator.Id,
        firstName: dto.Creator.Name,
        secondName: dto.Creator.SecondName,
        thirdName: dto.Creator.ThirdName,
        email: dto.Creator.Email,
    },
    message: dto.Text,
    createdAt: dayjs(dto.CreatedOn),
    isRead: dto.IsRead,
});

// Transform backend SubscriptionDto to frontend INotificationSubscription
const transformSubscriptionDto = (dto: any): INotificationSubscription => ({
    id: dto.Id,
    relatedEntity: {
        id: dto.EntityId,
        name: dto.EntityName || `Entity ${dto.EntityId}`,
        type: mapEntityTypeToNotificationOwner(dto.EntityType),
    },
});

export const getAllSubscriptions = async (workerId: number): Promise<INotificationSubscription[]> => {
    const response = await apiClient.get(`/notifications/subscriptions/${workerId}`);
    return response.data.map(transformSubscriptionDto);

};

export const getAllNotifications = async (workerId: number, filters: INotificationFilterOptions): Promise<INotification[]> => {
    const response = await apiClient.get(`/notifications/${workerId}?includeRead=${true}`);
    return filterNotifications((response.data || [])
        .map(transformNotificationDto)
        .sort((a: INotification, b: INotification) =>
            b.createdAt.isAfter(a.createdAt) ? 1 : -1
        ), filters, 1);

};

export const subscribeToEntity = async (
    workerId: number,
    entityId: number,
    entityType: NotificationOwnerEnum
): Promise<boolean> => {
    const response = await apiClient.post('/notifications/subscribe', {
        workerId: workerId,
        entityId,
        entityType: mapNotificationOwnerToEntityType(entityType),
    });
    return response.data === true;

};

export const unsubscribeFromEntity = async (
    workerId: number,
    entityId: number,
    entityType: NotificationOwnerEnum
): Promise<boolean> => {
    const response = await apiClient.post('/notifications/unsubscribe', {
        workerId: workerId,
        entityId,
        entityType: mapNotificationOwnerToEntityType(entityType),
    });
    return response.data === true;

};

export const getIsSubscribedToEntity = async (
    workerId: number,
    entityId: number,
    entityType: NotificationOwnerEnum
): Promise<boolean> => {
    const response = await apiClient.get(
        `/notifications/is-subscribed/${workerId}/${entityId}/${mapNotificationOwnerToEntityType(entityType)}`
    );
    return response.data === true;

};

export const markNotificationAsRead = async (workerId: number, notificationId: number): Promise<boolean> => {
    const response = await apiClient.put(`/notifications/read/${notificationId}/${workerId}`);
    return response.data === true;

};

export const deleteNotification = async (workerId: number, notificationId: number): Promise<boolean> => {
    const response = await apiClient.delete(`/notifications/${notificationId}/${workerId}`);
    return response.data === true;
};