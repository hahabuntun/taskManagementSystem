import { ISprintStatus } from "../../../interfaces/ISprintStatus";
import { Dayjs } from "dayjs"; Dayjs

export interface IEditSprintOptions {
    name: string;
    goal?: string;
    status: ISprintStatus;
    startDate?: Dayjs;
    endDate?: Dayjs;
}