import { useState } from "react";
import { IWorkerFilterOptions } from "../options/filterOptions/IWorkerFilterOptions";
import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { IAddWorkerOptions } from "../options/createOptions/IAddWorkerOptions";
import { notification } from "antd";
import { IEditWorkerOptions } from "../options/editOptions/IEditWorkerOptions";
import { IAddSubordinateOptions } from "../options/createOptions/IAddSubordinatesOptions";
import { IProjectFilterOptions } from "../options/filterOptions/IProjectFilterOptions";

import { addSubordinateToWorkerQuery, addWorkerQuery, changeWorkerPasswordQuery, editWorkerDataQuery, getAvailableWorkerSubordinates, getAvailalbeWorkerFilesResponsibleWorkers, getAvailalbeWorkerHistoryResponsibleWorkers, getManagerProjectsQuery, getManagersQuery, getWorkerDirectorsQuery, getWorkerQuery, getWorkersQuery, getWorkerSubordinatesQuery, removeSubordinateFromWorkerQuery, removeWorkerQuery } from "../queries/workersQueries";
import { getAvailalbeTaskFilesResponsibleWorkers, getAvailalbeTaskHistoryResponsibleWorkers } from "../queries/tasksQueries";
import { getAvailalbeProjectFilesResponsibleWorkers, getAvailalbeProjectHistoryResponsibleWorkers } from "../queries/projectsQueries";
import { getAvailalbeOrganizationFilesResponsibleWorkers, getAvailalbeOrganizationHistoryResponsibleWorkers } from "../queries/organizationQueries";
import { HistoryOwnerEnum } from "../../enums/ownerEntities/HistoryOwnerEnum";
import { FileOwnerEnum } from "../../enums/ownerEntities/FileOwnerEnum";
import { PageOwnerEnum } from "../../enums/ownerEntities/PageOwnerEnum";


export const useGetWorkers = (enabled: boolean = true) => {
  const [filters, setFilters] = useState<IWorkerFilterOptions>({});
  const workersQuery = useQuery({
    queryKey: ["workers", "none", filters],
    enabled: enabled,
    queryFn: () => {
      return getWorkersQuery(filters);
    },
  });

  return {
    ...workersQuery,
    filters,
    setFilters,
  };
};

export const useGetWorker = (workerId: number, enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["worker", workerId],
    enabled: enabled,
    queryFn: () => {
      return getWorkerQuery(workerId);
    },
  });

  return {
    ...query,
  };
};

export const useGetManagers = (enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["managers"],
    enabled: enabled,
    queryFn: () => {
      return getManagersQuery();
    },
  });

  return {
    ...query,
  };
};

export const useAddWorker = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({ options }: { options: IAddWorkerOptions }) => {
      return addWorkerQuery(options);
    },
    onSuccess: (_) => {
      queryClient.invalidateQueries({ queryKey: ["workers"] });
      queryClient.invalidateQueries({ queryKey: ["managers"] });
      queryClient.invalidateQueries({ queryKey: ["worker", undefined, "availableSubordinates"] });
      handleSuccess?.();
      notification.success({ message: "Сотрудник добавлен" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message || "Ошибка при добавлении сотрудника" });
    },
  });

  return mutation.mutateAsync;
};

export const useEditWorker = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      workerId,
      options,
    }: {
      workerId: number;
      options: IEditWorkerOptions;
    }) => {
      return editWorkerDataQuery(workerId, options);
    },
    onSuccess: (_, data) => {
      queryClient.invalidateQueries({ queryKey: ["worker", data.workerId] });
      queryClient.invalidateQueries({ queryKey: ["workers"] });
      queryClient.invalidateQueries({ queryKey: ["managers"] });
      queryClient.invalidateQueries({ queryKey: ["worker", data.workerId, "subordinates"] });
      queryClient.invalidateQueries({ queryKey: ["worker", data.workerId, "directors"] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "projects"] });
      handleSuccess?.();
      notification.success({ message: "Данные сотрудника изменены" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message || "Ошибка при изменении данных сотрудника" });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteWorker = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({ workerId }: { workerId: number }) => {
      return removeWorkerQuery(workerId);
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        queryClient.invalidateQueries({ queryKey: ["workers"] });
        queryClient.invalidateQueries({ queryKey: ["worker", data.workerId] });
        queryClient.invalidateQueries({ queryKey: ["managers"] });
        queryClient.invalidateQueries({ queryKey: ["worker", undefined, "subordinates"] });
        queryClient.invalidateQueries({ queryKey: ["worker", undefined, "directors"] });
        queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "projects"] });
        queryClient.invalidateQueries({ queryKey: ["projects"] });
        handleSuccess?.();
        notification.success({ message: "Сотрудник удален" });
      } else {
        notification.error({ message: "Ошибка при удалении сотрудника" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message || "Ошибка при удалении сотрудника" });
    },
  });

  return mutation.mutateAsync;
};

export const useGetWorkerRelatedWorkers = (
  workerId: number,
  relationType: "directors" | "subordinates",
  enabled: boolean = true,
) => {

  const [filters, setFilters] = useState<IWorkerFilterOptions>({});
  const query = useQuery({
    queryKey: ["worker", workerId, relationType, filters],
    enabled: enabled,
    queryFn: () => {
      if (relationType === "directors") {
        return getWorkerDirectorsQuery(workerId, filters);
      }
      else {
        return getWorkerSubordinatesQuery(workerId, filters);
      }
    },
  });

  return {
    filters,
    setFilters,
    ...query,
  };
};


export const useGetAvailableSubordinates = (workerId: number, enabled: boolean = true) => {
  const [filters, setFilters] = useState<IWorkerFilterOptions>({});
  const query = useQuery({
    queryKey: ['worker', workerId, 'availableSubordinates', filters],
    enabled: enabled,
    queryFn: () => {
      return getAvailableWorkerSubordinates(workerId, filters)
    }
  })

  return {
    ...query,
    filters,
    setFilters
  }
}

export const useAddSubordinateToWorker = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      workerId,
      options,
    }: {
      workerId: number;
      options: IAddSubordinateOptions;
    }) => {
      return addSubordinateToWorkerQuery(workerId, options);
    },
    onSuccess: (_, data) => {
      queryClient.invalidateQueries({ queryKey: ["worker", data.workerId, "subordinates"] });
      queryClient.invalidateQueries({ queryKey: ["worker", data.workerId, "availableSubordinates"] });
      queryClient.invalidateQueries({ queryKey: ["worker"] });
      queryClient.invalidateQueries({ queryKey: ["workers"] });
      handleSuccess?.();
      notification.success({ message: "Подчиненный добавлен" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message || "Ошибка при добавлении подчиненного" });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteSubordinateFromWorker = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      workerId,
      subordinateId,
    }: {
      workerId: number;
      subordinateId: number;
    }) => {
      return removeSubordinateFromWorkerQuery(workerId, subordinateId);
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        queryClient.invalidateQueries({
          queryKey: ["worker", data.workerId, "subordinates"],
        });
        queryClient.invalidateQueries({
          queryKey: ["worker", data.workerId, "availableSubordinates"],
        });
        handleSuccess?.();
        notification.success({ message: "Подчиненный удален" });
      } else {
        notification.error({ message: "Ошибка при удалении подчиненного" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};


export const useGetManagerProjects = (managerId: number, enabled: boolean) => {
  const [filters, setFilters] = useState<IProjectFilterOptions>({});
  const limit: number = 30;

  const query = useInfiniteQuery({
    queryKey: ["manager", managerId, "projects", filters],
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




export const useGetAvailableResponsibleWorkerForFilesFiltering = (
  ownerEntity: FileOwnerEnum,
  ownerEntityId: number,
  enabled: boolean = true
) => {
  console.log(ownerEntityId)
  const query = useQuery({
    queryKey: ["available-responsible-workers-for-files", ownerEntity],
    enabled: enabled,
    queryFn: () => {
      switch (ownerEntity) {
        case FileOwnerEnum.ORGANIZATION:
          return getAvailalbeOrganizationFilesResponsibleWorkers();
        case FileOwnerEnum.PROJECT:
          return getAvailalbeProjectFilesResponsibleWorkers();
        case FileOwnerEnum.WORKER:
          return getAvailalbeWorkerFilesResponsibleWorkers();
        case FileOwnerEnum.TASK:
          return getAvailalbeTaskFilesResponsibleWorkers();
      }
    },
  });

  return {
    ...query,
  };
};

export const useGetAvailableResponsibleWorkerForHistoryFiltering = (
  ownerEntity: HistoryOwnerEnum,
  ownerEntityId: number,
  enabled: boolean = true
) => {
  const query = useQuery({
    queryKey: ["available-responsible-workers-for-history", ownerEntity],
    enabled: enabled,
    queryFn: () => {
      switch (ownerEntity) {
        case HistoryOwnerEnum.ORGANIZATION:
          return getAvailalbeOrganizationHistoryResponsibleWorkers();
        case HistoryOwnerEnum.PROJECT:
          return getAvailalbeProjectHistoryResponsibleWorkers(ownerEntityId);
        case HistoryOwnerEnum.WORKER:
          return getAvailalbeWorkerHistoryResponsibleWorkers(ownerEntityId);
        case HistoryOwnerEnum.TASK:
          return getAvailalbeTaskHistoryResponsibleWorkers(ownerEntityId);
      }
    },
  });

  return {
    ...query,
  };
};



interface IChangePasswordParams {
  workerId: number;
  newPassword: string;
}

export const useChangePassword = () => {
  const mutation = useMutation({
    mutationFn: ({ workerId, newPassword }: IChangePasswordParams) =>
      changeWorkerPasswordQuery(workerId, newPassword),
    onSuccess: () => {
      notification.success({ message: "Пароль изменен" });
    },
    onError: () => {
      notification.success({ message: "Ошибка при изменении пароля" });
    }
  });

  return mutation.mutateAsync;
};