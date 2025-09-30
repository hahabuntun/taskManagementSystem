import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { notification } from "antd";
import { IAddWorkerPositionOptions } from "../options/createOptions/IAddWorkerPositionOptions";
import { IEditWorkerPositionOptions } from "../options/editOptions/IEditWorkerPositionOptions";
import { addWorkerPositionQuery, editWorkerPositionQuery, getWorkerPositionQuery, getWorkerPositionsQuery, removeWorkerPositionQuery } from "../queries/workerPositionsQueries";

export const useGetWorkerPositions = (enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["workerPositions"],
    enabled: enabled,
    queryFn: () => {
      return getWorkerPositionsQuery();
    },
  });

  return {
    ...query,
  };
};

export const useGetWorkerPosition = (
  workerPositionId: number,
  enabled: boolean = true
) => {
  console.log("we here")
  const query = useQuery({
    queryKey: ["workerPosition", workerPositionId],
    enabled: enabled,
    queryFn: () => {
      return getWorkerPositionQuery(workerPositionId);
    },
  });

  return {
    ...query,
  };
};

export const useAddWorkerPosition = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: ({ options }: { options: IAddWorkerPositionOptions }) => {
      return addWorkerPositionQuery(options);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["workerPositions"],
      });
      handleSuccess?.();
      notification.success({ message: "Должность добавлена" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteWorkerPosition = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({ workerPositionId }: { workerPositionId: number }) => {
      return removeWorkerPositionQuery(workerPositionId);
    },
    onSuccess: (isSuccess: boolean) => {
      if (isSuccess) {
        queryClient.invalidateQueries({
          queryKey: ["workerPositions"],
        });
        handleSuccess?.();
        notification.success({ message: "Должность удалена" });
      } else {
        notification.error({ message: "Ошибка при удалении должности" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useEditWorkerPosition = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      workerPositionId,
      options,
    }: {
      workerPositionId: number;
      options: IEditWorkerPositionOptions;
    }) => {
      return editWorkerPositionQuery(workerPositionId, options);
    },
    onSuccess: (_, data) => {
      queryClient.invalidateQueries({
        queryKey: ["workerPositions", data.workerPositionId],
      });
      queryClient.invalidateQueries({
        queryKey: ["workerPositions"],
      });
      handleSuccess?.();
      notification.success({ message: "Данные должности изменены" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};
