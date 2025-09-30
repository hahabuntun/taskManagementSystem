import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { TagOwnerEnum } from "../../enums/ownerEntities/TagOwnerEnum";
import {
    getTaskTagsQuery,
    getAvailableTaskTagsQuery,
    getProjectTagsQuery,
    getAvailableProjectTagsQuery,
    getTemplateTagsQuery,
    getAvailableTemplateTagsQuery,
    getNumTasksForEachTag,
    getTasksByTag,
    addExistingTag,
    addNewTaskTag,
    deleteTag,
    addTags,
    addExistingProjectTag,
    addNewProjectTag,
    deleteProjectTag,
    addProjectTags,
    addExistingTemplateTag,
    addNewTemplateTag,
    deleteTemplateTag,
    addTemplateTags,
} from "../queries/tagQueries";
import { AddTagDTO, AddTagsDTO, CreateTagDTO } from "../../interfaces/ITag";

export const useGetAllTags = (owner: TagOwnerEnum, enabled: boolean = true) => {
    return useQuery({
        queryKey: [owner, "tags"],
        enabled,
        queryFn: () => {
            switch (owner) {
                case TagOwnerEnum.PROJECT:
                    return getProjectTagsQuery();
                case TagOwnerEnum.TASK:
                    return getTaskTagsQuery();
                case TagOwnerEnum.TASK_TEMPLATE:
                    return getTemplateTagsQuery();
                default:
                    throw new Error("Invalid TagOwnerEnum");
            }
        },
    });
};

export const useGetAllAvailableTagsForEntity = (
    owner: TagOwnerEnum,
    ownerId: number,
    enabled: boolean = true
) => {
    return useQuery({
        queryKey: ["available", owner, ownerId, "tags"],
        enabled,
        queryFn: () => {
            switch (owner) {
                case TagOwnerEnum.PROJECT:
                    return getAvailableProjectTagsQuery(ownerId);
                case TagOwnerEnum.TASK:
                    return getAvailableTaskTagsQuery(ownerId);
                case TagOwnerEnum.TASK_TEMPLATE:
                    return getAvailableTemplateTagsQuery(ownerId);
                default:
                    throw new Error("Invalid TagOwnerEnum");
            }
        },
    });
};

export const useGetTasksByTag = (tagName: string, enabled: boolean = true) => {
    return useQuery({
        queryKey: ["tasksByTag", tagName],
        enabled,
        queryFn: () => getTasksByTag(tagName),
    });
};

export const useGetNumTasksForEachTag = (enabled: boolean = true) => {
    return useQuery({
        queryKey: ["numTasksForTags"],
        enabled,
        queryFn: () => getNumTasksForEachTag(),
    });
};

// Task Tag Mutations
export const useAddExistingTaskTag = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ taskId, tag }: { taskId: number; tag: AddTagDTO }) =>
            addExistingTag(taskId, tag),
        onSuccess: (_, { taskId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.TASK, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["task", taskId] });
        },
    });
};

export const useAddNewTaskTag = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ taskId, tag }: { taskId: number; tag: CreateTagDTO }) =>
            addNewTaskTag(taskId, tag),
        onSuccess: (_, { taskId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.TASK, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["task", taskId] });
        },
    });
};

export const useDeleteTaskTag = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ taskId, tagId }: { taskId: number; tagId: number }) =>
            deleteTag(taskId, tagId),
        onSuccess: (_, { taskId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.TASK, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["task", taskId] });
        },
    });
};

export const useUpdateTaskTags = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ taskId, tags }: { taskId: number; tags: AddTagsDTO }) =>
            addTags(taskId, tags),
        onSuccess: (_, { taskId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.TASK, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["task", taskId] });
        },
    });
};

// Project Tag Mutations
export const useAddExistingProjectTag = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ projectId, tag }: { projectId: number; tag: AddTagDTO }) =>
            addExistingProjectTag(projectId, tag),
        onSuccess: (_, { projectId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.PROJECT, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["project", projectId] });
        },
    });
};

export const useAddNewProjectTag = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ projectId, tag }: { projectId: number; tag: CreateTagDTO }) =>
            addNewProjectTag(projectId, tag),
        onSuccess: (_, { projectId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.PROJECT, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["project", projectId] });
        },
    });
};

export const useDeleteProjectTag = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ projectId, tagId }: { projectId: number; tagId: number }) =>
            deleteProjectTag(projectId, tagId),
        onSuccess: (_, { projectId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.PROJECT, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["project", projectId] });
        },
    });
};

export const useUpdateProjectTags = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ projectId, tags }: { projectId: number; tags: AddTagsDTO }) =>
            addProjectTags(projectId, tags),
        onSuccess: (_, { projectId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.PROJECT, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["project", projectId] });
        },
    });
};

// Task Template Tag Mutations
export const useAddExistingTemplateTag = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ templateId, tag }: { templateId: number; tag: AddTagDTO }) =>
            addExistingTemplateTag(templateId, tag),
        onSuccess: (_, { templateId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.TASK_TEMPLATE, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["taskTemplate", templateId] });
        },
    });
};

export const useAddNewTemplateTag = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ templateId, tag }: { templateId: number; tag: CreateTagDTO }) =>
            addNewTemplateTag(templateId, tag),
        onSuccess: (_, { templateId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.TASK_TEMPLATE, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["taskTemplate", templateId] });
        },
    });
};

export const useDeleteTemplateTag = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ templateId, tagId }: { templateId: number; tagId: number }) =>
            deleteTemplateTag(templateId, tagId),
        onSuccess: (_, { templateId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.TASK_TEMPLATE, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["taskTemplate", templateId] });
        },
    });
};

export const useUpdateTemplateTags = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: ({ templateId, tags }: { templateId: number; tags: AddTagsDTO }) =>
            addTemplateTags(templateId, tags),
        onSuccess: (_, { templateId }) => {
            queryClient.invalidateQueries({ queryKey: [TagOwnerEnum.TASK_TEMPLATE, "tags"] });
            queryClient.invalidateQueries({ queryKey: ["taskTemplate", templateId] });
        },
    });
};