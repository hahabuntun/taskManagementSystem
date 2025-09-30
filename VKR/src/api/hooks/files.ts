import { useState } from "react";
import { IFileFilterOptions } from "../options/filterOptions/IFileFilterOptions";
import { useInfiniteQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { IAddFileOptions } from "../options/createOptions/IAddFileOptions";
import { notification } from "antd";
import { addProjectFileQuery, downloadProjectFileQuery, getProjectFilesQuery, removeProjectFileQuery } from "../queries/projectsQueries";
import { addOrganizationFileQuery, downloadOrganizationFileQuery, getOrganizationFilesQuery, removeOrganizationFileQuery } from "../queries/organizationQueries";
import { addWorkerFileQuery, downloadWorkerFileQuery, getWorkerFilesQuery, removeWorkerFileQuery } from "../queries/workersQueries";
import { addTaskFileQuery, downloadTaskFileQuery, getTaskFilesQuery, removeTaskFileQuery } from "../queries/tasksQueries";
import { FileOwnerEnum } from "../../enums/ownerEntities/FileOwnerEnum";

export const useGetFiles = (
  ownerId: number,
  ownerType: FileOwnerEnum,
  enabled: boolean = true
) => {
  const [filters, setFilters] = useState<IFileFilterOptions>({});
  const limit: number = 30;

  const filesQuery = useInfiniteQuery({
    queryKey: [ownerType, ownerId, "files", filters],
    enabled: enabled,
    queryFn: ({ pageParam = 1 }) => {
      switch (ownerType) {
        case FileOwnerEnum.ORGANIZATION:
          return getOrganizationFilesQuery(ownerId, filters, pageParam, limit);
        case FileOwnerEnum.PROJECT:
          return getProjectFilesQuery(ownerId, filters, pageParam, limit);
        case FileOwnerEnum.WORKER:
          return getWorkerFilesQuery(ownerId, filters, pageParam, limit);
        case FileOwnerEnum.TASK:
          return getTaskFilesQuery(ownerId, filters, pageParam, limit);
        case FileOwnerEnum.TASK_TEMPLATE:
          return []
      }
    },
    getNextPageParam: (lastPage, allPages) => {
      if (limit === undefined) return undefined;
      return lastPage.length < limit ? undefined : allPages.length + 1;
    },
    initialPageParam: 1,
  });

  return {
    ...filesQuery,
    filters,
    setFilters,
  };
};

export const useAddFile = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      ownerId,
      ownerType,
      options,
    }: {
      ownerId: number;
      ownerType: FileOwnerEnum;
      options: IAddFileOptions;
    }) => {
      switch (ownerType) {
        case FileOwnerEnum.ORGANIZATION:
          return addOrganizationFileQuery(options);
        case FileOwnerEnum.PROJECT:
          return addProjectFileQuery(ownerId, options);
        case FileOwnerEnum.WORKER:
          return addWorkerFileQuery(ownerId, options);
        case FileOwnerEnum.TASK:
          return addTaskFileQuery(ownerId, options);
        case FileOwnerEnum.TASK_TEMPLATE:
          throw new Error();
      }
    },
    onSuccess: (_, data) => {
      handleSuccess?.();
      queryClient.invalidateQueries({ queryKey: ["available-responsible-workers-for-files", data.ownerType] })
      queryClient.invalidateQueries({ queryKey: [data.ownerType, data.ownerId, 'files'] })
      notification.success({ message: "Файл добавлен" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteFile = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      ownerId,
      ownerType,
      fileId,
    }: {
      ownerId: number;
      ownerType: FileOwnerEnum;
      fileId: number;
    }) => {
      switch (ownerType) {
        case FileOwnerEnum.ORGANIZATION:
          return removeOrganizationFileQuery(fileId);
        case FileOwnerEnum.PROJECT:
          return removeProjectFileQuery(ownerId, fileId);
        case FileOwnerEnum.WORKER:
          return removeWorkerFileQuery(ownerId, fileId);
        case FileOwnerEnum.TASK:
          return removeTaskFileQuery(ownerId, fileId);
        case FileOwnerEnum.TASK_TEMPLATE:
          throw new Error();
      }
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        handleSuccess?.();
        queryClient.invalidateQueries({ queryKey: ["available-responsible-workers-for-files", data.ownerType] })
        queryClient.invalidateQueries({ queryKey: [data.ownerType, data.ownerId, 'files'] })
        notification.success({ message: "Файл удален" });
      } else {
        notification.error({ message: "Ошибка при удалении файла" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};


export const useDownloadFile = () => {
  const mutation = useMutation({
    mutationFn: async ({
      fileId,
      ownerId,
      ownerType,
    }: {
      fileId: number;
      ownerId: number;
      ownerType: FileOwnerEnum;
    }) => {
      let blob;
      switch (ownerType) {
        case FileOwnerEnum.ORGANIZATION:
          blob = await downloadOrganizationFileQuery(ownerId, fileId);
          break;
        case FileOwnerEnum.PROJECT:
          blob = await downloadProjectFileQuery(ownerId, fileId);
          break;
        case FileOwnerEnum.WORKER:
          blob = await downloadWorkerFileQuery(ownerId, fileId);
          break;
        case FileOwnerEnum.TASK:
          blob = await downloadTaskFileQuery(ownerId, fileId);
          break;
        case FileOwnerEnum.TASK_TEMPLATE:
          throw new Error("Download not supported for task templates");
        default:
          throw new Error("Unsupported owner type");
      }
      return { blob, fileName: `file_${fileId}.dat` }; // Предполагаемое имя файла, адаптируйте под ваши данные
    },
    onSuccess: (data) => {
      const url = window.URL.createObjectURL(data.blob);
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", data.fileName); // Устанавливаем имя файла для скачивания
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
      notification.success({ message: "Файл успешно загружен" });
    },
    onError: (err: Error) => {
      notification.error({ message: `Ошибка при загрузке файла: ${err.message}` });
    },
  });

  return mutation.mutateAsync;
};
