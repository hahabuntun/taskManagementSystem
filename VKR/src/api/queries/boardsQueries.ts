import { ICustomBoardColumn } from "../../interfaces/IBoardCustomColumn";
import { IBoardTaskWithDetails } from "../../interfaces/IBoardTask";
import { ITask } from "../../interfaces/ITask";
import { filterHistory, filterTasks } from "../dummyData/filters";
import { IAddBoardColumnOptions } from "../options/createOptions/IAddBoardColumnOptions";
import { IAddTaskToBoardOptions } from "../options/createOptions/IAddTasksToBoardOptions";
import { IEditBoardOptions } from "../options/editOptions/IEditBoardOptions";
import { IFileFilterOptions } from "../options/filterOptions/IFileFilterOptions";
import { ITaskFilterOptions } from "../options/filterOptions/ITaskFilterOptions";
import { IHistoryItem } from '../../interfaces/IHistoryItem';
import { HistoryOwnerEnum } from '../../enums/ownerEntities/HistoryOwnerEnum';
import { IWorkerFields } from '../../interfaces/IWorkerFields';
import { IHistoryFilterOptions } from '../options/filterOptions/IHistoryFilterOptions';
import { apiClient } from "../../config/axiosConfig";
import { BoardBasisEnum } from "../../enums/BoardBasisEnum";
import { IBoard } from "../../interfaces/IBoard";
import dayjs from "dayjs";
import { getWorkerQuery, getWorkersQuery } from "./workersQueries";
import { getProjectQuery } from "./projectsQueries";
import { BoardOwnerEnum } from "../../enums/ownerEntities/BoardOwnerEnum";
import { taskStatuses } from "../../sync/taskStatuses";
import { TaskWorkerTypeEnum } from "../../enums/TaskWorkerTypeEnum";
import { taskPriorities } from "../../sync/taskPriorities";
import { sprintStatuses } from "../../sync/sprintStatuses";
import { ITaskWorker } from "../../interfaces/ITaskWorker";

export const mapBoardBasisToBackend = (basis: BoardBasisEnum): number => {
    switch (basis) {
        case BoardBasisEnum.STATUS_COLUMNS:
            return 0; // Status
        case BoardBasisEnum.PRIORITY_COLUMNS:
            return 1; // Priority
        case BoardBasisEnum.DATE:
            return 2; // Deadline
        case BoardBasisEnum.ASIGNEE:
            return 3; // Assignee
        case BoardBasisEnum.CUSTOM_COLUMNS:
            return 4; // CustomColumns
        default:
            throw new Error(`Unsupported BoardBasisEnum value: ${basis}`);
    }
};


export const mapBackendToBasis = (basic: number): BoardBasisEnum => {
    switch (basic) {
        case 0:
            return BoardBasisEnum.STATUS_COLUMNS; // Status
        case 1:
            return BoardBasisEnum.PRIORITY_COLUMNS; // Priority
        case 2:
            return BoardBasisEnum.DATE; // Deadline
        case 3:
            return BoardBasisEnum.ASIGNEE; // Assignee
        case 4:
            return BoardBasisEnum.CUSTOM_COLUMNS; // CustomColumns
        default:
            throw new Error();
    }
};
// Get a specific board by ID
export const getBoardQuery = async (
    boardId: number
): Promise<IBoard> => {
    const response = await apiClient.get(`/boards/${boardId}`);
    const projectId = response.data.ProjectId
    let own = !!projectId ? (await getProjectQuery(projectId)).name : (await getWorkerQuery(response.data.OwnerId)).email
    const item = response.data;
    return {
        id: item.Id,
        name: item.Name,
        boardBasis: mapBackendToBasis(item.Basis),
        createdAt: dayjs(item.CreatedOn),
        owner: {
            id: item.OwnerId ? item.OwnerId : item.ProjectId,
            name: own,
            type: item.OwnerId ? BoardOwnerEnum.WORKER : BoardOwnerEnum.PROJECT,
        }
    } as IBoard
};

// Edit a board
export const editBoardQuery = async (
    boardId: number,
    data: IEditBoardOptions
): Promise<boolean> => {
    const response = await apiClient.put(`/boards/${boardId}`, {
        id: boardId,
        name: data.name,
    });
    return response.status === 200;
};

// Remove a board
export const removeBoardQuery = async (
    boardId: number,
): Promise<boolean> => {
    const response = await apiClient.delete(`/boards/${boardId}`);
    return response.status === 204;
};

// Get board columns
export const getBoardColumnsQuery = async (
    boardId: number
): Promise<ICustomBoardColumn[]> => {
    const response = await apiClient.get(`/boards/${boardId}/columns`);
    return response.data.map((item: any) => ({
        boardId: item.BoardId,
        name: item.Name,
        order: item.Order
    }) as ICustomBoardColumn);
};


// Add a column to a board
export const addColumnToBoardQuery = async (
    boardId: number,
    data: IAddBoardColumnOptions
): Promise<boolean> => {
    const response = await apiClient.post(`/boards/${boardId}/columns`, {
        name: data.name
    });
    return response.status === 200;
};


// Change task column
export const changeTaskColumnQuery = async (
    boardId: number,
    taskId: number,
    columnName: string | undefined
): Promise<boolean> => {
    const response = await apiClient.put(`/boards/${boardId}/tasks/${taskId}/column`, {
        columnName,
    });

    return response.status === 200

};


// Remove a column from a board
export const removeColumnFromBoardQuery = async (
    boardId: number,
    columnName: string
): Promise<boolean> => {
    const response = await apiClient.delete(`/boards/${boardId}/columns/${columnName}`);
    return response.status === 204;
};

export const getBoardTasksQuery = async (
    boardId: number,
    filters: IFileFilterOptions
): Promise<ITask[]> => {
    const response = await apiClient.get(`/boards/${boardId}/tasks`);
    const tasks: ITask[] = response.data.map((item: any) => {
        // Combine Executors and Observers into workers
        const workers: ITaskWorker[] = [
            ...item.Executors.map((executor: any) => ({
                workerData: {
                    id: executor.Id,
                    name: executor.Name,
                    email: executor.Email,
                    secondName: executor.SecondName || "",
                    thirdName: "",
                },
                isResponsible: item.ResponsibleWorker && item.ResponsibleWorker.Id === executor.Id,
                taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
            })),
            ...item.Observers.map((observer: any) => ({
                workerData: {
                    id: observer.Id,
                    name: observer.Name,
                    email: observer.Email,
                    secondName: observer.SecondName || "",
                    thirdName: "",
                },
                isResponsible: false,
                taskWorkerType: TaskWorkerTypeEnum.VIEWER,
            })),
        ];

        // Set ResponsibleWorker if it matches an executor or add if not found
        const responsibleWorker = item.ResponsibleWorker;
        if (responsibleWorker) {
            const responsibleIndex = workers.findIndex(
                (w) => w.workerData.id === responsibleWorker.Id
            );
            if (responsibleIndex !== -1) {
                workers[responsibleIndex].isResponsible = true;
            } else {
                workers.push({
                    workerData: {
                        id: responsibleWorker.Id,
                        firstName: responsibleWorker.Name,
                        email: responsibleWorker.Email,
                        secondName: responsibleWorker.SecondName || "",
                        thirdName: "",
                    },
                    isResponsible: true,
                    taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
                });
            }
        }

        return {
            id: item.Id,
            name: item.ShortName,
            description: item.Description || "",
            type: item.TaskType.Name,
            progress: item.Progress,
            project: {
                id: item.Project.Id,
                name: item.Project.Name,
                managerId: item.Project.ManagerId,
            },
            createdAt: dayjs(item.CreatedOn),
            startDate: item.StartOn ? dayjs(item.StartOn) : undefined,
            endDate: item.ExpireOn ? dayjs(item.ExpireOn) : undefined,
            status: {
                id: item.TaskStatus.Id,
                name: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.name ?? "Не определен",
                color: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.color ?? "blue",
            },
            priority: {
                id: item.TaskPriority.Id,
                name: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.name ?? "Не определен",
                color: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.color ?? "blue",
            },
            creator: {
                id: item.Creator.Id,
                name: item.Creator.Name,
                email: item.Creator.Email,
                secondName: item.Creator.SecondName || "",
                thirdName: "",
            },
            tags: item.TagDTOs.map((tag: any) => ({
                id: tag.Id,
                name: tag.Name,
                color: tag.Color,
            })),
            workers,
            sprint: item.Sprint
                ? {
                    id: item.Sprint.Id,
                    name: item.Sprint.Title,
                    projectId: item.Sprint.ProjectId,
                    status: sprintStatuses.find((status) => status.id === item.Sprint.SprintStatusId),
                }
                : undefined,
            storyPoints: item.StoryPoints || undefined,
            checklists: item.Checklists || [],
            relationships: item.RelatedTasks.map((rel: any) => ({
                taskId: item.Id,
                relatedTaskId: rel.Task.Id,
                relationType: rel.RelationshipType,
                lag: undefined,
            })),
        };
    });

    return filterTasks(tasks, filters, 1); // Assuming filterTasks handles pagination if needed
};

export const getCustomBoardTasks = async (
    boardId: number,
    filters: IFileFilterOptions = {}
): Promise<IBoardTaskWithDetails[]> => {
    const response = await apiClient.get(`/boards/${boardId}/custom-tasks`);

    const boardTasks: IBoardTaskWithDetails[] = response.data.map((item: any) => {
        const taskData = item.Task;

        // Combine Executors and Observers into workers
        const workers: ITaskWorker[] = [
            ...taskData.Executors.map((executor: any) => ({
                workerData: {
                    id: executor.Id,
                    name: executor.Name,
                    email: executor.Email,
                    secondName: executor.SecondName || "",
                    thirdName: executor.ThirdName || "",
                },
                isResponsible: taskData.ResponsibleWorker && taskData.ResponsibleWorker.Id === executor.Id,
                taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
            })),
            ...taskData.Observers.map((observer: any) => ({
                workerData: {
                    id: observer.Id,
                    name: observer.Name,
                    email: observer.Email,
                    secondName: observer.SecondName || "",
                    thirdName: observer.ThirdName || "",
                },
                isResponsible: false,
                taskWorkerType: TaskWorkerTypeEnum.VIEWER,
            })),
        ];

        // Set ResponsibleWorker if it matches an executor or add if not found
        const responsibleWorker = taskData.ResponsibleWorker;
        if (responsibleWorker) {
            const responsibleIndex = workers.findIndex(
                (w) => w.workerData.id === responsibleWorker.Id
            );
            if (responsibleIndex !== -1) {
                workers[responsibleIndex].isResponsible = true;
            } else {
                workers.push({
                    workerData: {
                        id: responsibleWorker.Id,
                        firstName: responsibleWorker.Name,
                        email: responsibleWorker.Email,
                        secondName: responsibleWorker.SecondName || "",
                        thirdName: responsibleWorker.ThirdName || "",
                    },
                    isResponsible: true,
                    taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
                });
            }
        }

        // Parse the Task into ITask
        const task: ITask = {
            id: taskData.Id,
            name: taskData.ShortName,
            description: taskData.Description || "",
            type: taskData.TaskType.Name,
            progress: taskData.Progress,
            project: {
                id: taskData.Project.Id,
                name: taskData.Project.Name,
                managerId: taskData.Project.ManagerId,
            },
            createdAt: dayjs(taskData.CreatedOn),
            startDate: taskData.StartOn ? dayjs(taskData.StartOn) : undefined,
            endDate: taskData.ExpireOn ? dayjs(taskData.ExpireOn) : undefined,
            status: {
                id: taskData.TaskStatus.Id,
                name: taskData.TaskStatus.Name || (taskStatuses.find((status) => status.id === taskData.TaskStatus.Id)?.name ?? "Не определен"),
                color: taskData.TaskStatus.Color || (taskStatuses.find((status) => status.id === taskData.TaskStatus.Id)?.color ?? "blue"),
            },
            priority: {
                id: taskData.TaskPriority.Id,
                name: taskData.TaskPriority.Name || (taskPriorities.find((priority) => priority.id === taskData.TaskPriority.Id)?.name ?? "Не определен"),
                color: taskData.TaskPriority.Color || (taskPriorities.find((priority) => priority.id === taskData.TaskPriority.Id)?.color ?? "blue"),
            },
            creator: {
                id: taskData.Creator.Id,
                firstName: taskData.Creator.Name,
                email: taskData.Creator.Email,
                secondName: taskData.Creator.SecondName || "",
                thirdName: taskData.Creator.ThirdName || "",
            },
            tags: taskData.TagDTOs.map((tag: any) => ({
                id: tag.Id,
                name: tag.Name,
                color: tag.Color,
            })),
            workers,
            sprint: taskData.Sprint
                ? {
                    id: taskData.Sprint.Id,
                    name: taskData.Sprint.Title,
                    projectId: taskData.Sprint.ProjectId,
                    status: sprintStatuses.find((status) => status.id === taskData.Sprint.SprintStatusId) || {
                        id: taskData.Sprint.SprintStatusId,
                        name: taskData.Sprint.SprintStatus?.Name || "Не определен",
                        color: taskData.Sprint.SprintStatus?.Color || "blue",
                    },
                }
                : undefined,
            storyPoints: taskData.StoryPoints || undefined,
            checklists: taskData.Checklists || [],
            relationships: taskData.RelatedTasks.map((rel: any) => ({
                taskId: taskData.Id,
                relatedTaskId: rel.Task.Id,
                relationType: rel.RelationshipType,
                lag: undefined, // Not provided in response
            })),
        };

        return {
            boardId: item.BoardId,
            taskId: item.TaskId,
            customBoardColumnName: item.CustomColumnName,
            task,
        };
    });
    const filteredTasksIds = filterTasks(boardTasks.map(task => task.task), filters, 1).map(task => task.id)
    return boardTasks.filter(bt => filteredTasksIds.includes(bt.task.id)); // Filters can be applied here if needed, but IFileFilterOptions seems unrelated to IBoardTaskWithDetails
};

export const getAvailableTasksForBoardQuery = async (
    boardId: number,
    workerId: number,
    filters: ITaskFilterOptions,
): Promise<ITask[]> => {
    const response = await apiClient.get(`/boards/${boardId}/available-tasks/${workerId}`);

    const tasks: ITask[] = response.data.map((item: any) => {
        // Combine Executors and Observers into workers
        const workers: ITaskWorker[] = [
            ...item.Executors.map((executor: any) => ({
                workerData: {
                    id: executor.Id,
                    name: executor.Name,
                    email: executor.Email,
                    secondName: executor.SecondName || "",
                    thirdName: "",
                },
                isResponsible: item.ResponsibleWorker && item.ResponsibleWorker.Id === executor.Id,
                taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
            })),
            ...item.Observers.map((observer: any) => ({
                workerData: {
                    id: observer.Id,
                    name: observer.Name,
                    email: observer.Email,
                    secondName: observer.SecondName || "",
                    thirdName: "",
                },
                isResponsible: false,
                taskWorkerType: TaskWorkerTypeEnum.VIEWER,
            })),
        ];

        // Set ResponsibleWorker if it matches an executor or add if not found
        const responsibleWorker = item.ResponsibleWorker;
        if (responsibleWorker) {
            const responsibleIndex = workers.findIndex(
                (w) => w.workerData.id === responsibleWorker.Id
            );
            if (responsibleIndex !== -1) {
                workers[responsibleIndex].isResponsible = true;
            } else {
                workers.push({
                    workerData: {
                        id: responsibleWorker.Id,
                        firstName: responsibleWorker.Name,
                        email: responsibleWorker.Email,
                        secondName: responsibleWorker.SecondName || "",
                        thirdName: "",
                    },
                    isResponsible: true,
                    taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
                });
            }
        }

        return {
            id: item.Id,
            name: item.ShortName,
            description: item.Description || "",
            type: item.TaskType.Name,
            progress: item.Progress,
            project: {
                id: item.Project.Id,
                name: item.Project.Name,
                managerId: item.Project.ManagerId,
            },
            createdAt: dayjs(item.CreatedOn),
            startDate: item.StartOn ? dayjs(item.StartOn) : undefined,
            endDate: item.ExpireOn ? dayjs(item.ExpireOn) : undefined,
            status: {
                id: item.TaskStatus.Id,
                name: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.name ?? "Не определен",
                color: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.color ?? "blue",
            },
            priority: {
                id: item.TaskPriority.Id,
                name: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.name ?? "Не определен",
                color: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.color ?? "blue",
            },
            creator: {
                id: item.Creator.Id,
                name: item.Creator.Name,
                email: item.Creator.Email,
                secondName: item.Creator.SecondName || "",
                thirdName: "",
            },
            tags: item.TagDTOs.map((tag: any) => ({
                id: tag.Id,
                name: tag.Name,
                color: tag.Color,
            })),
            workers,
            sprint: item.Sprint
                ? {
                    id: item.Sprint.Id,
                    name: item.Sprint.Title,
                    projectId: item.Sprint.ProjectId,
                    status: sprintStatuses.find((status) => status.id === item.Sprint.SprintStatusId),
                }
                : undefined,
            storyPoints: item.StoryPoints || undefined,
            checklists: item.Checklists || [],
            relationships: item.RelatedTasks.map((rel: any) => ({
                taskId: item.Id,
                relatedTaskId: rel.Task.Id,
                relationType: rel.RelationshipType,
                lag: undefined,
            })),
        };
    });

    return filterTasks(tasks, filters, 1); // Assuming filterTasks handles pagination
};

// Add a task to a board
export const addTaskToBoardQuery = async (
    boardId: number,
    data: IAddTaskToBoardOptions
): Promise<boolean> => {
    const response = await apiClient.post(`/boards/${boardId}/tasks`, {
        taskId: data.taskId,
        customColumnName: data.columnName
    });
    return response.status === 200;

};

// Remove a task from a board
export const removeTaskFromBoardQuery = async (
    boardId: number,
    taskId: number
): Promise<boolean> => {
    const response = await apiClient.delete(`/boards/${boardId}/tasks/${taskId}`);
    return response.status === 204;

};

// Clear a board
export const clearBoardQuery = async (
    boardId: number
): Promise<boolean> => {
    const response = await apiClient.delete(`/boards/${boardId}/clear`);
    return response.status === 200;

};


//get board history
export const getBoardHistoryQuery = async (
    boardId: number,
    filters: IHistoryFilterOptions,
    page: number,
    limit?: number
): Promise<IHistoryItem[]> => {
    const response = await apiClient.get(`/history/${HistoryOwnerEnum.BOARD}/${boardId}`)
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

export const getAvailalbeBoardHistoryResponsibleWorkers = async (
    boardId: number
): Promise<IWorkerFields[]> => {
    console.log(boardId);
    const workers = await getWorkersQuery({});
    return workers;
};

//remove board history
export const removeBoardHistoryQuery = async (
    boardId: number
): Promise<boolean> => {
    const response = await apiClient.delete(`/history/${HistoryOwnerEnum.BOARD}/${boardId}`)
    return response.status === 200;
};

//remove one board history item
export const removeBoardHistoryItemQuery = async (
    itemId: number
): Promise<boolean> => {
    const response = await apiClient.delete(`/history/${itemId}`)
    return response.status === 200;
};
