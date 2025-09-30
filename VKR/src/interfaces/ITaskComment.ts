import { Dayjs } from "dayjs";
import { IWorkerFields } from "./IWorkerFields";

export interface ITaskComment {
    id: number;
    text: string;
    createdAt: Dayjs;
    creator: IWorkerFields;
    taskId: number;
}