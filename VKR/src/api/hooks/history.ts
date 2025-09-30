import { useState } from "react";
import { IHistoryFilterOptions } from "../options/filterOptions/IHistoryFilterOptions";
import { useInfiniteQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { notification } from "antd";
import { getOrganizationHistoryQuery, removeOrganizationHistoryItemQuery, removeOrganizationHistoryQuery } from "../queries/organizationQueries";
import { getProjectHistoryQuery, removeProjectHistoryItemQuery, removeProjectHistoryQuery } from "../queries/projectsQueries";
import { getWorkerHistoryQuery, removeWorkerHistoryItemQuery, removeWorkerHistoryQuery } from "../queries/workersQueries";
import { getTaskHistoryQuery, removeTaskHistoryItemQuery, removeTaskHistoryQuery } from "../queries/tasksQueries";
import { HistoryOwnerEnum } from "../../enums/ownerEntities/HistoryOwnerEnum";
import { getBoardHistoryQuery, removeBoardHistoryItemQuery, removeBoardHistoryQuery } from "../queries/boardsQueries";
import { getSprintHistoryQuery, removeSprintHistoryItemQuery, removeSprintHistoryQuery } from "../queries/sprintsQueries";

export const useGetHistory = (
  ownerId: number,
  ownerType: HistoryOwnerEnum,
  enabled: boolean = true
) => {
  const [filters, setFilters] = useState<IHistoryFilterOptions>({});
  const limit: number = 30;

  const historyQuery = useInfiniteQuery({
    queryKey: ["history", ownerType, ownerId, filters],
    enabled: enabled,
    queryFn: ({ pageParam = 1 }) => {
      switch (ownerType) {
        case HistoryOwnerEnum.ORGANIZATION:
          return getOrganizationHistoryQuery(
            ownerId,
            filters,
            pageParam,
            limit
          );
        case HistoryOwnerEnum.PROJECT:
          return getProjectHistoryQuery(ownerId, filters, pageParam, limit);
        case HistoryOwnerEnum.WORKER:
          return getWorkerHistoryQuery(ownerId, filters, pageParam, limit);
        case HistoryOwnerEnum.TASK:
          return getTaskHistoryQuery(ownerId, filters, pageParam, limit);
        case HistoryOwnerEnum.BOARD:
          return getBoardHistoryQuery(ownerId, filters, pageParam, limit);
        case HistoryOwnerEnum.SPRINT:
          return getSprintHistoryQuery(ownerId, filters, pageParam, limit);
      }
    },
    getNextPageParam: (lastPage, allPages) => {
      if (limit === undefined) return undefined;
      return lastPage.length < limit ? undefined : allPages.length + 1;
    },
    initialPageParam: 1,
  });

  return {
    ...historyQuery,
    filters,
    setFilters,
  };
};

export const useDeleteHistoryItem = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      ownerType,
      itemId,
    }: {
      ownerType: HistoryOwnerEnum;
      itemId: number;
    }) => {
      switch (ownerType) {
        case HistoryOwnerEnum.ORGANIZATION:
          return removeOrganizationHistoryItemQuery(itemId);
        case HistoryOwnerEnum.PROJECT:
          return removeProjectHistoryItemQuery(itemId);
        case HistoryOwnerEnum.WORKER:
          return removeWorkerHistoryItemQuery(itemId);
        case HistoryOwnerEnum.TASK:
          return removeTaskHistoryItemQuery(itemId);
        case HistoryOwnerEnum.BOARD:
          return removeBoardHistoryItemQuery(itemId);
        case HistoryOwnerEnum.SPRINT:
          return removeSprintHistoryItemQuery(itemId);
      }
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        handleSuccess?.();
        queryClient.invalidateQueries({ queryKey: ["history", data.ownerType] })
        queryClient.invalidateQueries({ queryKey: ["available-responsible-workers-for-history", data.ownerType] })
        notification.success({ message: "Запись удалена" });
      } else {
        notification.error({ message: "Ошибка при удалении записи" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteHistory = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      ownerType,
      ownerId,
    }: {
      ownerType: HistoryOwnerEnum;
      ownerId: number;
    }) => {
      switch (ownerType) {
        case HistoryOwnerEnum.ORGANIZATION:
          return removeOrganizationHistoryQuery();
        case HistoryOwnerEnum.PROJECT:
          return removeProjectHistoryQuery(ownerId);
        case HistoryOwnerEnum.WORKER:
          return removeWorkerHistoryQuery(ownerId);
        case HistoryOwnerEnum.TASK:
          return removeTaskHistoryQuery(ownerId);
        case HistoryOwnerEnum.BOARD:
          return removeBoardHistoryQuery(ownerId);
        case HistoryOwnerEnum.SPRINT:
          return removeSprintHistoryQuery(ownerId);
      }
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        handleSuccess?.();
        queryClient.invalidateQueries({ queryKey: ["history", data.ownerType] })
        queryClient.invalidateQueries({ queryKey: ["available-responsible-workers-for-history", data.ownerType] })
        notification.success({ message: "Записи удалена" });
      } else {
        notification.error({ message: "Ошибка при удалении записей" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};
