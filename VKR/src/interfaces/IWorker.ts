import { IWorkerPosition } from "./IWorkerPosition";
import { IWorkerStatus } from "./IWorkerStatus";
import { Dayjs } from "dayjs";

export interface IWorker {
    id: number;
    firstName: string;
    secondName: string;
    thirdName: string;
    email: string;
    createdAt: Dayjs;
    status: IWorkerStatus;
    workerPosition: IWorkerPosition;
    isAdmin: boolean;
    isManager: boolean;
}