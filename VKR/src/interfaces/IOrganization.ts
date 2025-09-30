import { Dayjs } from "dayjs";
import { IWorkerFields } from "./IWorkerFields";


export interface IOrganization {
    id: number;
    name: string;
    owner: IWorkerFields;
    description: string;
    createdAt: Dayjs;
}