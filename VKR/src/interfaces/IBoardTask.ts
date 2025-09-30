import { ITask } from "./ITask";

export interface IBoardTask {
    boardId: number;
    taskId: number;
    customBoardColumnName?: string;
}

export interface IBoardTaskWithDetails extends IBoardTask {
    task: ITask;
}