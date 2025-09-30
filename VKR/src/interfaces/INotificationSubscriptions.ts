import { NotificationOwnerEnum } from "../enums/ownerEntities/NotificationOwnerEnum";

export interface INotificationSubscription {
    id: number;
    relatedEntity: {
        id: number;
        name: string;
        type: NotificationOwnerEnum;
    };
}