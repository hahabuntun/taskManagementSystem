import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { ITaskPriority } from "../../../interfaces/ITaskPriority";
import { ITaskStatus } from "../../../interfaces/ITaskStatus";

import { Dayjs } from "dayjs";
import { ITaskWorker } from "../../../interfaces/ITaskWorker";
import { ISprint } from "../../../interfaces/ISprint";

export interface IAddTaskOptions {
    name: string;
    progress: number;
    description: string;
    status: ITaskStatus;
    type: TaskTypeEnum;
    priority: ITaskPriority;
    workers: ITaskWorker[];
    sprint?: ISprint;
    startDate?: Dayjs;
    endDate?: Dayjs;
    storyPoints?: number;
    existingTagIds: number[]
}