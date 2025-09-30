import { Dayjs } from 'dayjs';
import { TaskTypeEnum } from '../../../enums/TaskTypeEnum';
import { ITaskStatus } from '../../../interfaces/ITaskStatus';
import { ITaskPriority } from '../../../interfaces/ITaskPriority';
import { ITag } from '../../../interfaces/ITag';
export interface ITaskTemplateFilterOptions {
    name?: string;
    taskName?: string;
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
    tags?: ITag[];
}