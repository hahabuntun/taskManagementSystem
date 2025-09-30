import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { ITag } from "../../../interfaces/ITag";
import { ITaskPriority } from "../../../interfaces/ITaskPriority";
import { ITaskStatus } from "../../../interfaces/ITaskStatus";
import { Dayjs } from "dayjs";
import { IWorkerFields } from "../../../interfaces/IWorkerFields";

export interface ITaskFilterOptions {
    name?: string;
    description?: string;
    type?: TaskTypeEnum;
    createdFrom?: Dayjs;
    createdTill?: Dayjs;
    startedFrom?: Dayjs;
    startedTill?: Dayjs;
    endDateFrom?: Dayjs;
    endDateTill?: Dayjs;
    status?: ITaskStatus;
    priority?: ITaskPriority;
    creator?: IWorkerFields;
    tags?: ITag[];
}