import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { notification } from "antd";
import { IAddLinkOptions } from "../options/createOptions/IAddLinkOptions";
import { IEditLinkOptions } from "../options/editOptions/IEditLinkOptions";
import { addProjectLinkQuery, editProjectLinkQuery, getProjectLinksQuery, removeProjectLinkQuery } from "../queries/projectsQueries";
import { addTaskLinkQuery, editTaskLinkQuery, getTaskLinksQuery, removeTaskLinkQuery } from "../queries/tasksQueries";
import { LinkOwnerEnum } from "../../enums/ownerEntities/LinkOwnerEnum";
import { addTaskTemplateLinkQuery, editTaskTemplateLinkQuery, getTaskTemplateLinksQuery, removeTaskTemplateLinkQuery } from "../queries/taskTemplatesQueries";

export const useGetLinks = (
  ownerId: number,
  ownerType: LinkOwnerEnum,
  enabled: boolean = true
) => {

  const linksQuery = useQuery({
    queryKey: [ownerType, ownerId, "links"],
    enabled: enabled,
    queryFn: () => {
      switch (ownerType) {
        case LinkOwnerEnum.PROJECT:
          return getProjectLinksQuery(ownerId);
        case LinkOwnerEnum.TASK:
          return getTaskLinksQuery(ownerId);
        case LinkOwnerEnum.TASK_TEMPLATE:
          return getTaskTemplateLinksQuery(ownerId);
      }
    },
  });

  return {
    ...linksQuery,
  };
};

export const useAddLink = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      ownerId,
      ownerType,
      options,
    }: {
      ownerId: number;
      ownerType: LinkOwnerEnum;
      options: IAddLinkOptions;
    }) => {
      switch (ownerType) {
        case LinkOwnerEnum.PROJECT:
          return addProjectLinkQuery(ownerId, options);
        case LinkOwnerEnum.TASK:
          return addTaskLinkQuery(ownerId, options);
        case LinkOwnerEnum.TASK_TEMPLATE:
          return addTaskTemplateLinkQuery(ownerId, options);
      }
    },
    onSuccess: (_, data) => {
      handleSuccess?.();
      queryClient.invalidateQueries({ queryKey: [data.ownerType, data.ownerId, "links"] });
      notification.success({ message: "Ссылка добавлена" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useEditLink = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      ownerId,
      ownerType,
      linkId,
      options,
    }: {
      ownerId: number;
      ownerType: LinkOwnerEnum;
      linkId: number;
      options: IEditLinkOptions;
    }) => {
      switch (ownerType) {
        case LinkOwnerEnum.PROJECT:
          return editProjectLinkQuery(ownerId, linkId, options);
        case LinkOwnerEnum.TASK:
          return editTaskLinkQuery(ownerId, linkId, options);
        case LinkOwnerEnum.TASK_TEMPLATE:
          return editTaskTemplateLinkQuery(ownerId, linkId, options);
      }
    },
    onSuccess: (_, data) => {
      handleSuccess?.();
      queryClient.invalidateQueries({ queryKey: [data.ownerType, data.ownerId, "links"] });
      notification.success({ message: "Ссылка изменена" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteLink = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      ownerId,
      ownerType,
      linkId,
    }: {
      ownerId: number;
      ownerType: LinkOwnerEnum;
      linkId: number;
    }) => {
      switch (ownerType) {
        case LinkOwnerEnum.PROJECT:
          return removeProjectLinkQuery(ownerId, linkId);
        case LinkOwnerEnum.TASK:
          return removeTaskLinkQuery(ownerId, linkId);
        case LinkOwnerEnum.TASK_TEMPLATE:
          return removeTaskTemplateLinkQuery(ownerId, linkId);
      }
    },
    onSuccess: (isSuccess: boolean, data) => {
      if (isSuccess) {
        handleSuccess?.();
        queryClient.invalidateQueries({ queryKey: [data.ownerType, data.ownerId, "links"] });
        notification.success({ message: "Ссылка удалена" });
      } else {
        notification.error({ message: "Ошибка при удалении ссылки" });
      }
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });

  return mutation.mutateAsync;
};
