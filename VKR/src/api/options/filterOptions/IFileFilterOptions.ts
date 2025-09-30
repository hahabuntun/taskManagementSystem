import { Dayjs } from "dayjs";
import { IWorkerFields } from "../../../interfaces/IWorkerFields";

export interface IFileFilterOptions {
    name?: string;
    creator?: IWorkerFields;
    createdFrom?: Dayjs;
    createdTill?: Dayjs;
}