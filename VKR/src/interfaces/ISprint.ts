import { ISprintStatus } from "./ISprintStatus";
import { Dayjs } from "dayjs";

export interface ISprint {
    id: number;
    name: string;
    projectId: number;
    status: ISprintStatus;
    startDate?: Dayjs;
    endDate?: Dayjs;
    goal?: string;
}