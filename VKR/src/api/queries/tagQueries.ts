import { apiClient } from "../../config/axiosConfig";
import { TaskWorkerTypeEnum } from "../../enums/TaskWorkerTypeEnum";
import { AddTagDTO, AddTagsDTO, CreateTagDTO, FullTagDTO, ITag } from "../../interfaces/ITag";
import { ITask } from "../../interfaces/ITask";
import { ITaskWorker } from "../../interfaces/ITaskWorker";
import { sprintStatuses } from "../../sync/sprintStatuses";
import { taskPriorities } from "../../sync/taskPriorities";
import { taskStatuses } from "../../sync/taskStatuses";
import dayjs from "dayjs";

// Helper function to map API task data to ITask
const mapTask = (item: any): ITask => {
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

    const responsibleWorker = item.ResponsibleWorker;
    if (responsibleWorker) {
        const responsibleIndex = workers.findIndex((w) => w.workerData.id === responsibleWorker.Id);
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
            firstName: item.Creator.Name,
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
                status: sprintStatuses.find((status) => status.id === item.Sprint.SprintStatusId) ?? sprintStatuses[0],
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
};

// Task Tag Queries
export const getTaskTagsQuery = async (): Promise<ITag[]> => {
    const response = await apiClient.get(`tasks/all-tags`);
    return response.data.map((tag: any) => ({ id: tag.Id, name: tag.Name, color: tag.Color }));
};

export const getAvailableTaskTagsQuery = async (taskId: number): Promise<ITag[]> => {
    const response = await apiClient.get(`tasks/${taskId}/available-tags`);
    return response.data.map((tag: any) => ({ id: tag.Id, name: tag.Name, color: tag.Color }));
};

export const addExistingTag = async (taskId: number, tag: AddTagDTO): Promise<{ success: boolean }> => {
    const response = await apiClient.post(`/tasks/${taskId}/tags/add-existing`, tag);
    return { success: response.status === 200 };
};

export const addNewTaskTag = async (taskId: number, tag: CreateTagDTO): Promise<FullTagDTO> => {
    const response = await apiClient.post(`/tasks/${taskId}/tags/add-new`, tag);

    return {
        color: response.data.Color,
        id: response.data.Id,
        name: response.data.Name,
    }
};

export const deleteTag = async (taskId: number, tagId: number): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/tasks/${taskId}/tags/${tagId}`);
    return { success: response.status === 200 };
};

export const addTags = async (taskId: number, tags: AddTagsDTO): Promise<{ success: boolean }> => {
    const response = await apiClient.post(`/tasks/${taskId}/tags/add-tags`, tags);
    return { success: response.status === 200 };
};

// Project Tag Queries
export const getProjectTagsQuery = async (): Promise<ITag[]> => {
    const response = await apiClient.get(`projects/all-tags`);
    return response.data.map((tag: any) => ({ id: tag.Id, name: tag.Name, color: tag.Color }));
};

export const getAvailableProjectTagsQuery = async (projectId: number): Promise<ITag[]> => {
    const response = await apiClient.get(`projects/${projectId}/available-tags`);
    return response.data.map((tag: any) => ({ id: tag.Id, name: tag.Name, color: tag.Color }));
};

export const addExistingProjectTag = async (projectId: number, tag: AddTagDTO): Promise<{ success: boolean }> => {
    const response = await apiClient.post(`/projects/${projectId}/tags/add-existing`, tag);
    return { success: response.status === 200 };
};

export const addNewProjectTag = async (projectId: number, tag: CreateTagDTO): Promise<FullTagDTO> => {
    const response = await apiClient.post(`/projects/${projectId}/tags/add-new`, tag);
    return {
        color: response.data.Color,
        id: response.data.Id,
        name: response.data.Name,
    }
};

export const deleteProjectTag = async (projectId: number, tagId: number): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/projects/${projectId}/tags/${tagId}`);
    return { success: response.status === 200 };
};

export const addProjectTags = async (projectId: number, tags: AddTagsDTO): Promise<{ success: boolean }> => {
    const response = await apiClient.post(`/projects/${projectId}/tags/add-tags`, tags);
    return { success: response.status === 200 };
};

// Task Template Tag Queries
export const getTemplateTagsQuery = async (): Promise<ITag[]> => {
    const response = await apiClient.get(`task-templates/all-tags`);
    return response.data.map((tag: any) => ({ id: tag.Id, name: tag.Name, color: tag.Color }));
};

export const getAvailableTemplateTagsQuery = async (templateId: number): Promise<ITag[]> => {
    const response = await apiClient.get(`task-templates/${templateId}/available-tags`);
    return response.data.map((tag: any) => ({ id: tag.Id, name: tag.Name, color: tag.Color }));
};

export const addExistingTemplateTag = async (templateId: number, tag: AddTagDTO): Promise<{ success: boolean }> => {
    const response = await apiClient.post(`/task-templates/${templateId}/tags/add-existing`, tag);
    return { success: response.status === 200 };
};

export const addNewTemplateTag = async (templateId: number, tag: CreateTagDTO): Promise<FullTagDTO> => {
    const response = await apiClient.post(`/task-templates/${templateId}/tags/add-new`, tag);
    return {
        color: response.data.Color,
        id: response.data.Id,
        name: response.data.Name,
    }
};

export const deleteTemplateTag = async (templateId: number, tagId: number): Promise<{ success: boolean }> => {
    const response = await apiClient.delete(`/task-templates/${templateId}/tags/${tagId}`);
    return { success: response.status === 200 };
};

export const addTemplateTags = async (templateId: number, tags: AddTagsDTO): Promise<{ success: boolean }> => {
    const response = await apiClient.post(`/task-templates/${templateId}/tags/add-tags`, tags);
    return { success: response.status === 200 };
};

// Tag Aggregation Queries
export const getNumTasksForEachTag = async (): Promise<{ tag: ITag; numTasks: number }[]> => {
    const response = await apiClient.get(`/tasks`);
    const tasks: ITask[] = response.data.map(mapTask);

    const tagCounts = new Map<string, { tag: ITag; numTasks: number }>();
    tasks.forEach((task) => {
        task.tags.forEach((tag) => {
            const key = tag.name;
            if (tagCounts.has(key)) {
                tagCounts.get(key)!.numTasks += 1;
            } else {
                tagCounts.set(key, { tag, numTasks: 1 });
            }
        });
    });

    return Array.from(tagCounts.values()).sort((a, b) => a.tag.name.localeCompare(b.tag.name));
};

export const getTasksByTag = async (tagName: string): Promise<ITask[]> => {
    const response = await apiClient.get(`/tasks`);
    const tasks: ITask[] = response.data.map(mapTask);
    return tasks.filter((task) => task.tags.some((tag) => tag.name === tagName));
};