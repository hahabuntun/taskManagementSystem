import { TaskRelationshipTypeEnum } from "../../enums/TaskRelationshipTypeEnum";
import { IFile } from "../../interfaces/IFile";
import { IHistoryItem } from "../../interfaces/IHistoryItem";
import { ILink } from "../../interfaces/ILink";
import { ITask } from "../../interfaces/ITask";
import { ITaskComment } from "../../interfaces/ITaskComment";
import { ITaskExecutorForm, ITaskObserverForm, ITaskWorker } from "../../interfaces/ITaskWorker";
import { IWorker } from "../../interfaces/IWorker";
import { filterFiles, filterHistory, filterTasks } from "../dummyData/filters";
import { IAddFileOptions } from "../options/createOptions/IAddFileOptions";
import { IAddLinkOptions } from "../options/createOptions/IAddLinkOptions";
import { IAddTaskCommentOptions } from "../options/createOptions/IAddTaskCommentOptions";
import { IEditLinkOptions } from "../options/editOptions/IEditLinkOptions";
import { IEditTaskOptions } from "../options/editOptions/IEditTaskOptions";
import { IFileFilterOptions } from "../options/filterOptions/IFileFilterOptions";
import { IHistoryFilterOptions } from "../options/filterOptions/IHistoryFilterOptions";


import dayjs from "dayjs";
import { ITaskFilterOptions } from "../options/filterOptions/ITaskFilterOptions";
import { FileOwnerEnum } from '../../enums/ownerEntities/FileOwnerEnum';
import { HistoryOwnerEnum } from '../../enums/ownerEntities/HistoryOwnerEnum';
import { IWorkerFields } from '../../interfaces/IWorkerFields';
import { ITaskFilter } from '../../interfaces/ITaskFilter';
import { TaskWorkerTypeEnum } from '../../enums/TaskWorkerTypeEnum';
import { TaskTypeEnum } from '../../enums/TaskTypeEnum';
import { apiClient } from '../../config/axiosConfig';
import { taskStatuses } from '../../sync/taskStatuses';
import { workerStatuses } from "../../sync/workerStatuses";
import { LinkOwnerEnum } from "../../enums/ownerEntities/LinkOwnerEnum";
import { ISprint } from "../../interfaces/ISprint";
import { sprintStatuses } from "../../sync/sprintStatuses";
import { taskPriorities } from "../../sync/taskPriorities";
import { getWorkerQuery, getWorkersQuery } from "./workersQueries";


export const mapTaskDTOToITask = (item: any, currentTaskId: number): ITask => {


    // Combine Executors and Observers into workers
    const workers: ITaskWorker[] = [
        ...(item.Executors || []).map((executor: any) => ({
            workerData: {
                id: executor.Id,
                name: executor.Name,
                email: executor.Email,
                secondName: executor.SecondName,
                thirdName: "",
            },
            isResponsible: item.ResponsibleWorker && item.ResponsibleWorker.Id === executor.Id,
            taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
        })),
        ...(item.Observers || []).map((observer: any) => ({
            workerData: {
                id: observer.Id,
                name: observer.Name,
                email: observer.Email,
                secondName: observer.SecondName,
                thirdName: "",
            },
            isResponsible: false,
            taskWorkerType: TaskWorkerTypeEnum.VIEWER,
        })),
    ];

    return {
        id: item.Id,
        name: item.ShortName || "",
        description: item.Description || "",
        type: item.TaskType?.Name,
        progress: item.Progress || 0,
        project: {
            id: item.Project?.Id,
            name: item.Project?.Name,
            managerId: item.Project?.ManagerId,
        },
        createdAt: dayjs(item.CreatedOn),
        startDate: item.StartOn ? dayjs(item.StartOn) : undefined,
        endDate: item.ExpireOn ? dayjs(item.ExpireOn) : undefined,
        status: {
            id: item.TaskStatus?.Id,
            name: taskStatuses.find(status => status.id === item.TaskStatus?.Id)?.name ?? "Не определен",
            color: taskStatuses.find(status => status.id === item.TaskStatus?.Id)?.color ?? "blue",
        },
        priority: {
            id: item.TaskPriority?.Id,
            name: taskStatuses.find(status => status.id === item.TaskPriority?.Id)?.name ?? "Не определен",
            color: taskStatuses.find(status => status.id === item.TaskPriority?.Id)?.color ?? "blue",
        },
        creator: {
            id: item.Creator?.Id,
            firstName: item.Creator?.Name,
            email: item.Creator?.Email,
            secondName: item.Creator?.SecondName,
            thirdName: "",
        },
        tags: (item.TagDTOs || []).map((tag: any) => ({
            id: tag.Id,
            name: tag.Name,
            color: tag.Color,
        })),
        workers,
        sprint: undefined,
        storyPoints: item.StoryPoints,
        checklists: item.Checklists || [],
        relationships: (item.RelatedTasks || []).map((rel: any) => ({
            taskId: currentTaskId,
            relatedTaskId: rel.Task.Id,
            relationType: rel.RelationshipType,
            lag: undefined, // Backend doesn't provide lag
        })),
    };
};

export const getTaskQuery = async (taskId: number): Promise<ITask> => {
    const response = await apiClient.get(`/tasks/${taskId}`);



    const item = response.data;


    // Combine Executors and Observers into workers
    const workers: ITaskWorker[] = [
        ...item.Executors.map((executor: any) => ({
            workerData: {
                id: executor.Id,
                name: executor.Name,
                email: executor.Email,
                secondName: executor.SecondName,
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
                secondName: observer.SecondName,
                thirdName: "",
            },
            isResponsible: false,
            taskWorkerType: TaskWorkerTypeEnum.VIEWER,
        })),
    ];
    console.log('aaa', item)

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
            name: taskStatuses.find(status => status.id === item.TaskStatus.Id)?.name ?? "Не определен",
            color: taskStatuses.find(status => status.id === item.TaskStatus.Id)?.color ?? "blue",
        },
        priority: {
            id: item.TaskPriority.Id,
            name: taskPriorities.find(priority => priority.id === item.TaskPriority.Id)?.name ?? "Не определен",
            color: taskPriorities.find(priority => priority.id === item.TaskPriority.Id)?.color ?? "blue",
        },
        creator: {
            id: item.Creator.Id,
            firstName: item.Creator.Name,
            email: item.Creator.Email,
            secondName: item.Creator.SecondName,
            thirdName: "",
        },
        tags: item.TagDTOs.map((tag: any) => ({
            id: tag.Id,
            name: tag.Name,
            color: tag.Color,
        })),
        workers,
        sprint: item.Sprint ? {
            id: item.Sprint.Id,
            name: item.Sprint.Title,
            projectId: item.Sprint.ProjectId,
            status: sprintStatuses.find(status => status.id === item.Sprint.SprintStatusId)
        } as ISprint : undefined,
        storyPoints: item.StoryPoints, // Not in response, assume undefined
        checklists: item.Checklists || [], // Not in response, assume empty
        relationships: item.RelatedTasks.map((rel: any) => ({
            taskId: item.Id,
            relatedTaskId: rel.Task.Id,
            relationType: rel.RelationshipType,
            lag: undefined, // Not provided in response
        })),
    };
};


export const removeTaskQuery = async (t: ITask): Promise<boolean> => {
    console.log("We here?")
    const response = await apiClient.delete(`/tasks/${t.id}`);
    console.log(response, "r")

    return response.status === 200;
};



export const editTaskQuery = async (
    taskId: number,
    data: IEditTaskOptions
): Promise<boolean> => {
    console.log(data, 'asdaasdasdsassssssssssssssssssssssssssssssssssds')
    const response = await apiClient.put(`/tasks/${taskId}`, {
        name: data.name,
        progress: data.progress,
        description: data.description,
        startDate: data.startDate ? data.startDate.utc().toISOString() : null,
        endDate: data.endDate ? data.endDate.utc().toISOString() : null,
        taskTypeId: data.type === TaskTypeEnum.TASK ? 1 : 2,
        taskStatusId: data.status.id,
        taskPriorityId: data.priority.id,
        storyPoints: data.storyPoints,
        sprintId: data.sprintId,
        executorIds: data.workers
            .filter((worker) => worker.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR)
            .map((worker) => worker.workerData.id),
        responsibleExecutorId:
            data.workers.find((worker) => worker.isResponsible === true)?.workerData.id,
        observerIds: data.workers
            .filter((worker) => worker.taskWorkerType === TaskWorkerTypeEnum.VIEWER)
            .map((worker) => worker.workerData.id),
    });

    return response.status === 200;
};


export const addTaskExecutor = async (
    taskId: number,
    executor: ITaskExecutorForm
): Promise<{ success: boolean }> => {
    const response = await apiClient.post(`/tasks/${taskId}/executors`, {
        workerId: executor.workerId,
        isResponsible: executor.isResponsible
    });
    return { success: response.status === 201 };
};

export const removeTaskExecutor = async (
    taskId: number,
    workerId: number
): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/tasks/${taskId}/executors/${workerId}`);
    return { success: response.status === 204 };
};

export const updateTaskExecutorResponsible = async (
    taskId: number,
    workerId: number | null
): Promise<{ success: boolean }> => {
    const response = await apiClient.patch(`/tasks/${taskId}/executors/responsible`, {
        workerId,
    });
    return { success: response.status === 204 };
};

export const addTaskObserver = async (
    taskId: number,
    observer: ITaskObserverForm
): Promise<{ success: boolean }> => {
    const response = await apiClient.post(`/tasks/${taskId}/observers`, {
        workerId: observer.workerId
    });
    return { success: response.status === 201 };
};

export const removeTaskObserver = async (
    taskId: number,
    workerId: number
): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/tasks/${taskId}/observers/${workerId}`);
    return { success: response.status === 204 };
};





export const getAvailableTaskTakersForExistingTaskQuery = async (
    taskId: number,
    workerId: number
): Promise<IWorker[]> => {
    const task = await getTaskQuery(taskId);
    const allTaskTakers = await getAllTaskTakersQuery(task.project.id, workerId);
    const worker = await getWorkerQuery(workerId)
    return allTaskTakers.filter((taker) => !task.workers.map(worker => worker.workerData.id).includes(taker.id)).concat([worker]);
}


export const getAllTaskTakersQuery = async (
    projectId: number,
    workerId: number
): Promise<IWorker[]> => {

    const response = await apiClient.get(`project-member-management/${projectId}/members/${workerId}`);

    const data = response.data;

    // Map TaskTakers to IWorker[]
    const taskTakers: IWorker[] = data.TaskTakers.map((taker: any): IWorker => ({
        id: taker.Id,
        firstName: taker.Name,
        secondName: taker.SecondName,
        thirdName: taker.ThirdName,
        email: taker.Email,
        createdAt: dayjs(taker.CreatedOn),
        isAdmin: taker.CanManageWorkers,
        isManager: taker.CanManageProjects,
        status: {
            id: taker.WorkerStatus.Id,
            name: workerStatuses.find(status => status.id === taker.WorkerStatus.Id)?.name ?? "Не указан",
            color: workerStatuses.find(status => status.id === taker.WorkerStatus.Id)?.color ?? "blue"
        },
        workerPosition: {
            id: taker.WorkerPosition.Id,
            title: taker.WorkerPosition.Title,
            canAssignTasksTo: taker.WorkerPosition.TaskGivers.map((giver: any) => giver.Id) || [],
            canTakeTasksFrom: taker.WorkerPosition.TaskTakers.map((taker: any) => taker.Id) || []
        }
    }) as IWorker);

    const worker = await getWorkerQuery(workerId);

    return [...taskTakers, worker];
};



export const getRelatedTasksQuery = async (
    taskId: number,
    relationType: TaskRelationshipTypeEnum,
    filters: ITaskFilterOptions
): Promise<ITask[]> => {
    try {
        const response = await apiClient.get(`/tasks/${taskId}/related`);
        const relatedTasks = response.data as any[];


        // Filter tasks by relationType
        const tasks = relatedTasks
            .filter(task => task.RelationshipType === relationType)
            .map(task => mapTaskDTOToITask(task.Task, task.Task.Id));
        console.log(relatedTasks, "rel")
        return filterTasks(tasks, filters, 1)
    } catch (error) {
        throw new Error("Unable to fetch related tasks");
    }
};

export const addRelatedTaskQuery = async (
    taskId: number,
    relatedTaskId: number,
    relationType: TaskRelationshipTypeEnum,
): Promise<boolean> => {
    // Map frontend relationType to backend relationshipTypeId
    const relationTypeIdMap: { [key in TaskRelationshipTypeEnum]: number } = {
        [TaskRelationshipTypeEnum.FINISH_START]: 1, // FinishToStart
        [TaskRelationshipTypeEnum.FINISH_FINISH]: 2, // FinishToFinish
        [TaskRelationshipTypeEnum.START_START]: 3, // StartToStart
        [TaskRelationshipTypeEnum.START_FINISH]: 4, // StartToFinish
        [TaskRelationshipTypeEnum.SUBTASK]: 5, // ParentChild
    };

    const relationshipTypeId = relationTypeIdMap[relationType];
    if (!relationshipTypeId) {
        throw new Error(`Invalid relation type: ${relationType}`);
    }

    const response = await apiClient.post(`/tasks/${taskId}/relationships`, {
        relatedTaskId,
        relationshipTypeId,
    });

    return response.status === 200;

};

export const removeRelationBetweenTasksQuery = async (
    taskId: number,
    relatedTaskId: number
): Promise<boolean> => {
    const response = await apiClient.delete(`/tasks/${taskId}/relationships/${relatedTaskId}`);
    return response.status === 200;
};


export const getAvailableRelatedTasks = async (
    taskId: number,
): Promise<ITask[]> => {
    const response = await apiClient.get(`/tasks/${taskId}/available-related`);
    const availableTasks = response.data as any[];


    return availableTasks.map(task => mapTaskDTOToITask(task, task.Id));
};



export const getTaskCommentsQuery = async (
    taskId: number,
): Promise<ITaskComment[]> => {
    const response = await apiClient.get(`/task-messages?taskId=${taskId}`)


    return response.data.map((message: any) => ({
        id: message.Id,
        taskId: message.RelatedTaskId,
        createdAt: dayjs(message.CreatedOn),
        creator: {
            id: message.SenderId,
            email: message.SenderEmail,
            firstName: message.SenderName,
            secondName: message.SenderSecondName,
            thirdName: message.SenderThirdName
        },
        text: message.MessageText
    }) as ITaskComment)
}

export const addTaskCommentQuery = async (
    taskId: number,
    data: IAddTaskCommentOptions,
    creator: IWorker,
): Promise<boolean> => {

    const response = await apiClient.post(`/task-messages`, {
        messageText: data.text,
        senderId: creator.id,
        relatedTaskId: taskId
    })

    return response.status === 200
};




// remove task comment
export const removeTaskCommentQuery = async (
    commentId: number,
): Promise<boolean> => {
    const response = await apiClient.delete(`/task-messages/${commentId}`)
    return response.status === 204
};




// get all task files
export const getTaskFilesQuery = async (
    taskId: number,
    filters: IFileFilterOptions,
    page: number,
    limit?: number
): Promise<IFile[]> => {
    const response = await apiClient.get(`/${FileOwnerEnum.TASK}/${taskId}/files`);
    const items = response.data.map((item: any) => ({
        id: item.Id,
        createdAt: dayjs(item.CreatedAt),
        creator: {
            id: item.Creator.Id,
            email: item.Creator.Email,
            firstName: item.Creator.Name,
            secondName: item.Creator.SecondName,
            thirdName: item.Creator.ThirdName
        },
        description: item.Description,
        name: item.Name,
        size: item.FileSize,
        owner: {
            id: taskId,
            name: "task",
            type: FileOwnerEnum.TASK
        }
    }) as IFile)

    return filterFiles(items, filters, page, limit);
};

export const getAvailalbeTaskFilesResponsibleWorkers = async (
): Promise<IWorkerFields[]> => {
    return await getWorkersQuery({})
};

// add task file
export const addTaskFileQuery = async (
    taskId: number,
    data: IAddFileOptions,
): Promise<boolean> => {
    const formData = new FormData();
    formData.append('file', data.file); // Append the file
    formData.append('title', data.name); // Append the title
    if (data.description) formData.append('description', data.description);

    const response = await apiClient.post(`/${FileOwnerEnum.TASK}/${taskId}/files`, formData, {
        headers: {
            "Content-Type": "multipart/form-data", // Явно указываем для ясности
        },
    });


    return response.status === 201
};

// delete task file
export const removeTaskFileQuery = async (
    taskId: number,
    fileId: number,
): Promise<boolean> => {
    const response = await apiClient.delete(
        `/${FileOwnerEnum.TASK}/${taskId}/files/${fileId}`,
    );

    return response.status === 204;
};


// download task file
export const downloadTaskFileQuery = async (taskId: number, fileId: number) => {
    const response = await apiClient.get(`/${FileOwnerEnum.TASK}/${taskId}/files/${fileId}`, {
        responseType: 'blob', // Expect binary data
    });

    if (response.status !== 200) {
        throw new Error('Failed to download file');
    }

    return response.data as Blob;
};


//get worker history
export const getTaskHistoryQuery = async (
    taskId: number,
    filters: IHistoryFilterOptions,
    page: number,
    limit?: number
): Promise<IHistoryItem[]> => {
    const response = await apiClient.get(`/history/${HistoryOwnerEnum.TASK}/${taskId}`)
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

export const getAvailalbeTaskHistoryResponsibleWorkers = async (
    taskId: number
): Promise<IWorkerFields[]> => {
    console.log(taskId);
    const workers = await getWorkersQuery({});
    return workers;
};

//remove worker history
export const removeTaskHistoryQuery = async (
    taskId: number
): Promise<boolean> => {
    const response = await apiClient.delete(`/history/${HistoryOwnerEnum.TASK}/${taskId}`)
    return response.status === 200;
};

//remove one worker history item
export const removeTaskHistoryItemQuery = async (
    itemId: number
): Promise<boolean> => {
    const response = await apiClient.delete(`/history/${itemId}`)
    return response.status === 200;
};


// get task links
export const getTaskLinksQuery = async (
    taskId: number,
): Promise<ILink[]> => {
    const response = await apiClient.get(`/tasks/${taskId}/links`);
    const res = response.data.map((link: any) => ({
        id: link.Id,
        name: link.Description,
        link: link.Link,
        owner: {
            id: taskId,
            name: "",
            type: LinkOwnerEnum.TASK
        }
    }) as ILink)

    return res;

}

// add task link
export const addTaskLinkQuery = async (
    taskId: number,
    data: IAddLinkOptions
): Promise<boolean> => {
    const response = await apiClient.post(`/tasks/${taskId}/links`, {
        link: data.link,
        description: data.name
    });

    return response.status === 201
};

// edit task link
export const editTaskLinkQuery = async (
    taskId: number,
    linkId: number,
    data: IEditLinkOptions
): Promise<boolean> => {
    const response = await apiClient.put(`/tasks/${taskId}/links/${linkId}`, {
        link: data.link,
        description: data.name
    });

    return response.status === 204
};

// remove task link
export const removeTaskLinkQuery = async (
    taskId: number,
    linkId: number,
): Promise<boolean> => {
    const response = await apiClient.delete(`/tasks/${taskId}/links/${linkId}`);

    return response.status === 204
};




export const getTaskFilters = async (): Promise<ITaskFilter[]> => {
    const response = await apiClient.get("/task-filters");

    console.log(response, 'fi')
    return response.data.map((item: any) => ({
        name: item.Name,
        options: {
            createdFrom: item.Options.CreatedFrom ? dayjs(item.Options.CreatedFrom) : undefined,
            createdTill: item.Options.CreatedTill ? dayjs(item.Options.CreatedTill) : undefined,
            creator: undefined,
            description: item.Options.Description,
            endDateFrom: item.Options.EndDateFrom ? dayjs(item.Options.EndDateFrom) : undefined,
            endDateTill: item.Options.EndDateTill ? dayjs(item.Options.EndDateTill) : undefined,
            name: item.Options.Name,
            priority: undefined,
            startedFrom: item.Options.StartedFrom ? dayjs(item.Options.StartedFrom) : undefined,
            startedTill: item.Options.StartedTill ? dayjs(item.Options.StartedTill) : undefined,
            status: undefined,
            tags: undefined,
            type: undefined,
        }
    }) as ITaskFilter);
};

export const addTaskFilterTo = async (filterName: string, options: ITaskFilterOptions): Promise<boolean> => {
    const response = await apiClient.post("/task-filters", {
        name: filterName, options: {
            name: options.name,
            description: options.description,
            type: options.type,
            status: options.status,
            priority: options.priority,
            creator: undefined,
            tags: [],
            createdFrom: options.createdFrom ? options.createdFrom.utc().toISOString() : null,
            createdTill: options.createdTill ? options.createdTill.utc().toISOString() : null,
            startedFrom: options.startedFrom ? options.startedFrom.utc().toISOString() : null,
            startedTill: options.startedTill ? options.startedTill.utc().toISOString() : null,
            endDateFrom: options.endDateFrom ? options.endDateFrom.utc().toISOString() : null,
            endDateTill: options.endDateTill ? options.endDateTill.utc().toISOString() : null,
        }
    });
    return response.status === 200;

};

export const editTaskFilter = async (filterName: string, options: ITaskFilterOptions): Promise<boolean> => {
    const response = await apiClient.put(`/task-filters/${filterName}`, { options });
    return response.status === 200;

};

export const removeTaskFilter = async (filterName: string): Promise<boolean> => {
    const response = await apiClient.delete(`/task-filters/${filterName}`);
    return response.status === 200;

};




export const updateTaskResponsibleWorker = async (
    taskId: number,
    workerId: number | null
): Promise<boolean> => {
    const response = await apiClient.patch(`/tasks/${taskId}/executors/responsible`, {
        workerId,
    });
    return response.status === 204;
};


