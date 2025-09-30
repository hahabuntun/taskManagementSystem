import { Dayjs } from 'dayjs';
import { TaskTypeEnum } from "../../enums/TaskTypeEnum";
import { ITag } from "../../interfaces/ITag";
import { ITaskPriority } from "../../interfaces/ITaskPriority";
import { ITaskStatus } from "../../interfaces/ITaskStatus";

export interface ITaskTemplateOptions {
    name: string;
    taskName?: string;
    description?: string;
    type?: TaskTypeEnum;
    priority?: ITaskPriority;
    status?: ITaskStatus;
    tags: ITag[];
    progress?: number;
    startDate?: Dayjs;
    endDate?: Dayjs;
    storyPoints?: number;
    links: { name: string, description?: string }[]
}