import { TaskWorkerTypeEnum } from "../enums/TaskWorkerTypeEnum";
import { IWorkerFields } from "./IWorkerFields";

export interface ITaskWorker {
    workerData: IWorkerFields
    isResponsible: boolean;
    taskWorkerType: TaskWorkerTypeEnum;
}

export interface ITaskWorkerForm {
    workerId: number;
    taskWorkerType: TaskWorkerTypeEnum;
    isResponsible: boolean;
}

export interface ITaskExecutorForm {
    workerId: number;
    isResponsible: boolean;
}

export interface ITaskObserverForm {
    workerId: number;
}