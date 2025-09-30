import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  addColumnToBoardQuery,
  addTaskToBoardQuery,
  changeTaskColumnQuery,
  clearBoardQuery,
  editBoardQuery,
  getAvailableTasksForBoardQuery,
  getBoardColumnsQuery,
  getBoardQuery,
  getBoardTasksQuery,
  getCustomBoardTasks,
  removeBoardQuery,
  removeColumnFromBoardQuery,
  removeTaskFromBoardQuery,
} from "../queries/boardsQueries";
import { ITaskFilterOptions } from "../options/filterOptions/ITaskFilterOptions";
import { useState } from "react";
import { IAddBoardOptions } from "../options/createOptions/IAddBoardOptions";
import { IWorker } from "../../interfaces/IWorker";
import { notification } from "antd";
import { IAddBoardColumnOptions } from "../options/createOptions/IAddBoardColumnOptions";
import { IAddTaskToBoardOptions } from "../options/createOptions/IAddTasksToBoardOptions";
import { IEditBoardOptions } from "../options/editOptions/IEditBoardOptions";
import { addProjectBoardQuery, getProjectBoardsQuery } from "../queries/projectsQueries";
import { BoardOwnerEnum } from "../../enums/ownerEntities/BoardOwnerEnum";
import { addWorkerBoardQuery, getWorkerPersonalBoardsQuery, getWorkerProjectBoardsQuery } from "../queries/workersQueries";
import { IBoardFilterOptopns } from "../options/filterOptions/IBoardFilterOptions";



export const useGetBoards = (
  ownerType: BoardOwnerEnum,
  ownerId: number, type?: "personal" | "project",
  enabled: boolean = true
) => {
  const [filters, setFilters] = useState<IBoardFilterOptopns>({});
  const boardsQuery = useQuery({
    queryKey: ["boards", type, ownerType, ownerId, filters],
    enabled: enabled,
    queryFn: () => {
      if (ownerType === BoardOwnerEnum.WORKER) {
        switch (type) {
          case "personal":
            return getWorkerPersonalBoardsQuery(ownerId, filters, 1);
          case "project":
            return getWorkerProjectBoardsQuery(ownerId, filters, 1);
          default:
            return []
        }
      }
      else {
        return getProjectBoardsQuery(ownerId, filters, 1);
      }
    },
  });

  return {
    ...boardsQuery,
    filters,
    setFilters
  };
}


export const useGetBoard = (boardId: number, enabled: boolean = true) => {
  const boardQuery = useQuery({
    queryKey: ["board", boardId],
    enabled: enabled,
    queryFn: () => getBoardQuery(boardId),
  });

  return {
    ...boardQuery,
  };
};

export const useGetBoardTasks = (boardId: number, enabled: boolean = true) => {
  const [filters, setFilters] = useState<ITaskFilterOptions>({});

  const query = useQuery({
    queryKey: ["board", boardId, "tasks", filters],
    enabled: enabled,
    queryFn: () => getBoardTasksQuery(boardId, filters),
  });


  return {
    ...query,
    filters: filters,
    setFilters: setFilters,
  };
};

export const useGetCustomBoardTasks = (
  boardId: number,
  enabled: boolean = true,
) => {
  const [filters, setFilters] = useState<ITaskFilterOptions>({});
  const query = useQuery({
    queryKey: ['customBoard', boardId, 'tasks', filters],
    enabled: enabled && !!boardId,
    queryFn: () => getCustomBoardTasks(boardId, filters),
  });

  return {
    ...query,
    filters: filters,
    setFilters: setFilters,
  };
};

export const useGetAvailableBoardTasks = (boardId: number, workerId: number, enabled: boolean = true) => {
  const [filters, setFilters] = useState<ITaskFilterOptions>({});

  const query = useQuery({
    queryKey: ["board", boardId, "availableTasks", workerId, filters],
    enabled: enabled && !!boardId && !!workerId,
    queryFn: () => getAvailableTasksForBoardQuery(boardId, workerId, filters),
  });

  return {
    ...query,
    filters,
    setFilters,
  };
};

export const useAddBoard = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async ({
      entityType,
      entityId,
      options,
      creator,
    }: {
      entityType: BoardOwnerEnum;
      entityId: number;
      options: IAddBoardOptions;
      creator: IWorker | null;
    }) => {
      if (entityType === BoardOwnerEnum.PROJECT && creator) {
        return await addProjectBoardQuery(entityId, options, creator);
      } else {
        return await addWorkerBoardQuery(entityId, options);
      }
    },
    onSuccess: (_, data) => {
      notification.success({ message: "Доска создана" });
      queryClient.invalidateQueries({ queryKey: ["boards", data.entityType === BoardOwnerEnum.PROJECT ? "project" : "personal", data.entityType, data.entityId] });
      handleSuccess?.();
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};


export const useEditBoard = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async ({
      boardId,
      options,
    }: {
      boardId: number;
      options: IEditBoardOptions;
    }) => {
      return await editBoardQuery(boardId, options);
    },
    onSuccess: (_, data) => {
      notification.success({ message: "Данные доски изменены" });
      queryClient.invalidateQueries({ queryKey: ["boards"], exact: false }); // Invalidate all boards since we don't know the owner
      queryClient.invalidateQueries({ queryKey: ["board", data.boardId] });
      handleSuccess?.();
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteBoard = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async ({ boardId }: { boardId: number }) => {
      return await removeBoardQuery(boardId);
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        notification.success({ message: "Доска удалена" });
        queryClient.invalidateQueries({ queryKey: ["boards"], exact: false });
        queryClient.invalidateQueries({ queryKey: ["board", data.boardId] });
        queryClient.invalidateQueries({ queryKey: ["customBoard", data.boardId], exact: false });
        handleSuccess?.();
      } else {
        notification.error({ message: "Ошибка при удалении доски" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};


export const useGetCustomBoardColumns = (boardId?: number, enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["board", boardId, "columns"],
    enabled: enabled && !!boardId,
    queryFn: () => {
      if (boardId) {
        return getBoardColumnsQuery(boardId);
      } else {
        return [];
      }
    },
  });

  return {
    ...query,
  };
};

export const useAddCustomBoardColumn = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async ({
      boardId,
      options,
    }: {
      boardId: number;
      options: IAddBoardColumnOptions;
    }) => {
      return await addColumnToBoardQuery(boardId, options);
    },
    onSuccess: (_, data) => {
      queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "columns"] });
      queryClient.invalidateQueries({ queryKey: ["customBoard", data.boardId, "tasks"] });
      notification.success({ message: "Колонка добавлена" });
      handleSuccess?.();
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteCustomBoardColumn = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async ({
      boardId,
      columnName,
    }: {
      boardId: number;
      columnName: string;
    }) => {
      return await removeColumnFromBoardQuery(boardId, columnName);
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "columns"] });
        queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "tasks"] });
        queryClient.invalidateQueries({ queryKey: ["customBoard", data.boardId, "tasks"] });
        notification.success({ message: "Колонка удалена" });
        handleSuccess?.();
      } else {
        notification.error({ message: "Ошибка при удалении колонки" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};


export const useChangeTaskColumn = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async ({
      boardId,
      taskId,
      columnName,
    }: {
      boardId: number;
      taskId: number;
      columnName: string | undefined;
    }) => {
      return await changeTaskColumnQuery(boardId, taskId, columnName);
    },
    onSuccess: (_, data) => {
      queryClient.invalidateQueries({ queryKey: ["customBoard", data.boardId, "tasks"] });
      queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "tasks"] });
      notification.success({ message: "Колонка задачи изменена" });
      handleSuccess?.();
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};


export const useAddTaskToBoard = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async ({
      boardId,
      options,
    }: {
      boardId: number;
      options: IAddTaskToBoardOptions;
    }) => {
      return await addTaskToBoardQuery(boardId, options);
    },
    onSuccess: (_, data) => {
      notification.success({ message: "Задача добавлены на доску" });
      queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "tasks"] });
      queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "availableTasks"] });
      queryClient.invalidateQueries({ queryKey: ["customBoard", data.boardId, "tasks"] });
      handleSuccess?.();
    },
    onError: () => {
      notification.error({ message: "Ошибка добавления задачи на доску" });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteTaskFromBoard = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: async ({
      boardId,
      taskId,
    }: {
      boardId: number;
      taskId: number;
    }) => {
      return await removeTaskFromBoardQuery(boardId, taskId);
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "tasks"] });
        queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "availableTasks"] });
        queryClient.invalidateQueries({ queryKey: ["customBoard", data.boardId, "tasks"] });
        notification.success({ message: "Задача удалена с доски" });
        handleSuccess?.();
      } else {
        notification.error({ message: "Ошибка при удалении задачи с доски" });
      }
    },
    onError: () => {
      notification.error({ message: "Ошибка при удалении задачи с доски" });
    },
  });

  return mutation.mutateAsync;
};

export const useClearBoard = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: async ({ boardId }: { boardId: number }) => {
      return await clearBoardQuery(boardId);
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "tasks"] });
        queryClient.invalidateQueries({ queryKey: ["board", data.boardId, "availableTasks"] });
        queryClient.invalidateQueries({ queryKey: ["customBoard", data.boardId, "tasks"] });
        notification.success({ message: "Задачи удалены с доски" });
        handleSuccess?.();
      } else {
        notification.error({ message: "Ошибка при удалении задач с доски" });
      }
    },
    onError: () => {
      notification.error({ message: "Ошибка при удалении задач с доски" });
    },
  });

  return mutation.mutateAsync;
};
