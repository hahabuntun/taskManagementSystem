import { Dayjs } from 'dayjs';
export interface IBoardFilterOptopns {
    name?: string;
    createdFrom?: Dayjs
    createdTill?: Dayjs;
}