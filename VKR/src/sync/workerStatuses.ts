import { WorkerStatusEnum } from "../enums/statuses/WorkerStatusEnum";
import { IWorkerStatus } from "../interfaces/IWorkerStatus";

export const workerStatuses: IWorkerStatus[] = [
    { id: 0, name: WorkerStatusEnum.ACTIVE, color: "#95de64" },
    { id: 1, name: WorkerStatusEnum.BLOCKED, color: "#cf1322" },
];
