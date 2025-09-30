import { TaskTypeEnum } from "../enums/TaskTypeEnum";
import { ITag } from "./ITag";
import { ITaskPriority } from "./ITaskPriority";

import { Dayjs } from "dayjs";
import { ITaskStatus } from "./ITaskStatus";

export interface ITaskTemplate {
    id: number,
    name: string;
    taskName?: string;
    description?: string;
    type?: TaskTypeEnum;
    priority?: ITaskPriority;
    status?: ITaskStatus;
    tags: ITag[];
    progress?: number;
    createdAt: Dayjs;
    startDate?: Dayjs;
    endDate?: Dayjs;
    storyPoints?: number;
}