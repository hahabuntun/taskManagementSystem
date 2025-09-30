import dayjs from 'dayjs';
import { NotificationOwnerEnum } from '../enums/ownerEntities/NotificationOwnerEnum';
import { IWorkerFields } from './IWorkerFields';

export interface INotification {
    id: number;
    relatedEntity: {
        id: number;
        name: string;
        type: NotificationOwnerEnum;
    };
    responsibleWorker: IWorkerFields;
    message: string;
    createdAt: dayjs.Dayjs;
    isRead: boolean;
}