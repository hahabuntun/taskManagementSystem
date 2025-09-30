import { TaskTypeEnum } from "../enums/TaskTypeEnum";
import { ICheckList } from "./ICheckList";
import { IProjectFields } from "./IProject";
import { ISprint } from "./ISprint";
import { ITag } from "./ITag";
import { ITaskPriority } from "./ITaskPriority";
import { ITaskRelationship } from "./ITaskRelationship";
import { ITaskStatus } from "./ITaskStatus";
import { ITaskWorker } from "./ITaskWorker";
import { Dayjs } from "dayjs";
import { IWorkerFields } from "./IWorkerFields";

export interface ITask {
    id: number;
    name: string;
    description: string;
    type: TaskTypeEnum;
    progress: number;
    project: IProjectFields;
    createdAt: Dayjs;
    startDate?: Dayjs;
    endDate?: Dayjs;
    status: ITaskStatus;
    priority: ITaskPriority;
    creator: IWorkerFields;
    tags: ITag[];
    workers: ITaskWorker[];
    sprint?: ISprint;
    storyPoints?: number;
    checklists: ICheckList[];
    relationships: ITaskRelationship[];
}



