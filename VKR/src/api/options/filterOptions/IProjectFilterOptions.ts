import { IProjectStatus } from "../../../interfaces/IProjectStatus";
import { ITag } from "../../../interfaces/ITag";
import { Dayjs } from "dayjs";
import { IWorkerFields } from "../../../interfaces/IWorkerFields";

export interface IProjectFilterOptions {
    name?: string;
    status?: IProjectStatus;
    manager?: IWorkerFields;
    tags?: ITag[];
    startedFrom?: Dayjs;
    startedTill?: Dayjs;
    endDateFrom?: Dayjs;
    endDateTill?: Dayjs;
    createdFrom?: Dayjs;
    createdTill?: Dayjs;
}