import { IEditTaskOptions } from './../options/editOptions/IEditTaskOptions';
import dayjs from 'dayjs';
import { filterHistory } from "./../dummyData/filters";
import { ISprint } from "../../interfaces/ISprint";
import { ITask } from "../../interfaces/ITask";
import { IEditSprintOptions } from "../options/editOptions/IEditSprintOptions";
import { ITaskFilterOptions } from "../options/filterOptions/ITaskFilterOptions";
import { IHistoryItem } from "../../interfaces/IHistoryItem";
import { HistoryOwnerEnum } from '../../enums/ownerEntities/HistoryOwnerEnum';
import { IHistoryFilterOptions } from '../options/filterOptions/IHistoryFilterOptions';
import { IWorkerFields } from '../../interfaces/IWorkerFields';
import { ApiSprintAnalyticsResponse, SprintAnalyticsData } from '../../interfaces/analytics';
import { apiClient } from '../../config/axiosConfig';
import { getProjectTasksQuery } from './projectsQueries';
import { editTaskQuery } from './tasksQueries';
import { sprintStatuses } from '../../sync/sprintStatuses';
import { getWorkersQuery } from './workersQueries';



export const getSprintQuery = async (sprintId: number): Promise<ISprint> => {
    const response = await apiClient.get(`/sprints/${sprintId}`)

    const item = response.data;
    return {
        id: item.Id,
        name: item.Title,
        projectId: item.ProjectId,
        status: {
            id: item.SprintStatus.Id,
            name: sprintStatuses.find(status => status.id === item.SprintStatus.Id)?.name ?? "",
            color: sprintStatuses.find(status => status.id === item.SprintStatus.Id)?.color ?? "blue",
        }
    } as ISprint
}

export const removeSprintQuery = async (sprintId: number): Promise<boolean> => {
    const response = await apiClient.delete(`/sprints/${sprintId}`)

    return response.status === 200;
};

export const editSprintQuery = async (
    sprintId: number,
    data: IEditSprintOptions
): Promise<boolean> => {
    const response = await apiClient.put(`/sprints`, {
        id: sprintId,
        title: data.name,
        goal: data.goal,
        startsOn: data.startDate ? data.startDate.utc().toISOString() : null, // Convert to UTC ISO string
        expireOn: data.endDate ? data.endDate.utc().toISOString() : null,
        sprintStatusId: data.status.id
    })

    return response.status === 200;
};

export const getSprintTasksQuery = async (
    sprintId: number,
    projectId: number,
    filters: ITaskFilterOptions,
    page: number,
    limit?: number
): Promise<ITask[]> => {
    const result = await getProjectTasksQuery(projectId, filters, page, limit)
    return result.filter((task) => task.sprint && task.sprint.id === sprintId)
};

export const getAvailableTasksForSprintQuery = async (
    sprintId: number,
    projectId: number,
    filters: ITaskFilterOptions,
    page: number,
    limit?: number
): Promise<ITask[]> => {
    return (await getProjectTasksQuery(projectId, filters, page, limit)).filter((task) => (task.sprint && task.sprint.id !== sprintId) || (!task.sprint))
};

export const addTaskToSprintQuery = async (
    task: ITask,
    sprintId?: number,
): Promise<boolean> => {
    console.log(task, "adasda")
    console.log(sprintId)
    const options: IEditTaskOptions = {
        name: task.name,
        priority: task.priority,
        progress: task.progress,
        projectId: task.project.id,
        status: task.status,
        type: task.type,
        workers: task.workers,
        description: task.description,
        endDate: task.endDate,
        startDate: task.startDate,
        storyPoints: task.storyPoints,
        sprintId: sprintId,
    }

    const res = await editTaskQuery(task.id, options);

    return res;
};

export const removeTaskFromSprintQuery = async (
    sprintId: number,
    task: ITask
): Promise<boolean> => {
    if (sprintId === task.sprint?.id) {
        const options: IEditTaskOptions = {
            name: task.name,
            priority: task.priority,
            progress: task.progress,
            projectId: task.project.id,
            status: task.status,
            type: task.type,
            workers: task.workers,
            description: task.description,
            endDate: task.endDate,
            startDate: task.startDate,
            storyPoints: task.storyPoints,
        }

        const res = await editTaskQuery(task.id, options);

        return res;
    }
    return false;
};


export const getSprintHistoryQuery = async (
    sprintId: number,
    filters: IHistoryFilterOptions,
    page: number,
    limit?: number
): Promise<IHistoryItem[]> => {
    const response = await apiClient.get(`/history/${HistoryOwnerEnum.SPRINT}/${sprintId}`)
    const items = response.data.map((item: any) => ({
        id: item.Id,
        message: item.Text,
        createdAt: dayjs(item.CreatedOn),
        owner: {
            id: item.RelatedEntityId,
            type: item.RelatedEntityType
        },
        responsibleWorker: {
            id: item.Creator.Id,
            firstName: item.Creator.Name,
            secondName: item.Creator.SecondName,
            thirdName: item.Creator.ThirdName,
            email: item.Creator.Email
        }
    }) as IHistoryItem)
    return filterHistory(items, filters, page, limit);
};

export const getAvailalbeSprintHistoryResponsibleWorkers = async (
    sprintId: number
): Promise<IWorkerFields[]> => {
    console.log(sprintId);
    const workers = await getWorkersQuery({});
    return workers;
};

export const removeSprintHistoryQuery = async (sprintId: number): Promise<boolean> => {
    const response = await apiClient.delete(`/history/${HistoryOwnerEnum.SPRINT}/${sprintId}`)
    return response.status === 200;
};

export const removeSprintHistoryItemQuery = async (
    itemId: number
): Promise<boolean> => {
    const response = await apiClient.delete(`/history/${itemId}`)
    return response.status === 200;
};



export const getSprintAnalytics = async (sprintId: number): Promise<SprintAnalyticsData> => {
    const response = await apiClient.get(`analytics/sprint/${sprintId}`);

    const data: ApiSprintAnalyticsResponse = response.data;

    return {
        tasksByStatus: Object.fromEntries(
            data.TasksByStatus.map(({ Status, Count }) => [Status.Name, Count])
        ),
        tasksByPriority: Object.fromEntries(
            data.TasksByPriority.map(({ Priority, Count }) => [Priority.Name, Count])
        ),
        tasksByTag: Object.fromEntries(
            data.TasksByTag.map(({ Tag, Count }) => [Tag.Name, Count])
        ),
        tasksByEmployee: data.TasksByEmployee.map(({ Item1, Item2 }) => ({
            email: Item1.Email,
            statuses: Object.fromEntries(
                Item2.map(({ Status, Count }) => [Status.Name, Count])
            ),
        })),
        totalTasks: data.TotalTasks,
        overdueTasks: data.OverdueTasks,
    };
};