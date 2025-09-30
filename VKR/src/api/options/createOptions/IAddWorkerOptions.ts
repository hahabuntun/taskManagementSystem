import { IWorkerPosition } from "../../../interfaces/IWorkerPosition";
import { IWorkerStatus } from "../../../interfaces/IWorkerStatus";

export interface IAddWorkerOptions {
    firstName: string;
    secondName: string;
    thirdName: string;
    email: string;
    password: string;
    status: IWorkerStatus;
    workerPosition: IWorkerPosition;
    isAdmin: boolean;
    isManager: boolean;
}