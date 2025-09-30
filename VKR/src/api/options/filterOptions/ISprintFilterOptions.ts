import { Dayjs } from 'dayjs';
import { ISprintStatus } from '../../../interfaces/ISprintStatus';
export interface ISprintFilterOptions {
    name?: string;
    startDate?: Dayjs;
    endDate?: Dayjs;
    status?: ISprintStatus;
}