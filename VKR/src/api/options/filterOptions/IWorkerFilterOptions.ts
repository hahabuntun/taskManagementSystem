import { IWorkerPosition } from "../../../interfaces/IWorkerPosition";
import { IWorkerStatus } from "../../../interfaces/IWorkerStatus";
import { Dayjs } from "dayjs";

export interface IWorkerFilterOptions {
    firstName?: string;
    secondName?: string;
    thirdName?: string;
    email?: string;
    createdFrom?: Dayjs;
    createdTill?: Dayjs;
    status?: IWorkerStatus;
    workerPosition?: IWorkerPosition;
    isAdmin?: boolean;
    isManager?: boolean;
}