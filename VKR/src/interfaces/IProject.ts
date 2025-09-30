import { ICheckList } from "./ICheckList";
import { IProjectMember } from "./IProjectMember";
import { IProjectStatus } from "./IProjectStatus";
import { ITag } from "./ITag";
import { IWorker } from "./IWorker";
import { Dayjs } from "dayjs";

export interface IProject {
    id: number;
    name: string;
    description: string;
    goal?: string;
    progress: number;
    status: IProjectStatus;
    createdAt: Dayjs;
    startDate?: Dayjs;
    endDate?: Dayjs;
    manager: IWorker;
    tags: ITag[];
    checklists: ICheckList[];
    members: IProjectMember[];
}

export interface IProjectFields {
    id: number;
    name: string;
    managerId: number;
}