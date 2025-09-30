import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { ITaskPriority } from "../../../interfaces/ITaskPriority";
import { ITaskStatus } from "../../../interfaces/ITaskStatus";
import { Dayjs } from "dayjs";
import { ITaskWorker } from "../../../interfaces/ITaskWorker";

export interface IEditTaskOptions {
    name: string;
    projectId: number;
    description?: string;
    type: TaskTypeEnum;
    status: ITaskStatus;
    priority: ITaskPriority;
    progress: number;
    endDate?: Dayjs;
    startDate?: Dayjs;
    workers: ITaskWorker[];
    sprintId?: number;
    storyPoints?: number;
}