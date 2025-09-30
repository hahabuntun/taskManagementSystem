import { ISprintStatus } from "../../../interfaces/ISprintStatus";
import { Dayjs } from "dayjs";

export interface IAddSprintOptions {
    name: string;
    status: ISprintStatus;
    startDate?: Dayjs;
    endDate?: Dayjs;
}