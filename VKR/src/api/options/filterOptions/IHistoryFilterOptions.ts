import { Dayjs } from "dayjs";
import { IWorkerFields } from "../../../interfaces/IWorkerFields";

export interface IHistoryFilterOptions {
    text?: string;
    responsibleWorker?: IWorkerFields;
    createdFrom?: Dayjs;
    createdTill?: Dayjs;
}