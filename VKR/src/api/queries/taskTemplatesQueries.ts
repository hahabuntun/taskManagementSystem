import { taskStatuses } from './../../sync/taskStatuses';
import { ITaskTemplateOptions } from '../options/ITaskTemplateOptions';
import { ITaskTemplate } from "../../interfaces/ITaskTemplate";
import { ITaskTemplateFilterOptions } from "../options/filterOptions/ITaskTemplateFilterOptions";
import { ILink } from '../../interfaces/ILink';
import { IAddLinkOptions } from '../options/createOptions/IAddLinkOptions';
import { IEditLinkOptions } from '../options/editOptions/IEditLinkOptions';
import { apiClient } from '../../config/axiosConfig';
import { taskPriorities } from '../../sync/taskPriorities';

import dayjs from "dayjs";
import { TaskTypeEnum } from '../../enums/TaskTypeEnum';
import { LinkOwnerEnum } from '../../enums/ownerEntities/LinkOwnerEnum';
import { filterTaskTemplates } from '../dummyData/filters';


const mapTaskTypeToEnum = (typeName: string | null | undefined): TaskTypeEnum | undefined => {
    if (!typeName) return undefined;
    switch (typeName) {
        case 'Задача':
            return TaskTypeEnum.TASK;
        case 'Веха':
            return TaskTypeEnum.MILESTONE;
        case 'Сводная задача':
            return TaskTypeEnum.SUMMARY_TASK;
        default:
            return undefined;
    }
};

const mapTaskTypeToId = (type?: TaskTypeEnum): number | undefined => {
    if (!type) return undefined;
    switch (type) {
        case TaskTypeEnum.TASK:
            return 1;
        case TaskTypeEnum.MILESTONE:
            return 2;
        case TaskTypeEnum.SUMMARY_TASK:
            return 3;
        default:
            return undefined; // Для безопасности, если тип неизвестен
    }
};

export const getTaskTemplatesQuery = async (filters: ITaskTemplateFilterOptions): Promise<ITaskTemplate[]> => {
    const response = await apiClient.get(`task-templates`);
    const items = response.data;


    const res: ITaskTemplate[] = items.map((item: any) => ({
        id: item.Id ?? 0,
        name: item.TemplateName ?? 'Без названия',
        createdAt: item.CreatedOn ? dayjs(item.CreatedOn) : dayjs(), // Обязательное поле, используем текущую дату как fallback
        description: item.Description ?? undefined,
        priority: item.TaskPriority?.Id
            ? {
                id: item.TaskPriority.Id,
                name: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.name ?? 'Не назначен',
                color: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.color ?? 'blue',
            }
            : undefined,
        status: item.TaskStatus?.Id
            ? {
                id: item.TaskStatus.Id,
                name: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.name ?? 'Не назначен',
                color: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.color ?? 'blue',
            }
            : undefined,
        progress: item.Progress ?? undefined,
        tags: item.Tags?.map((tag: any) => ({
            id: tag.Id ?? 0,
            name: tag.Name ?? 'Без названия',
            color: tag.Color ?? '#000000',
        })) ?? [],
        taskName: item.TaskName ?? undefined,
        type: mapTaskTypeToEnum(item.TaskType?.Name),
        storyPoints: item.StoryPoints ?? undefined,
        endDate: item.EndDate ? dayjs(item.EndDate) : undefined,
        startDate: item.StartDate ? dayjs(item.StartDate) : undefined,
    }));

    return filterTaskTemplates(res, filters, 1);
};

export const getTaskTemplateQuery = async (templateId: number): Promise<ITaskTemplate> => {
    try {
        const response = await apiClient.get(`/task-templates/${templateId}`);
        const item = response.data;


        return {
            id: item.Id ?? 0,
            name: item.TemplateName ?? 'Без названия',
            createdAt: item.CreatedOn ? dayjs(item.CreatedOn) : dayjs(), // Обязательное поле, используем текущую дату как fallback
            description: item.Description ?? undefined,
            priority: item.TaskPriority?.Id
                ? {
                    id: item.TaskPriority.Id,
                    name: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.name ?? 'Не назначен',
                    color: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.color ?? 'blue',
                }
                : undefined,
            status: item.TaskStatus?.Id
                ? {
                    id: item.TaskStatus.Id,
                    name: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.name ?? 'Не назначен',
                    color: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.color ?? 'blue',
                }
                : undefined,
            progress: item.Progress ?? undefined,
            tags: item.Tags?.map((tag: any) => ({
                id: tag.Id ?? 0,
                name: tag.Name ?? 'Без названия',
                color: tag.Color ?? '#000000',
            })) ?? [],
            taskName: item.TaskName ?? undefined,
            type: mapTaskTypeToEnum(item.TaskType?.Name),
            endDate: item.EndDate ? dayjs(item.EndDate) : undefined,
            startDate: item.StartDate ? dayjs(item.StartDate) : undefined,
            storyPoints: item.StoryPoints ?? undefined,
        };
    } catch (e) {
        throw new Error(`Failed to fetch and parse task template with ID ${templateId}`);
    }
};

export const addTaskTemplateQuery = async (data: ITaskTemplateOptions): Promise<boolean> => {
    const response = await apiClient.post(`/task-templates`, {
        templateName: data.name ?? 'Без названия',
        taskName: data.taskName ?? undefined,
        description: data.description ?? undefined,
        taskStatusId: data.status?.id ?? undefined,
        taskPriorityId: data.priority?.id ?? undefined,
        taskTypeId: mapTaskTypeToId(data.type), // Используем вспомогательную функцию
        startDate: data.startDate ? dayjs(data.startDate).toISOString() : undefined,
        endDate: data.endDate ? dayjs(data.endDate).toISOString() : undefined,
        progress: data.progress ?? undefined,
        storyPoints: data.storyPoints ?? undefined,
        links: data.links?.map((l) => ({
            link: l.name ?? '',
            descriptions: l.description ?? undefined,
        })) ?? [],
        tagIds: data.tags?.map((t) => t.id) ?? [],
    });
    return response.status === 200;
};

export const editTaskTemplateQuery = async (templateId: number, data: ITaskTemplateOptions): Promise<boolean> => {
    try {
        const response = await apiClient.put(`/task-templates/${templateId}`, {
            templateName: data.name ?? 'Без названия',
            taskName: data.taskName ?? undefined,
            description: data.description ?? undefined,
            taskStatusId: data.status?.id ?? undefined,
            taskPriorityId: data.priority?.id ?? undefined,
            taskTypeId: mapTaskTypeToId(data.type), // Используем вспомогательную функцию
            startDate: data.startDate ? dayjs(data.startDate).toISOString() : undefined,
            endDate: data.endDate ? dayjs(data.endDate).toISOString() : undefined,
            progress: data.progress ?? undefined,
            storyPoints: data.storyPoints ?? undefined,
        });

        return response.status === 200;
    } catch (e) {
        throw new Error(`Failed to edit task template with ID ${templateId}`);
    }
};

export const removeTaskTemplateQuery = async (templateId: number): Promise<boolean> => {
    try {
        const response = await apiClient.delete(`/task-templates/${templateId}`);
        return response.status === 204;
    } catch (e) {
        throw new Error(`Failed to remove task template with ID ${templateId}`);
    }
};



export const getTaskTemplateLinksQuery = async (
    templateId: number,
): Promise<ILink[]> => {
    const response = await apiClient.get(`/task-templates/${templateId}/links`);
    const res = response.data.map((link: any) => ({
        id: link.Id,
        name: link.Description,
        link: link.Link,
        owner: {
            id: templateId,
            name: "",
            type: LinkOwnerEnum.TASK_TEMPLATE
        }
    }) as ILink)

    return res;
}

// add task template link
export const addTaskTemplateLinkQuery = async (
    templateId: number,
    data: IAddLinkOptions
): Promise<boolean> => {
    const response = await apiClient.post(`/task-templates/${templateId}/links`, {
        link: data.link,
        description: data.name
    });

    return response.status === 201
};

// edit task template link
export const editTaskTemplateLinkQuery = async (
    templateId: number,
    linkId: number,
    data: IEditLinkOptions
): Promise<boolean> => {
    const response = await apiClient.put(`/task-templates/${templateId}/links/${linkId}`, {
        link: data.link,
        description: data.name
    });

    return response.status === 204
};

// remove task template link
export const removeTaskTemplateLinkQuery = async (
    templateId: number,
    linkId: number,
): Promise<boolean> => {
    const response = await apiClient.delete(`/task-templates/${templateId}/links/${linkId}`);

    return response.status === 204
};

