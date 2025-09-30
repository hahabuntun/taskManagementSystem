import { NotificationOwnerEnum } from "../../../enums/ownerEntities/NotificationOwnerEnum";
import dayjs from "dayjs";
import { IWorkerFields } from "../../../interfaces/IWorkerFields";

export interface INotificationFilterOptions {
    type?: NotificationOwnerEnum;
    name?: string;
    message?: string;
    createdFrom?: dayjs.Dayjs;
    createdTill?: dayjs.Dayjs;
    isRead?: boolean;
    responsibleWorker?: IWorkerFields;
}