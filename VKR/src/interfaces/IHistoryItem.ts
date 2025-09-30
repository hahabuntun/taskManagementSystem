import { HistoryOwnerEnum } from "../enums/ownerEntities/HistoryOwnerEnum";
import { Dayjs } from "dayjs";
import { IWorkerFields } from "./IWorkerFields";

export interface IHistoryItem {
    id: number;
    message: string;
    createdAt: Dayjs;
    responsibleWorker: IWorkerFields;
    owner: { // task, project, organization, worker
        type: HistoryOwnerEnum;
        id: number;
    }
}