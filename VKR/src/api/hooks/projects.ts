import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  addMemberToProjectQuery,
  addProjectQuery,
  addSprintToProjectQuery,
  addTaskReceiverQuery,
  editProjectDataQuery,
  getProjectMemberQuery,
  getProjectMembersQuery,
  getProjectQuery,
  getProjectsQuery,
  removeMemberFromProjectQuery,
  removeProjectQuery,
  removeTaskReceiverQuery,
} from "../queries/projectsQueries";
import { IProjectFilterOptions } from "../options/filterOptions/IProjectFilterOptions";
import { useState } from "react";

import { IAddProjectOptions } from "../options/createOptions/IAddProjectOptions";
import { notification } from "antd";
import { IEditProjectOptions } from "../options/editOptions/IEditProjectOptions";

import { IAddSprintOptions } from "../options/createOptions/IAddSprintOptions";

import { getManagerProjectsQuery, getWorkerProjectsQuery } from "../queries/workersQueries";
import { PageOwnerEnum } from "../../enums/ownerEntities/PageOwnerEnum";

export const useGetProjects = (entityId: number, pageType: PageOwnerEnum, enabled: boolean = true) => {
  const [filters, setFilters] = useState<IProjectFilterOptions>({});

  const query = useQuery({
    queryKey: ["projects", pageType, entityId, filters],
    enabled: enabled,
    queryFn: () => {
      if (pageType === PageOwnerEnum.ORGANIZATION) {
        return getProjectsQuery(filters, 1);
      }
      else {
        return getWorkerProjectsQuery(entityId, filters);
      }
    },
  });

  return {
    ...query,
    filters,
    setFilters,
  };
};


export const useGetProjectsWhereManager = (
  managerId: number,
  enabled: boolean
) => {
  const [filters, setFilters] = useState<IProjectFilterOptions>({});
  const limit: number = 30;

  const query = useInfiniteQuery({
    queryKey: ["projects", "manager", managerId, , filters],
    enabled: enabled,
    queryFn: () => {
      return getManagerProjectsQuery(managerId, filters);
    },
    getNextPageParam: (lastPage, allPages) => {
      if (limit === undefined) return undefined;
      return lastPage.length < limit ? undefined : allPages.length + 1;
    },
    initialPageParam: 1,
  });

  return {
    ...query,
    filters,
    setFilters,
  };
};

export const useGetProject = (projectId: number, enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["project", projectId],
    enabled: enabled,
    queryFn: () => {
      return getProjectQuery(projectId);
    },
  });

  return {
    ...query,
  };
};

export const useAddProject = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      managerId,
      options,
    }: {
      managerId: number;
      options: IAddProjectOptions;
    }) => {
      return addProjectQuery(managerId, options);
    },
    onSuccess: () => {
      handleSuccess?.();
      queryClient.invalidateQueries({ queryKey: ["projects"] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "projects"] });
      notification.success({ message: "Проект добавлен" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteProject = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({ projectId }: { projectId: number }) => {
      return removeProjectQuery(projectId);
    },
    onSuccess: (isSuccess: boolean) => {
      if (isSuccess) {
        handleSuccess?.();
        queryClient.invalidateQueries({ queryKey: ["projects"] });
        notification.success({ message: "Проект удален" });
      } else {
        notification.error({ message: "Ошибка при удалении проекта" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useEditProject = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      projectId,
      options,
    }: {
      projectId: number;
      options: IEditProjectOptions;
    }) => {
      return editProjectDataQuery(projectId, options);
    },
    onSuccess: (_, data) => {
      handleSuccess?.();
      queryClient.invalidateQueries({ queryKey: ["projects"] });
      queryClient.invalidateQueries({ queryKey: ["project", data.projectId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "projects"] });
      notification.success({ message: "Данные проекта изменены" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};



export const useAddSprintToProject = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      projectId,
      options,
    }: {
      projectId: number;
      options: IAddSprintOptions;
    }) => {
      return addSprintToProjectQuery(projectId, options);
    },
    onSuccess: (_, data) => {
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "sprints"] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "sprints"] });
      queryClient.invalidateQueries({ queryKey: ["project", data.projectId] });
      handleSuccess?.();
      notification.success({ message: "Спринт создан" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};


export const useGetProjectMembers = (
  projectId: number,
  enabled: boolean = true
) => {
  const query = useQuery({
    queryKey: ["project", projectId, "members"],
    enabled: enabled,
    queryFn: () => getProjectMembersQuery(projectId),
  });

  return {
    ...query,
  };
};

export const useAddMemberToProject = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      projectId,
      workerId,
    }: {
      projectId: number;
      workerId: number;
    }) => {
      return addMemberToProjectQuery(projectId, workerId);
    },
    onSuccess: (_, variables) => {
      handleSuccess?.();
      queryClient.invalidateQueries({ queryKey: ["project", variables.projectId, "member"] });
      queryClient.invalidateQueries({ queryKey: ["project", variables.projectId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "projects"] });
      notification.success({ message: "Сотрудник добавлен на проект" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteMemberFromProject = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      projectId,
      workerId,
    }: {
      projectId: number;
      workerId: number;
    }) => {
      return removeMemberFromProjectQuery(projectId, workerId);
    },
    onSuccess: (isSuccess: boolean, variables) => {
      if (isSuccess) {
        handleSuccess?.();
        queryClient.invalidateQueries({ queryKey: ["project", variables.projectId, "member"] });
        queryClient.invalidateQueries({ queryKey: ["project", variables.projectId] });
        queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "projects"] });
        notification.success({ message: "Сотрудник удален с проекта" });
      } else {
        notification.error({ message: "Ошибка при удалении сотрудника" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useGetProjectMember = (
  projectId: number,
  workerId: number,
  enabled: boolean = true
) => {
  const query = useQuery({
    queryKey: ["project", projectId, "member", workerId],
    enabled: enabled,
    queryFn: () => getProjectMemberQuery(projectId, workerId),
  });

  return {
    ...query,
  };
};

export const useAddTaskReceiver = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      projectId,
      workerId,
      receiverId,
    }: {
      projectId: number;
      workerId: number;
      receiverId: number;
    }) => addTaskReceiverQuery(projectId, workerId, receiverId),
    onSuccess: (_, variables) => {
      handleSuccess?.();
      queryClient.invalidateQueries({ queryKey: ["project", variables.projectId, "member"] });
      queryClient.invalidateQueries({ queryKey: ["project", variables.projectId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "tasks"] });
      notification.success({ message: "Получатель задачи добавлен" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};



export const useRemoveTaskReceiver = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      projectId,
      workerId,
      receiverId,
    }: {
      projectId: number;
      workerId: number;
      receiverId: number;
    }) => removeTaskReceiverQuery(projectId, workerId, receiverId),
    onSuccess: (_, variables) => {
      handleSuccess?.();
      queryClient.invalidateQueries({ queryKey: ["project", variables.projectId, "member"] });
      queryClient.invalidateQueries({ queryKey: ["project", variables.projectId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "tasks"] });
      notification.success({ message: "Получатель задачи удален" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};