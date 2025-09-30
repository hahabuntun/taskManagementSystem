import { FileOwnerEnum } from "../enums/ownerEntities/FileOwnerEnum";

import { Dayjs } from "dayjs";
import { IWorkerFields } from "./IWorkerFields";

export interface IFile {
    id: number;
    name: string;
    createdAt: Dayjs;
    description: string;
    size: number;
    owner: {
        type: FileOwnerEnum;
        name: string;
        id: number;
    }
    creator: IWorkerFields;
}