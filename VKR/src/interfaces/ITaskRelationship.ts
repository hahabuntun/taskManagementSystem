import { TaskRelationshipTypeEnum } from "../enums/TaskRelationshipTypeEnum";

export interface ITaskRelationship {
    taskId: number;
    relatedTaskId: number;
    relationType: TaskRelationshipTypeEnum;
    lag?: number; // in days, can be negative for lead time
}