import { Dayjs } from "dayjs";

export interface IAddProjectOptions {
    name: string;
    statusId: number;
    description: string;
    progress: number;
    goal?: string;
    startDate?: Dayjs;
    endDate?: Dayjs;
}