import { IWorker } from "./IWorker";
import { Dayjs } from "dayjs";

export interface IProjectMember {
    workerData: IWorker,
    taskTakers: number[],
    createdAt: Dayjs
}