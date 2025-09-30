import { Dayjs } from "dayjs";

export interface IEditProjectOptions {
    name: string;
    description?: string;
    goal?: string;
    progress: number;
    startDate?: Dayjs;
    endDate?: Dayjs;
    statusId: number;
}