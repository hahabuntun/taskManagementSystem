import { TaskStatusEnum } from "../enums/statuses/TaskStatusEnum";
import { ISprint } from "./ISprint";


export const statusNameMapping: Record<string, TaskStatusEnum> = {
    "В ожидании": TaskStatusEnum.PENDING,
    "В работе": TaskStatusEnum.IN_WORK,
    "На проверке": TaskStatusEnum.AWAITING_CHECK,
    "Завершена": TaskStatusEnum.COMPLETED,
    "Приостановлен": TaskStatusEnum.ARCHIVED,
};
// types/analytics.ts
export interface ProjectAnalyticsData {
    tasksByStatus: Record<string, number>;
    tasksByPriority: Record<string, number>;
    tasksByTag: Record<string, number>;
    tasksByEmployee: Array<{ email: string; statuses: Record<string, number> }>;
    tasksBySprint: Array<{ sprint: ISprint; completedTasks: number }>;
    overdueTasks: number; // Добавлено поле для просроченных задач
}

export interface SprintAnalyticsData {
    tasksByStatus: Record<string, number>;
    tasksByPriority: Record<string, number>;
    tasksByTag: Record<string, number>;
    tasksByEmployee: Array<{ email: string; statuses: Record<string, number> }>;
    totalTasks: number;
    overdueTasks: number; // Добавлено поле для просроченных задач
}

export interface WorkerAnalyticsData {
    tasksByProject: Array<{
        projectId: number;
        projectName: string;
        count: number;
        statuses: Record<string, number>; // Добавлено: количество задач по статусам
        overdue: number; // Добавлено: количество просроченных задач
    }>;
    tasksBySprint: Array<{ sprintId: number; count: number }>;
    tasksByTag: Record<string, number>;
    tasksByStatus: Record<string, number>;
    tasksByPriority: Record<string, number>;
    overdueTasks: number;
}

export interface OrganizationAnalyticsData {
    tasksByStatus: Record<string, number>;
    tasksByPriority: Record<string, number>;
    tasksByTag: Record<string, number>;
    tasksByProject: Array<{ projectName: string; completed: number; overdue: number }>;
    tasksByEmployee: Array<{ email: string; statuses: Record<string, number> }>;
    overdueTasks: number; // Добавлено поле для просроченных задач
}

interface ApiPriority { Id: number; Name: string; Color: string; }

interface ApiTag { Id: number; Name: string; Color: string; }

interface ApiWorker { Id: number; Name: string; SecondName: string; Email: string; }

interface ApiStatus { Id: number; Name: string; Color: string; }

interface ApiSprint {
    Id: number;
    Title: string;
    Goal: string;
    CreatedOn: string;
    StartsOn?: string;
    ExpireOn?: string;
    ProjectId: number;
    ProjectName: string;
    SprintStatusId: number;
    SprintStatus: { Id: number; Name: string; Color: string };
}

export interface ApiOrganizationAnalyticsResponse {

    TasksByStatus: Array<{ Status: ApiStatus; Count: number }>;
    TasksByPriority: Array<{ Priority: ApiPriority; Count: number }>;
    TasksByTag: Array<{ Tag: ApiTag; Count: number }>;
    TasksByProject: Array<{ Item1: string; Item2: number; Item3: number }>;
    TasksByEmployee: Array<{
        Item1: ApiWorker; Item2:
        Array<{ Status: ApiStatus; Count: number }>
    }>;
    OverdueTasks: number;
}


export interface ApiProjectAnalyticsResponse {
    TasksByStatus: Array<{ Status: ApiStatus; Count: number }>;
    TasksByPriority: Array<{ Priority: ApiPriority; Count: number }>;
    TasksByTag: Array<{ Tag: ApiTag; Count: number }>;
    TasksByEmployee: Array<{
        Item1: ApiWorker;
        Item2: Array<{ Status: ApiStatus; Count: number }>;
    }>;
    TasksBySprint: Array<{ Sprint: ApiSprint; completedTasksNum: number }>;
    OverdueTasks: number;
}

export interface ApiSprintAnalyticsResponse {
    TasksByStatus: Array<{ Status: ApiStatus; Count: number }>;
    TasksByPriority: Array<{ Priority: ApiPriority; Count: number }>;
    TasksByTag: Array<{ Tag: ApiTag; Count: number }>;
    TasksByEmployee: Array<{ Item1: ApiWorker; Item2: Array<{ Status: ApiStatus; Count: number }> }>;
    TotalTasks: number;
    OverdueTasks: number;
}

export interface ApiWorkerAnalyticsResponse {
    TasksByProject: Array<{ Item1: number; Item2: string; Item3: number; Item4: Array<{ Status: ApiStatus; Count: number }>; Item5: number; }>;
    TasksBySprint: Array<{ Item1: number; Item2: number }>; TasksByTag: Array<{ Tag: ApiTag; Count: number }>;
    TasksByStatus: Array<{ Status: ApiStatus; Count: number }>;
    TasksByPriority: Array<{ Priority: ApiPriority; Count: number }>;
    OverdueTasks: number;
}

