import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { IAddTaskOptions } from "../options/createOptions/IAddTaskOptions";
import { IWorker } from "../../interfaces/IWorker";
import { addTaskToProjectQuery, getProjectTasksQuery } from "../queries/projectsQueries";
import { notification } from "antd";
import {
  addRelatedTaskQuery,
  addTaskCommentQuery,
  addTaskExecutor,
  addTaskFilterTo,
  addTaskObserver,
  editTaskFilter,
  editTaskQuery,
  getAllTaskTakersQuery,
  getAvailableRelatedTasks,
  getRelatedTasksQuery,
  getTaskCommentsQuery,
  getTaskFilters,
  getTaskQuery,
  removeRelationBetweenTasksQuery,
  removeTaskCommentQuery,
  removeTaskExecutor,
  removeTaskFilter,
  removeTaskObserver,
  removeTaskQuery,
  updateTaskExecutorResponsible,
  updateTaskResponsibleWorker
} from "../queries/tasksQueries";
import { IEditTaskOptions } from "../options/editOptions/IEditTaskOptions";
import { TaskRelationshipTypeEnum } from "../../enums/TaskRelationshipTypeEnum";
import { useState } from "react";
import { ITaskFilterOptions } from "../options/filterOptions/ITaskFilterOptions";
import { getWorkerTasksWhereCreatorQuery, getWorkerTasksWhereExecutorQuery, getWorkerTasksWhereResponsibleQuery, getWorkerTasksWhereViewerQuery } from "../queries/workersQueries";
import { IAddTaskCommentOptions } from "../options/createOptions/IAddTaskCommentOptions";
import { ITaskComment } from "../../interfaces/ITaskComment";
import { PageOwnerEnum } from "../../enums/ownerEntities/PageOwnerEnum";
import { ITask } from "../../interfaces/ITask";
import { ITaskTemplateFilterOptions } from "../options/filterOptions/ITaskTemplateFilterOptions";
import { addTaskTemplateQuery, editTaskTemplateQuery, getTaskTemplateQuery, getTaskTemplatesQuery, removeTaskTemplateQuery } from "../queries/taskTemplatesQueries";
import { ITaskTemplateOptions } from "../options/ITaskTemplateOptions";
import { addTaskToSprintQuery, getSprintTasksQuery } from "../queries/sprintsQueries";
import { ITaskExecutorForm, ITaskObserverForm } from "../../interfaces/ITaskWorker";



export const useGetTasks = ({
  entityId,
  pageType,
  howRelatesToTask,
  projectId,
  enabled
}: {
  entityId: number,
  pageType: PageOwnerEnum,
  howRelatesToTask?: "creator" | "executor" | "responsible" | "viewer",
  projectId?: number,
  limit?: number,
  enabled: boolean
}) => {
  const [filters, setFilters] = useState<ITaskFilterOptions>({});
  const query = useQuery({
    queryKey: [pageType, "tasks", entityId, howRelatesToTask, filters],
    enabled: enabled,
    queryFn: () => {
      if (pageType === PageOwnerEnum.WORKER) {
        switch (howRelatesToTask) {
          case "creator":
            return getWorkerTasksWhereCreatorQuery(
              entityId,
              filters,
              1,
            );
          case "executor":
            return getWorkerTasksWhereExecutorQuery(
              entityId,
              filters,
            );
          case "responsible":
            return getWorkerTasksWhereResponsibleQuery(
              entityId,
              filters,
              1
            );
          case "viewer":
            return getWorkerTasksWhereViewerQuery(
              entityId,
              filters,
              1
            );
          default:
            return []
        }
      }
      else if (pageType === PageOwnerEnum.PROJECT) {

        return getProjectTasksQuery(entityId, filters, 1);
      }
      else if (pageType === PageOwnerEnum.SPRINT && projectId) {

        return getSprintTasksQuery(entityId, projectId, filters, 1);
      }
      else {

        return []
      }

    },
  });

  return {
    ...query,
    filters,
    setFilters,
  };
}


export const useAddTask = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      projectId,
      options,
      creator,
    }: {
      projectId: number;
      options: IAddTaskOptions;
      creator: IWorker;
    }) => {
      return addTaskToProjectQuery(projectId, options, creator);
    },
    onSuccess: (_, data) => {
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "tasks", data.projectId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "tasks"] });
      queryClient.invalidateQueries({ queryKey: ["board"], exact: false });
      handleSuccess?.();
      notification.success({ message: "Задача добавлена" });
    },
    onError: () => {
      notification.error({
        message: "Ошибка при добавлении задачи"
      });
    },
  });

  return mutation.mutateAsync;
};

export const useGetTask = (taskId: number, enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["task", taskId],
    enabled: enabled,
    queryFn: () => {
      return getTaskQuery(taskId);
    },
  });

  return {
    ...query,
  };
};

export const useDeleteTask = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({ task }: { task: ITask }) => {
      return removeTaskQuery(task);
    },
    onSuccess: (res, { task }) => {
      if (res) {
        queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "tasks", task.project.id] });
        queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "tasks"] });
        queryClient.invalidateQueries({ queryKey: ["task", task.id] });
        queryClient.invalidateQueries({ queryKey: ["board"], exact: false });
        handleSuccess?.();
        notification.success({ message: "Задача удалена" });
      } else {
        notification.error({ message: "Ошибка при удалении задачи" });
      }
    },
    onError: () => {
      notification.error({ message: "Ошибка при удалении задачи" });
    },
  });

  return mutation.mutateAsync;
};

export const useEditTaskData = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: ({
      taskId,
      options,
    }: {
      taskId: number;
      options: IEditTaskOptions;
    }) => {
      return editTaskQuery(taskId, options);
    },
    onSuccess: (_, data) => {
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "tasks"], exact: false });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.SPRINT, "tasks"], exact: false });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "tasks"], exact: false });
      queryClient.invalidateQueries({ queryKey: ["task", data.taskId] });
      queryClient.invalidateQueries({ queryKey: ["board"], exact: false });
      handleSuccess?.();
      notification.success({ message: "Данные задачи изменены" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при изменении данных задачи" });
    },
  });

  return mutation.mutateAsync;
};



export const useChangeTaskSprint = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: ({
      task,
      sprintId,
    }: {
      task: ITask;
      sprintId?: number;
    }) => addTaskToSprintQuery(task, sprintId),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["task"],
      });
      queryClient.invalidateQueries({
        queryKey: [PageOwnerEnum.WORKER, "tasks"],
      });
      queryClient.invalidateQueries({
        queryKey: [PageOwnerEnum.PROJECT, "tasks"],
      });
      queryClient.invalidateQueries({
        queryKey: [PageOwnerEnum.PROJECT, "sprints"],
      });
      queryClient.invalidateQueries({
        queryKey: [PageOwnerEnum.WORKER, "sprints"],
      });
      queryClient.invalidateQueries({
        queryKey: ["board"],
        exact: false,
      });
      handleSuccess?.();
      notification.success({ message: "Спринт задачи изменен" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при изменении спринта" });
    },
  });

  return mutation.mutateAsync;
};

export const useGetRelatedTasks = (
  taskId: number,
  relType: TaskRelationshipTypeEnum,
  enabled?: boolean
) => {
  const [filters, setFilters] = useState<ITaskFilterOptions>({});
  const query = useQuery({
    queryKey: ["task", taskId, "related", relType, filters],
    enabled: enabled,
    queryFn: () => {
      return getRelatedTasksQuery(taskId, relType, filters);
    },
  });
  return {
    ...query,
    filters,
    setFilters
  };
};

export const useAddRelatedTask = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      taskId,
      relatedTaskId,
      relType,
    }: {
      taskId: number;
      relatedTaskId: number;
      relType: TaskRelationshipTypeEnum;
      lag?: number;
    }) => {
      return addRelatedTaskQuery(taskId, relatedTaskId, relType);
    },
    onSuccess: (_, data) => {
      // Invalidate all related tasks queries for taskId
      queryClient.invalidateQueries({
        queryKey: ["task", data.taskId, "related"],
        exact: false, // Match all variations (including relType and filters)
      });
      // Invalidate available related tasks
      queryClient.invalidateQueries({
        queryKey: ["availableRelatedTasks"],
        exact: false,
      });
      handleSuccess?.();
      notification.success({ message: "Связанная задача добавлена" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при добавлении связи" });
    },
  });
  return mutation.mutateAsync;
};

export const useDeleteRelationBetweenTasks = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({
      taskId,
      relatedTaskId,
    }: {
      taskId: number;
      relatedTaskId: number;
    }) => {
      return removeRelationBetweenTasksQuery(taskId, relatedTaskId);
    },
    onSuccess: (isSuccess, data) => {
      if (isSuccess) {
        // Invalidate all related tasks queries for taskId
        queryClient.invalidateQueries({
          queryKey: ["task", data.taskId, "related"],
          exact: false, // Match all variations (including relType and filters)
        });
        // Invalidate available related tasks
        queryClient.invalidateQueries({
          queryKey: ["availableRelatedTasks"],
          exact: false,
        });
        handleSuccess?.();
        notification.success({ message: "Связь удалена" });
      } else {
        notification.error({ message: "Ошибка при удалении связи" });
      }
    },
    onError: () => {
      notification.error({ message: "Ошибка при удалении связи" });
    },
  });
  return mutation.mutateAsync;
};



export const useGetAvailableRelatedTasks = (
  projectId: number,
  taskId: number,
) => {
  const [filters, setFilters] = useState<ITaskFilterOptions>({});
  const query = useQuery({
    queryKey: ["availableRelatedTasks", projectId, taskId, filters],
    queryFn: () => getAvailableRelatedTasks(taskId),
    enabled: !!projectId && !!taskId,
  });
  return {
    ...query,
    setFilters
  }
};


export const useGetTaskComments = (taskId: number, enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["task", taskId, "comments"],
    enabled: enabled,
    queryFn: () => {
      return getTaskCommentsQuery(taskId)
    }
  })

  return {
    ...query
  }
}

export const useAddTaskComment = (handleSucces?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (
      {
        taskId,
        options,
        creator
      }:
        {
          taskId: number,
          options: IAddTaskCommentOptions,
          creator: IWorker
        }) => addTaskCommentQuery(taskId, options, creator),
    onSuccess: (_, data) => {
      queryClient.invalidateQueries({
        queryKey: ["task", data.taskId, "comments"],
      });
      notification.success({ message: "Комментарий добавлен" });
      handleSucces?.();
    },
    onError: () => {
      notification.error({ message: "Ошибка при добавлении комментария" });
    }
  }
  )

  return mutation.mutateAsync;
}


export const useDeleteTaskComment = (handleSucces?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (
      {
        comment,
      }:
        {
          comment: ITaskComment,
        }) => removeTaskCommentQuery(comment.id),
    onSuccess: (isSucces: boolean, data) => {
      if (isSucces) {
        queryClient.invalidateQueries({
          queryKey: ["task", data.comment.taskId, "comments"],
        });
        notification.success({ message: "Комментарий удален" });
        handleSucces?.();
      }
      else {
        notification.error({ message: "Ошибка при удалении комментария" });
      }

    },
    onError: () => {
      notification.error({ message: "Ошибка при удалении комментария" });
    }
  }
  )

  return mutation.mutateAsync;
}


export const useGetAllTaskTakersForWorker = (projectId: number, workerId: number, enabled: boolean = true) => {
  const query = useQuery({
    queryKey: [projectId, "allTaskTakersOfWorker", workerId],
    enabled: enabled,
    queryFn: () => {
      return getAllTaskTakersQuery(projectId, workerId)
    }
  })

  return {
    ...query
  }
}


export const useAddTaskExecutor = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ taskId, executor }: { taskId: number; executor: ITaskExecutorForm }) =>
      addTaskExecutor(taskId, executor),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries({ queryKey: ["task", taskId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "tasks"] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "tasks"] });
      queryClient.invalidateQueries({ queryKey: ["board"], exact: false });
      handleSuccess?.();
      notification.success({ message: "Исполнитель добавлен" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при добавлении исполнителя" });
    },
  });
};

export const useRemoveTaskExecutor = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ taskId, workerId }: { taskId: number; workerId: number }) =>
      removeTaskExecutor(taskId, workerId),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries({ queryKey: ["task", taskId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "tasks"] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "tasks"] });
      queryClient.invalidateQueries({ queryKey: ["board"], exact: false });
      handleSuccess?.();
      notification.success({ message: "Исполнитель удален" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при удалении исполнителя" });
    },
  });
};

export const useUpdateTaskExecutorResponsible = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ taskId, workerId }: { taskId: number; workerId: number | null }) =>
      updateTaskExecutorResponsible(taskId, workerId),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries({ queryKey: ["task", taskId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.PROJECT, "tasks"] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "tasks"] });
      queryClient.invalidateQueries({ queryKey: ["board"], exact: false });
      handleSuccess?.();
      notification.success({ message: "Ответственный исполнитель обновлен" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при изменении ответственного" });
    },
  });
};


export const useAddTaskObserver = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ taskId, observer }: { taskId: number; observer: ITaskObserverForm }) =>
      addTaskObserver(taskId, observer),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries({ queryKey: ["task", taskId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "tasks"] });
      queryClient.invalidateQueries({ queryKey: ["board"], exact: false });
      handleSuccess?.();
      notification.success({ message: "Наблюдатель добавлен" });
    },
    onError: (err: Error) => {
      notification.error({ message: err.message });
    },
  });
};

export const useRemoveTaskObserver = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ taskId, workerId }: { taskId: number; workerId: number }) =>
      removeTaskObserver(taskId, workerId),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries({ queryKey: ["task", taskId] });
      queryClient.invalidateQueries({ queryKey: [PageOwnerEnum.WORKER, "tasks"] });
      queryClient.invalidateQueries({ queryKey: ["board"], exact: false });
      handleSuccess?.();
      notification.success({ message: "Наблюдатель удален" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при удалении наблюдателя" });
    },
  });
};

export const useGetTaskTemplates = (enabled: boolean = true) => {
  const [filters, setFilters] = useState<ITaskTemplateFilterOptions>({});
  const query = useQuery({
    queryKey: ["taskTemplates", filters],
    enabled: enabled,
    queryFn: () => {
      return getTaskTemplatesQuery(filters)
    }
  })

  return {
    ...query,
    filters,
    setFilters
  }
}


export const useGetTaskTemplate = (templateId: number, enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["taskTemplate",],
    enabled: enabled,
    queryFn: () => {
      return getTaskTemplateQuery(templateId)
    }
  })

  return {
    ...query,
  }
}


export const useDeleteTaskTemplate = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({ templateId }: { templateId: number }) => {
      return removeTaskTemplateQuery(templateId);
    },
    onSuccess: (isSuccess) => {
      if (isSuccess) {
        queryClient.invalidateQueries({ queryKey: ["taskTemplates"] });
        queryClient.invalidateQueries({ queryKey: ["taskTemplate"] });
        handleSuccess?.();
        notification.success({ message: "Шаблон удален" });
      } else {
        notification.error({ message: "Ошибка при удалении шаблона" });
      }
    },
    onError: () => {
      notification.error({ message: "Ошибка при удалении шаблона" });
    },
  });
  return mutation.mutateAsync;
};

export const useAddTaskTemplate = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({ options }: { options: ITaskTemplateOptions }) => {
      return addTaskTemplateQuery(options);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["taskTemplates"] });
      handleSuccess?.();
      notification.success({ message: "Шаблон добавлен" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при добавлении шаблона" });
    },
  });
  return mutation.mutateAsync;
};

export const useEditTaskTemplate = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: ({ templateId, options }: { templateId: number; options: ITaskTemplateOptions }) => {
      return editTaskTemplateQuery(templateId, options);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["taskTemplates"] });
      queryClient.invalidateQueries({ queryKey: ["taskTemplate"] });
      handleSuccess?.();
      notification.success({ message: "Шаблон изменен" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при изменении шаблона" });
    },
  });
  return mutation.mutateAsync;
};


export const useGetTaskFilters = (enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["taskFilters"],
    enabled: enabled,
    queryFn: () => getTaskFilters(),
  });

  return query;
};

export const useAddTaskFilter = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: ({
      filterName,
      options,
    }: {
      filterName: string;
      options: ITaskFilterOptions;
    }) => {
      return addTaskFilterTo(filterName, options);
    },
    onSuccess: (_) => {
      queryClient.invalidateQueries({ queryKey: ["taskFilters"] });
      handleSuccess?.();
      notification.success({ message: "Фильтр добавлен" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при добавлении фильтра" });
    },
  });

  return mutation.mutateAsync;
};

export const useEditTaskFilter = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: ({
      filterName,
      options,
    }: {
      filterName: string;
      options: ITaskFilterOptions;
    }) => {
      return editTaskFilter(filterName, options);
    },
    onSuccess: (_) => {
      queryClient.invalidateQueries({ queryKey: ["taskFilters"] });
      handleSuccess?.();
      notification.success({ message: "Фильтр изменён" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при изменении фильтра" });
    },
  });

  return mutation.mutateAsync;
};

export const useDeleteTaskFilter = (handleSuccess?: () => void) => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: ({
      filterName,
    }: {
      filterName: string;
    }) => {
      return removeTaskFilter(filterName);
    },
    onSuccess: (_) => {
      queryClient.invalidateQueries({ queryKey: ["taskFilters"] });
      handleSuccess?.();
      notification.success({ message: "Фильтр удалён" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при удалении фильтра" });
    },
  });

  return mutation.mutateAsync;
};






export const useUpdateTaskResponsibleWorker = (
  handleSuccess?: () => void
) => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: async ({
      taskId,
      workerId,
    }: {
      taskId: number;
      workerId: number | null;
    }) => {
      return await updateTaskResponsibleWorker(taskId, workerId);
    },
    onSuccess: (_, data) => {

      // Invalidate relevant queries
      queryClient.invalidateQueries({ queryKey: ["task", data.taskId] });
      queryClient.invalidateQueries({
        queryKey: [PageOwnerEnum.WORKER, "tasks", data.taskId],
      });
      queryClient.invalidateQueries({
        queryKey: [PageOwnerEnum.PROJECT],
      });
      queryClient.invalidateQueries({
        queryKey: [PageOwnerEnum.WORKER],
      });
      queryClient.invalidateQueries({
        queryKey: ["board"],
        exact: false,
      });

      handleSuccess?.();
      notification.success({ message: "Ответственный исполнитель обновлен" });
    },
    onError: () => {
      notification.error({ message: "Ошибка при изменении ответственного" });
    },
  });

  return mutation.mutateAsync;
};