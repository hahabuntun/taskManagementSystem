import { TaskWorkerTypeEnum } from "../../../enums/TaskWorkerTypeEnum";

export interface IAddTaskWorkerOptions {
    workerId: number;
    type: TaskWorkerTypeEnum;
    canReviewTask: boolean;
}