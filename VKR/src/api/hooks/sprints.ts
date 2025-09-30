import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { IEditSprintOptions } from "../options/editOptions/IEditSprintOptions";
import { notification } from "antd";
import { getSprintQuery, addTaskToSprintQuery, editSprintQuery, getAvailableTasksForSprintQuery, getSprintTasksQuery, removeSprintQuery, removeTaskFromSprintQuery } from "../queries/sprintsQueries";
import { getProjectSprintsQuery } from "../queries/projectsQueries";
import { getWorkerSprintsQuery } from "../queries/workersQueries";
import { useState } from "react";
import { ISprintFilterOptions } from "../options/filterOptions/ISprintFilterOptions";
import { ITaskFilterOptions } from "../options/filterOptions/ITaskFilterOptions";
import { PageOwnerEnum } from "../../enums/ownerEntities/PageOwnerEnum";
import { ITask } from "../../interfaces/ITask";


export const useGetSprints = (
    pageType: PageOwnerEnum,
    entityId: number,
    enabled: boolean = true
) => {

    const [filters, setFilters] = useState<ISprintFilterOptions>({})
    const query = useQuery({
        queryKey: [pageType, 'sprints', entityId, filters],
        enabled: enabled,
        queryFn: () => {
            switch (pageType) {
                case PageOwnerEnum.PROJECT:
                    return getProjectSprintsQuery(entityId, filters);
                case PageOwnerEnum.WORKER:
                    return getWorkerSprintsQuery(entityId, filters);
                default:
                    return []
            }
        }
    })

    return {
        ...query,
        filters,
        setFilters
    }
}

export const useGetSprint = (sprintId: number, enabled: boolean = true) => {
    const query = useQuery({
        queryKey: ["sprint", sprintId],
        enabled: enabled,
        queryFn: () => getSprintQuery(sprintId)
    })
    return {
        ...query
    }
}


export const useEditSprint = (handleSuccess?: () => void) => {
    const queryClient = useQueryClient();
    const mutation = useMutation({
        mutationFn: ({
            sprintId,
            options
        }: {
            sprintId: number,
            options: IEditSprintOptions,
        }) => {
            return editSprintQuery(sprintId, options);
        },
        onSuccess: () => {
            notification.success({ message: "Данные спринта изменились" });
            queryClient.invalidateQueries({ queryKey: ["sprint"] });
            queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "sprints"] });
            queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "sprints"] });
            handleSuccess?.();
        },
        onError: (err: Error) => {
            notification.error({ message: err.message });
        }
    })

    return mutation.mutateAsync;
}

export const useDeleteSprint = (handleSuccess?: () => void) => {
    const queryClient = useQueryClient();
    const mutation = useMutation({
        mutationFn: ({
            sprintId,
        }: {
            sprintId: number,
        }) => {
            return removeSprintQuery(sprintId);
        },
        onSuccess: () => {
            notification.success({ message: "Спринт удален" });
            queryClient.invalidateQueries({ queryKey: ["sprint"] });
            queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "sprints"] });
            queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "sprints"] });
            handleSuccess?.();
        },
        onError: (err: Error) => {
            notification.error({ message: err.message });
        }
    })

    return mutation.mutateAsync;
}

export const useGetSprintTasks = (
    sprintId: number,
    projectId: number,
    enabled: boolean = true
) => {
    const [filters, setFilters] = useState<ITaskFilterOptions>({});

    const query = useQuery({
        queryKey: ['sprint', sprintId, 'tasks', filters],
        enabled: enabled,
        queryFn: () => {
            return getSprintTasksQuery(sprintId, projectId, filters, 1);
        }
    })

    return {
        ...query,
        filters,
        setFilters
    }
}


export const useAddTaskToSprint = (handleSuccess?: () => void) => {
    const queryClient = useQueryClient();
    const mutation = useMutation({
        mutationFn: ({
            sprintId,
            task
        }: {
            sprintId: number,
            task: ITask
        }) => {

            return addTaskToSprintQuery(task, sprintId);

        },
        onSuccess: (_, data) => {
            queryClient.invalidateQueries({ queryKey: ["sprint", data.sprintId] });
            queryClient.invalidateQueries({ queryKey: ["availableTasksForSprint", data.sprintId] });
            notification.success({ message: "Задача добавлена в спринт" });
            handleSuccess?.();
        },
        onError: (err: Error) => {
            notification.error({ message: err.message });
        }
    })

    return mutation.mutateAsync;
}

export const useGetAvaiableTasksForSprint = (sprintId: number, projectId: number, enabled: boolean = true) => {
    const [filters, setFilters] = useState<ITaskFilterOptions>({})
    const query = useQuery({
        queryKey: ["availableTasksForSprint", sprintId, filters],
        enabled: enabled,
        queryFn: () => {
            return getAvailableTasksForSprintQuery(sprintId, projectId, filters, 1)
        }
    })

    return {
        ...query,
        filters,
        setFilters
    }
}

export const useDeleteTaskFromSprint = (handleSuccess?: () => void) => {
    const queryClient = useQueryClient();
    const mutation = useMutation({
        mutationFn: ({
            sprintId,
            task
        }: {
            sprintId: number,
            task: ITask
        }) => {
            return removeTaskFromSprintQuery(sprintId, task);
        },
        onSuccess: (_, data) => {
            queryClient.invalidateQueries({ queryKey: ["sprint", data.sprintId] });
            queryClient.invalidateQueries({ queryKey: ["availableTasksForSprint", data.sprintId] });
            notification.success({ message: "Задача удалена из спринта" });
            handleSuccess?.();
        },
        onError: (err: Error) => {
            notification.error({ message: err.message });
        }
    })

    return mutation.mutateAsync;
}




