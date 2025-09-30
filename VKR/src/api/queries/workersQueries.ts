import { workerStatuses } from './../../sync/workerStatuses';
import { filterBoards, filterProjects, filterSprints } from './../dummyData/filters';
import { IAddWorkerOptions } from "../options/createOptions/IAddWorkerOptions";
import { IWorker } from "../../interfaces/IWorker";
import { IWorkerFilterOptions } from "../options/filterOptions/IWorkerFilterOptions";
import { IEditWorkerOptions } from "../options/editOptions/IEditWorkerOptions";
import { IFile } from "../../interfaces/IFile";
import { IBoard } from "../../interfaces/IBoard";
import { filterFiles, filterHistory, filterTasks } from "../dummyData/filters";
import { IAddSubordinateOptions } from "../options/createOptions/IAddSubordinatesOptions";
import { ITaskFilterOptions } from "../options/filterOptions/ITaskFilterOptions";
import { ITask } from "../../interfaces/ITask";
import { TaskWorkerTypeEnum } from "../../enums/TaskWorkerTypeEnum";
import { IProjectFilterOptions } from "../options/filterOptions/IProjectFilterOptions";
import { IProject } from "../../interfaces/IProject";
import { IFileFilterOptions } from "../options/filterOptions/IFileFilterOptions";
import { IAddFileOptions } from "../options/createOptions/IAddFileOptions";
import { IHistoryFilterOptions } from "../options/filterOptions/IHistoryFilterOptions";
import { IHistoryItem } from "../../interfaces/IHistoryItem";
import { IAddBoardOptions } from "../options/createOptions/IAddBoardOptions";
import { ISprint } from "../../interfaces/ISprint";


import dayjs from "dayjs";
import { ISprintFilterOptions } from "../options/filterOptions/ISprintFilterOptions";
import { IBoardFilterOptopns } from "../options/filterOptions/IBoardFilterOptions";
import { FileOwnerEnum } from '../../enums/ownerEntities/FileOwnerEnum';
import { HistoryOwnerEnum } from '../../enums/ownerEntities/HistoryOwnerEnum';
import { IWorkerFields } from '../../interfaces/IWorkerFields';
import { ApiWorkerAnalyticsResponse, WorkerAnalyticsData } from '../../interfaces/analytics';
import { taskStatuses } from '../../sync/taskStatuses';
import { taskPriorities } from '../../sync/taskPriorities';
import { apiClient } from '../../config/axiosConfig';
import { getProjectsQuery } from './projectsQueries';
import { projectStatuses } from '../../sync/projectStatuses';
import { ITaskWorker } from '../../interfaces/ITaskWorker';
import { sprintStatuses } from '../../sync/sprintStatuses';
import { mapBackendToBasis, mapBoardBasisToBackend } from './boardsQueries';
import { BoardOwnerEnum } from '../../enums/ownerEntities/BoardOwnerEnum';

// get all organization workers
export const getWorkersQuery = async (
  filters: IWorkerFilterOptions,
): Promise<IWorker[]> => {

  const params = new URLSearchParams();

  // Pagination parameters

  // Filter parameters
  if (filters.firstName) params.append("Name", filters.firstName);
  if (filters.secondName) params.append("SecondName", filters.secondName);
  if (filters.thirdName) params.append("ThirdName", filters.thirdName);
  if (filters.email) params.append("Email", filters.email);
  if (filters.isAdmin !== undefined) params.append("CanManageWorkers", filters.isAdmin.toString());
  if (filters.isManager !== undefined) params.append("CanManageProjects", filters.isManager.toString());

  const url = `/workers?${params.toString()}`;
  const response = await apiClient.get(url);

  return response.data.map(
    (item: any) => ({
      id: item.Id,
      firstName: item.Name,
      secondName: item.SecondName,
      thirdName: item.ThirdName,
      email: item.Email,
      createdAt: dayjs(item.CreatedOn),
      isAdmin: item.CanManageWorkers,
      isManager: item.CanManageProjects,
      status: {
        id: item.WorkerStatus.id,
        name: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.name ?? "Не указан",
        color: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.color ?? "blue"
      },
      workerPosition: {
        id: item.WorkerPosition.Id,
        title: item.WorkerPosition.Title,
        canAssignTasksTo: [],
        canTakeTasksFrom: []
      }
    }) as IWorker
  )
};

// get org worker
export const getWorkerQuery = async (workerId: number): Promise<IWorker> => {
  const response = await apiClient.get(`/workers/${workerId}`);
  const item: any = response.data;
  return ({
    id: item.Id,
    firstName: item.Name,
    secondName: item.SecondName,
    thirdName: item.ThirdName,
    email: item.Email,
    createdAt: dayjs(item.CreatedOn),
    isAdmin: item.CanManageWorkers,
    isManager: item.CanManageProjects,
    status: {
      id: item.WorkerStatus.id,
      name: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.name ?? "Не указан",
      color: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.color ?? "blue"
    },
    workerPosition: {
      id: item.WorkerPosition.Id,
      title: item.WorkerPosition.Title,
      canAssignTasksTo: [],
      canTakeTasksFrom: []
    }
  }) as IWorker

};

//return all workers who can manage projects
export const getManagersQuery = async (): Promise<IWorker[]> => {
  const params = new URLSearchParams();

  params.append("CanManageProjects", true.toString())

  const url = `/workers?${params.toString()}`;
  const response = await apiClient.get(url);
  return response.data.map(
    (item: any) => ({
      id: item.Id,
      firstName: item.Name,
      secondName: item.SecondName,
      thirdName: item.ThirdName,
      email: item.Email,
      createdAt: dayjs(item.CreatedOn),
      isAdmin: item.CanManageWorkers,
      isManager: item.CanManageProjects,
      status: {
        id: item.WorkerStatus.id,
        name: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.name ?? "Не указан",
        color: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.color ?? "blue"
      },
      workerPosition: {
        id: item.WorkerPosition.Id,
        title: item.WorkerPosition.Title,
        canAssignTasksTo: [],
        canTakeTasksFrom: []
      }
    }) as IWorker
  )
};

// add organization worker
export const addWorkerQuery = async (
  data: IAddWorkerOptions
): Promise<IWorker> => {
  const response = await apiClient.post('/workers', {
    name: data.firstName,
    secondName: data.secondName,
    thirdName: data.thirdName,
    email: data.email,
    password: data.password,
    canManageProjects: data.isManager,
    canManageWorkers: data.isAdmin,
    workerStatus: data.status.id,
    workerPosition: data.workerPosition.id
  });
  const item = response.data;
  return ({
    id: item.Id,
    firstName: item.Name,
    secondName: item.SecondName,
    thirdName: item.ThirdName,
    email: item.Email,
    createdAt: dayjs(item.CreatedOn),
    isAdmin: item.CanManageWorkers,
    isManager: item.CanManageProjects,
    status: {
      id: item.WorkerStatus.id,
      name: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.name ?? "Не указан",
      color: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.color ?? "blue"
    },
    workerPosition: {
      id: item.WorkerPosition.Id,
      title: item.WorkerPosition.Title,
      canAssignTasksTo: [],
      canTakeTasksFrom: []
    }
  }) as IWorker
};

// remove organization worker
export const removeWorkerQuery = async (workerId: number): Promise<boolean> => {
  const response = await apiClient.delete(`/workers/${workerId}`);
  console.log(response.status)
  return response.status === 204;
};

// edit worker data
export const editWorkerDataQuery = async (
  workerId: number,
  data: IEditWorkerOptions
): Promise<IWorker> => {
  const response = await apiClient.put(`/workers/${workerId}`, {
    id: workerId,
    name: data.firstName,
    secondName: data.secondName,
    thirdName: data.thirdName,
    email: data.email,
    password: data.password,
    canManageProjects: data.isManager,
    canManageWorkers: data.isAdmin,
    workerStatus: data.statusId,
    workerPosition: data.workerPositionId
  });

  const item = response.data;

  return ({
    id: item.Id,
    firstName: item.Name,
    secondName: item.SecondName,
    thirdName: item.ThirdName,
    email: item.Email,
    createdAt: dayjs(item.CreatedOn),
    isAdmin: item.CanManageWorkers,
    isManager: item.CanManageProjects,
    status: {
      id: item.WorkerStatus.id,
      name: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.name ?? "Не указан",
      color: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.color ?? "blue"
    },
    workerPosition: {
      id: item.WorkerPosition.Id,
      title: item.WorkerPosition.Title,
      canAssignTasksTo: [],
      canTakeTasksFrom: []
    }
  }) as IWorker
};


// get worker subordinates
export const getWorkerSubordinatesQuery = async (
  workerId: number,
  filters: IWorkerFilterOptions
): Promise<IWorker[]> => {

  const params = new URLSearchParams();

  // Pagination parameters

  // Filter parameters
  if (filters.firstName) params.append("Name", filters.firstName);
  if (filters.secondName) params.append("SecondName", filters.secondName);
  if (filters.thirdName) params.append("ThirdName", filters.thirdName);
  if (filters.email) params.append("Email", filters.email);
  if (filters.isAdmin !== undefined) params.append("CanManageWorkers", filters.isAdmin.toString());
  if (filters.isManager !== undefined) params.append("CanManageProjects", filters.isManager.toString());

  const url = `/workers-management/my-employees/${workerId}?${params.toString()}`;
  const response = await apiClient.get(url);

  return response.data.map(
    (item: any) => ({
      id: item.Id,
      firstName: item.Name,
      secondName: item.SecondName,
      thirdName: item.ThirdName,
      email: item.Email,
      createdAt: dayjs(item.CreatedOn),
      isAdmin: item.CanManageWorkers,
      isManager: item.CanManageProjects,
      status: {
        id: item.WorkerStatus.id,
        name: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.name ?? "Не указан",
        color: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.color ?? "blue"
      },
      workerPosition: {
        id: item.WorkerPosition.Id,
        title: item.WorkerPosition.Title,
        canAssignTasksTo: [],
        canTakeTasksFrom: []
      }
    }) as IWorker
  )


}

// get worker directors
export const getWorkerDirectorsQuery = async (
  workerId: number,
  filters: IWorkerFilterOptions
): Promise<IWorker[]> => {
  const params = new URLSearchParams();

  // Pagination parameters

  // Filter parameters
  if (filters.firstName) params.append("Name", filters.firstName);
  if (filters.secondName) params.append("SecondName", filters.secondName);
  if (filters.thirdName) params.append("ThirdName", filters.thirdName);
  if (filters.email) params.append("Email", filters.email);
  if (filters.isAdmin !== undefined) params.append("CanManageWorkers", filters.isAdmin.toString());
  if (filters.isManager !== undefined) params.append("CanManageProjects", filters.isManager.toString());

  const url = `/workers-management/my-managers/${workerId}?${params.toString()}`;
  const response = await apiClient.get(url);

  return response.data.map(
    (item: any) => ({
      id: item.Id,
      firstName: item.Name,
      secondName: item.SecondName,
      thirdName: item.ThirdName,
      email: item.Email,
      createdAt: dayjs(item.CreatedOn),
      isAdmin: item.CanManageWorkers,
      isManager: item.CanManageProjects,
      status: {
        id: item.WorkerStatus.id,
        name: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.name ?? "Не указан",
        color: workerStatuses.find(status => status.id === item.WorkerStatus.id)?.color ?? "blue"
      },
      workerPosition: {
        id: item.WorkerPosition.Id,
        title: item.WorkerPosition.Title,
        canAssignTasksTo: [],
        canTakeTasksFrom: []
      }
    }) as IWorker
  )
}


// add single worker subordinate
export const addSubordinateToWorkerQuery = async (
  workerId: number,
  data: IAddSubordinateOptions
): Promise<boolean> => {
  const response = await apiClient.post(`/workers-management`, {
    managerId: workerId,
    subordinateId: data.workerId
  });
  console.log("we here?")

  return response.status === 200
};

// delete worker subordinate
export const removeSubordinateFromWorkerQuery = async (
  workerId: number,
  subordinateId: number,
): Promise<boolean> => {
  const params = new URLSearchParams();
  params.append("managerId", workerId.toString());
  params.append("subordinateId", subordinateId.toString());
  const response = await apiClient.delete(`/workers-management?${params.toString()}`);

  return response.status === 204
};


export const getAvailableWorkerSubordinates = async (
  workerId: number,
  filters: IWorkerFilterOptions
): Promise<IWorker[]> => {
  const subordinates: IWorker[] = await getWorkerSubordinatesQuery(workerId, filters);
  const allWorkers: IWorker[] = await getWorkersQuery(filters);

  const subordinateIds = new Set(subordinates.map((subordinate) => subordinate.id));

  const availableWorkers = allWorkers.filter(
    (worker) => !subordinateIds.has(worker.id) && worker.id !== workerId
  );

  return availableWorkers;

}



export const getWorkerTasksWhereExecutorQuery = async (
  workerId: number,
  filters: ITaskFilterOptions
): Promise<ITask[]> => {
  const response = await apiClient.get(`/tasks`);

  const tasks: ITask[] = response.data.map((item: any) => {

    // Combine Executors and Observers into workers
    const workers: ITaskWorker[] = [
      ...item.Executors.map((executor: any) => ({
        workerData: {
          id: executor.Id,
          name: executor.Name,
          email: executor.Email,
          secondName: executor.SecondName,
          thirdName: "",
        },
        isResponsible: item.ResponsibleWorker && item.ResponsibleWorker.Id === executor.Id,
        taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
      })),
      ...item.Observers.map((observer: any) => ({
        workerData: {
          id: observer.Id,
          name: observer.Name,
          email: observer.Email,
          secondName: observer.SecondName,
          thirdName: "",
        },
        isResponsible: false,
        taskWorkerType: TaskWorkerTypeEnum.VIEWER,
      })),
    ];

    // Set the first executor as responsible, if any
    if (workers.length > 0 && workers[0].taskWorkerType === TaskWorkerTypeEnum.EXECUTOR) {
      workers[0].isResponsible = true;
    }

    return {
      id: item.Id,
      name: item.ShortName,
      description: item.Description || "",
      type: item.TaskType.Name,
      progress: item.Progress,
      project: {
        id: item.Project.Id,
        name: item.Project.Name,
        managerId: item.Project.ManagerId,
      },
      createdAt: dayjs(item.CreatedOn),
      startDate: item.StartOn ? dayjs(item.StartOn) : undefined,
      endDate: item.ExpireOn ? dayjs(item.ExpireOn) : undefined,
      status: {
        id: item.TaskStatus.Id,
        name: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.name ?? "Не определен",
        color: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.color ?? "blue",
      },
      priority: {
        id: item.TaskPriority.Id,
        name: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.name ?? "Не определен",
        color: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.color ?? "blue",
      },
      creator: {
        id: item.Creator.Id,
        name: item.Creator.Name,
        email: item.Creator.Email,
        secondName: item.Creator.SecondName,
        thirdName: "",
      },
      tags: item.TagDTOs.map((tag: any) => ({
        id: tag.Id,
        name: tag.Name,
        color: tag.Color,
      })),
      workers,
      sprint: item.Sprint ? {
        id: item.Sprint.Id,
        name: item.Sprint.Title,
        projectId: item.Sprint.ProjectId,
        status: sprintStatuses.find(status => status.id === item.Sprint.SprintStatusId)
      } as ISprint : undefined,
      storyPoints: item.StoryPoints, // Not in response, assume undefined
      checklists: item.Checklists || [], // Not in response, assume empty
      relationships: item.RelatedTasks.map((rel: any) => ({
        taskId: item.Id,
        relatedTaskId: rel.Task.Id,
        relationType: rel.RelationshipType,
        lag: undefined, // Not provided in response
      })),
    };
  });

  return filterTasks(tasks, filters, 1, undefined).filter((task) =>
    task.workers.some(
      (worker) =>
        worker.workerData.id === workerId &&
        worker.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR
    )
  );
};

export const getWorkerTasksWhereResponsibleQuery = async (
  workerId: number,
  filters: ITaskFilterOptions,
  page: number,
  limit?: number
): Promise<ITask[]> => {
  const response = await apiClient.get(`/tasks`);

  const tasks: ITask[] = response.data.map((item: any) => {


    // Combine Executors and Observers into workers
    const workers: ITaskWorker[] = [
      ...item.Executors.map((executor: any) => ({
        workerData: {
          id: executor.Id,
          name: executor.Name,
          email: executor.Email,
          secondName: executor.SecondName,
          thirdName: "",
        },
        isResponsible: item.ResponsibleWorker && item.ResponsibleWorker.Id === executor.Id,
        taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
      })),
      ...item.Observers.map((observer: any) => ({
        workerData: {
          id: observer.Id,
          name: observer.Name,
          email: observer.Email,
          secondName: observer.SecondName,
          thirdName: "",
        },
        isResponsible: false,
        taskWorkerType: TaskWorkerTypeEnum.VIEWER,
      })),
    ];


    return {
      id: item.Id,
      name: item.ShortName,
      description: item.Description || "",
      type: item.TaskType.Name,
      progress: item.Progress,
      project: {
        id: item.Project.Id,
        name: item.Project.Name,
        managerId: item.Project.ManagerId,
      },
      createdAt: dayjs(item.CreatedOn),
      startDate: item.StartOn ? dayjs(item.StartOn) : undefined,
      endDate: item.ExpireOn ? dayjs(item.ExpireOn) : undefined,
      status: {
        id: item.TaskStatus.Id,
        name: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.name ?? "Не определен",
        color: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.color ?? "blue",
      },
      priority: {
        id: item.TaskPriority.Id,
        name: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.name ?? "Не определен",
        color: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.color ?? "blue",
      },
      creator: {
        id: item.Creator.Id,
        name: item.Creator.Name,
        email: item.Creator.Email,
        secondName: item.Creator.SecondName,
        thirdName: "",
      },
      tags: item.TagDTOs.map((tag: any) => ({
        id: tag.Id,
        name: tag.Name,
        color: tag.Color,
      })),
      workers,
      sprint: item.Sprint ? {
        id: item.Sprint.Id,
        name: item.Sprint.Title,
        projectId: item.Sprint.ProjectId,
        status: sprintStatuses.find(status => status.id === item.Sprint.SprintStatusId)
      } as ISprint : undefined,
      storyPoints: item.StoryPoints, // Not in response, assume undefined
      checklists: item.Checklists || [], // Not in response, assume empty
      relationships: item.RelatedTasks.map((rel: any) => ({
        taskId: item.Id,
        relatedTaskId: rel.Task.Id,
        relationType: rel.RelationshipType,
        lag: undefined, // Not provided in response
      })),
    };
  });

  return filterTasks(tasks, filters, page, limit).filter((task) =>
    task.workers.some(
      (worker) =>
        worker.workerData.id === workerId &&
        worker.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR &&
        worker.isResponsible
    )
  );
};

export const getWorkerTasksWhereViewerQuery = async (
  workerId: number,
  filters: ITaskFilterOptions,
  page: number,
  limit?: number
): Promise<ITask[]> => {
  const response = await apiClient.get(`/tasks`);

  const tasks: ITask[] = response.data.map((item: any) => {


    // Combine Executors and Observers into workers
    const workers: ITaskWorker[] = [
      ...item.Executors.map((executor: any) => ({
        workerData: {
          id: executor.Id,
          name: executor.Name,
          email: executor.Email,
          secondName: executor.SecondName,
          thirdName: "",
        },
        isResponsible: item.ResponsibleWorker && item.ResponsibleWorker.Id === executor.Id,
        taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
      })),
      ...item.Observers.map((observer: any) => ({
        workerData: {
          id: observer.Id,
          name: observer.Name,
          email: observer.Email,
          secondName: observer.SecondName,
          thirdName: "",
        },
        isResponsible: false,
        taskWorkerType: TaskWorkerTypeEnum.VIEWER,
      })),
    ];

    return {
      id: item.Id,
      name: item.ShortName,
      description: item.Description || "",
      type: item.TaskType.Name,
      progress: item.Progress,
      project: {
        id: item.Project.Id,
        name: item.Project.Name,
        managerId: item.Project.ManagerId,
      },
      createdAt: dayjs(item.CreatedOn),
      startDate: item.StartOn ? dayjs(item.StartOn) : undefined,
      endDate: item.ExpireOn ? dayjs(item.ExpireOn) : undefined,
      status: {
        id: item.TaskStatus.Id,
        name: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.name ?? "Не определен",
        color: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.color ?? "blue",
      },
      priority: {
        id: item.TaskPriority.Id,
        name: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.name ?? "Не определен",
        color: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.color ?? "blue",
      },
      creator: {
        id: item.Creator.Id,
        name: item.Creator.Name,
        email: item.Creator.Email,
        secondName: item.Creator.SecondName,
        thirdName: "",
      },
      tags: item.TagDTOs.map((tag: any) => ({
        id: tag.Id,
        name: tag.Name,
        color: tag.Color,
      })),
      workers,
      sprint: item.Sprint ? {
        id: item.Sprint.Id,
        name: item.Sprint.Title,
        projectId: item.Sprint.ProjectId,
        status: sprintStatuses.find(status => status.id === item.Sprint.SprintStatusId)
      } as ISprint : undefined,
      storyPoints: item.StoryPoints, // Not in response, assume undefined
      checklists: item.Checklists || [], // Not in response, assume empty
      relationships: item.RelatedTasks.map((rel: any) => ({
        taskId: item.Id,
        relatedTaskId: rel.Task.Id,
        relationType: rel.RelationshipType,
        lag: undefined, // Not provided in response
      })),
    };
  });

  return filterTasks(tasks, filters, page, limit).filter((task) =>
    task.workers.some(
      (worker) =>
        worker.workerData.id === workerId &&
        worker.taskWorkerType === TaskWorkerTypeEnum.VIEWER
    )
  );
};

export const getWorkerTasksWhereCreatorQuery = async (
  workerId: number,
  filters: ITaskFilterOptions,
  page: number,
  limit?: number
): Promise<ITask[]> => {
  const response = await apiClient.get(`/tasks`);

  const tasks: ITask[] = response.data.map((item: any) => {


    // Combine Executors and Observers into workers
    const workers: ITaskWorker[] = [
      ...item.Executors.map((executor: any) => ({
        workerData: {
          id: executor.Id,
          name: executor.Name,
          email: executor.Email,
          secondName: executor.SecondName,
          thirdName: "",
        },
        isResponsible: item.ResponsibleWorker && item.ResponsibleWorker.Id === executor.Id,
        taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
      })),
      ...item.Observers.map((observer: any) => ({
        workerData: {
          id: observer.Id,
          name: observer.Name,
          email: observer.Email,
          secondName: observer.SecondName,
          thirdName: "",
        },
        isResponsible: false,
        taskWorkerType: TaskWorkerTypeEnum.VIEWER,
      })),
    ];

    return {
      id: item.Id,
      name: item.ShortName,
      description: item.Description || "",
      type: item.TaskType.Name,
      progress: item.Progress,
      project: {
        id: item.Project.Id,
        name: item.Project.Name,
        managerId: item.Project.ManagerId,
      },
      createdAt: dayjs(item.CreatedOn),
      startDate: item.StartOn ? dayjs(item.StartOn) : undefined,
      endDate: item.ExpireOn ? dayjs(item.ExpireOn) : undefined,
      status: {
        id: item.TaskStatus.Id,
        name: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.name ?? "Не определен",
        color: taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.color ?? "blue",
      },
      priority: {
        id: item.TaskPriority.Id,
        name: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.name ?? "Не определен",
        color: taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)?.color ?? "blue",
      },
      creator: {
        id: item.Creator.Id,
        name: item.Creator.Name,
        email: item.Creator.Email,
        secondName: item.Creator.SecondName,
        thirdName: "",
      },
      tags: item.TagDTOs.map((tag: any) => ({
        id: tag.Id,
        name: tag.Name,
        color: tag.Color,
      })),
      workers,
      sprint: item.Sprint ? {
        id: item.Sprint.Id,
        name: item.Sprint.Title,
        projectId: item.Sprint.ProjectId,
        status: sprintStatuses.find(status => status.id === item.Sprint.SprintStatusId)
      } as ISprint : undefined,
      storyPoints: item.StoryPoints, // Not in response, assume undefined
      checklists: item.Checklists || [], // Not in response, assume empty
      relationships: item.RelatedTasks.map((rel: any) => ({
        taskId: item.Id,
        relatedTaskId: rel.Task.Id,
        relationType: rel.RelationshipType,
        lag: undefined, // Not provided in response
      })),
    };
  });

  return filterTasks(tasks, filters, page, limit).filter((task) => {
    const isCreator = task.creator.id === workerId;
    const isProjectMember = task.workers.some(
      (worker) => worker.workerData.id === workerId
    ) || task.project.managerId === workerId;
    return isCreator && isProjectMember;
  });
};

export const getWorkerProjectsQuery = async (
  workerId: number,
  filters: IProjectFilterOptions,
): Promise<IProject[]> => {
  const projects: IProject[] = await getProjectsQuery(filters, 1);
  return projects.filter(project => project.members.map(member => (member.workerData.id)).includes(workerId))
}


export const getManagerProjectsQuery = async (
  managerId: number,
  filters: IProjectFilterOptions,
): Promise<IProject[]> => {
  const response = await apiClient.get(`/api/projects/manager/${managerId}`)

  const items = response.data;

  const result = items.map((item: any) => ({
    id: item.Id,
    name: item.Name,
    checklists: item.ProjectChecklists || [],
    createdAt: dayjs(item.CreatedOn),
    description: item.Description || '',
    manager: {
      id: item.Manager.Id,
      firstName: item.Manager.Name,
      secondName: item.Manager.SecondName,
      thirdName: item.Manager.ThirdName || '',
      email: item.Manager.Email,
      createdAt: dayjs(item.Manager.CreatedOn),
      isAdmin: item.Manager.CanManageWorkers,
      isManager: item.Manager.CanManageProjects,
      status: {
        id: item.Manager.WorkerStatus.Id,
        name: workerStatuses.find(status => status.id === item.Manager.WorkerStatus.Id)?.name,
        color: workerStatuses.find(status => status.id === item.Manager.WorkerStatus.Id)?.color
      },
      workerPosition: {
        id: item.Manager.WorkerPosition.Id,
        title: item.Manager.WorkerPosition.Title,
        canAssignTasksTo: item.Manager.WorkerPosition.TaskGivers.map((pos: any) => pos.Id),
        canTakeTasksFrom: item.Manager.WorkerPosition.TaskTakers.map((pos: any) => pos.Id)
      }
    },
    members: item.Workers.map((member: any) => ({
      workerData: {
        id: member.Id,
        firstName: member.Name,
        secondName: member.SecondName,
        thirdName: member.ThirdName || '',
        email: member.Email,
        createdAt: dayjs(member.CreatedOn),
        isAdmin: false, // Отсутствует CanManageWorkers
        isManager: false, // Отсутствует CanManageProjects
        status: {
          id: 0, // Отсутствует WorkerStatus
          name: 'Unknown',
          color: 'blue'
        },
        workerPosition: {
          id: member.WorkerPosition.Id,
          title: member.WorkerPosition.Title,
          canAssignTasksTo: member.WorkerPosition.TaskGivers.map((pos: any) => pos.Id),
          canTakeTasksFrom: member.WorkerPosition.TaskTakers.map((pos: any) => pos.Id)
        }
      },
      taskTakers: [], // Заполнить, если есть данные о подчиненных
      createdAt: dayjs(member.CreatedOn) // Используем CreatedOn работника, так как нет даты присоединения к проекту
    })),
    progress: item.Progress,
    status: {
      id: item.ProjectStatusId,
      name: item.ProjectStatusName || projectStatuses.find(status => status.name === item.ProjectStatusName)?.name || 'Unknown',
      color: projectStatuses.find(status => status.name === item.ProjectStatusName)?.color
    },
    startDate: item.StartDate ? dayjs(item.StartDate) : undefined,
    endDate: item.EndDate ? dayjs(item.EndDate) : undefined,
    tags: item.ProjectTags || [],
    goal: item.Goal || ''
  }));
  return filterProjects(result, filters, 1)
}



// get all worker files
export const getWorkerFilesQuery = async (
  workerId: number,
  filters: IFileFilterOptions,
  page: number,
  limit?: number
): Promise<IFile[]> => {
  const response = await apiClient.get(`/${FileOwnerEnum.WORKER}/${workerId}/files`);
  const items = response.data.map((item: any) => ({
    id: item.Id,
    createdAt: dayjs(item.CreatedAt),
    creator: {
      id: item.Creator.Id,
      email: item.Creator.Email,
      firstName: item.Creator.Name,
      secondName: item.Creator.SecondName,
      thirdName: item.Creator.ThirdName
    },
    description: item.Description,
    name: item.Name,
    size: item.FileSize,
    owner: {
      id: workerId,
      name: "worker",
      type: FileOwnerEnum.WORKER
    }
  }) as IFile)

  return filterFiles(items, filters, page, limit);
};

export const getAvailalbeWorkerFilesResponsibleWorkers = async (
): Promise<IWorkerFields[]> => {
  return await getWorkersQuery({})
};

// add worker file
export const addWorkerFileQuery = async (
  workerId: number,
  data: IAddFileOptions,
): Promise<boolean> => {
  const formData = new FormData();
  formData.append('file', data.file); // Append the file
  formData.append('title', data.name); // Append the title
  if (data.description) formData.append('description', data.description);

  const response = await apiClient.post(`/${FileOwnerEnum.WORKER}/${workerId}/files`, formData, {
    headers: {
      "Content-Type": "multipart/form-data", // Явно указываем для ясности
    },
  });


  return response.status === 201
};

// delete worker file
export const removeWorkerFileQuery = async (
  workerId: number,
  fileId: number,
): Promise<boolean> => {
  const response = await apiClient.delete(
    `/${FileOwnerEnum.WORKER}/${workerId}/files/${fileId}`,
  );

  return response.status === 204;
};


// download worker file
export const downloadWorkerFileQuery = async (workerId: number, fileId: number) => {
  const response = await apiClient.get(`/${FileOwnerEnum.WORKER}/${workerId}/files/${fileId}`, {
    responseType: 'blob', // Expect binary data
  });

  if (response.status !== 200) {
    throw new Error('Failed to download file');
  }

  return response.data as Blob;
};


//get worker history
export const getWorkerHistoryQuery = async (
  workerId: number,
  filters: IHistoryFilterOptions,
  page: number,
  limit?: number
): Promise<IHistoryItem[]> => {
  const response = await apiClient.get(`/history/${HistoryOwnerEnum.WORKER}/${workerId}`)
  const items = response.data.map((item: any) => ({
    id: item.Id,
    message: item.Text,
    createdAt: dayjs(item.CreatedOn),
    owner: {
      id: item.RelatedEntityId,
      type: item.RelatedEntityType
    },
    responsibleWorker: {
      id: item.Creator.Id,
      firstName: item.Creator.Name,
      secondName: item.Creator.SecondName,
      thirdName: item.Creator.ThirdName,
      email: item.Creator.Email
    }
  }) as IHistoryItem)
  return filterHistory(items, filters, page, limit);
};

export const getAvailalbeWorkerHistoryResponsibleWorkers = async (
  workerId: number
): Promise<IWorkerFields[]> => {
  console.log(workerId);
  const workers = await getWorkersQuery({});
  return workers;
};

//remove worker history
export const removeWorkerHistoryQuery = async (
  workerId: number
): Promise<boolean> => {
  const response = await apiClient.delete(`/history/${HistoryOwnerEnum.WORKER}/${workerId}`)
  return response.status === 200;
};

//remove one worker history item
export const removeWorkerHistoryItemQuery = async (
  itemId: number
): Promise<boolean> => {
  const response = await apiClient.delete(`/history/${itemId}`)
  return response.status === 200;
};



// Get all project boards associated with a worker
export const getWorkerProjectBoardsQuery = async (
  workerId: number,
  filters: IBoardFilterOptopns,
  page: number,
  limit?: number
): Promise<IBoard[]> => {
  const response = await apiClient.get(`/boards/worker/${workerId}/projects`);
  const worker = await getWorkerQuery(workerId);
  const items = response.data.map((item: any) => ({
    id: item.Id,
    boardBasis: mapBackendToBasis(item.Basis),
    createdAt: dayjs(item.CreatedOn),
    name: item.Name,
    owner: {
      id: workerId,
      name: worker.email,
      type: BoardOwnerEnum.PROJECT
    },
  }) as IBoard)
  return filterBoards(items, filters, page, limit);

};

// Get all personal boards associated with a worker
export const getWorkerPersonalBoardsQuery = async (
  workerId: number,
  filters: IBoardFilterOptopns,
  page: number,
  limit?: number
): Promise<IBoard[]> => {
  const response = await apiClient.get(`/boards/worker/${workerId}/personal`);
  const worker = await getWorkerQuery(workerId);
  const items = response.data.map((item: any) => ({
    id: item.Id,
    boardBasis: mapBackendToBasis(item.Basis),
    createdAt: dayjs(item.CreatedOn),
    name: item.Name,
    owner: {
      id: workerId,
      name: worker.email,
      type: BoardOwnerEnum.WORKER
    },
  }) as IBoard)
  return filterBoards(items, filters, page, limit);

};

// Add a worker board
export const addWorkerBoardQuery = async (
  workerId: number,
  data: IAddBoardOptions
): Promise<IBoard> => {
  const payload = {
    name: data.name,
    description: '',
    ownerId: workerId,
    basis: mapBoardBasisToBackend(data.basis), // Map the basis enum to the backend numeric value
  };

  const response = await apiClient.post(`/boards/worker/${workerId}`, payload, {
    headers: {
      "X-Creator-Id": workerId.toString(), // Pass the creator ID in headers if required by the API
    },
  });

  return response.data;

};


export const getWorkerSprintsQuery = async (
  workerId: number,
  filters: ISprintFilterOptions
): Promise<ISprint[]> => {
  const response = await apiClient.get(`/sprints/worker/${workerId}`);

  const items: ISprint[] = response.data.map((item: any) => ({
    id: item.Id,
    name: item.Title,
    projectId: item.ProjectId,
    status: {
      id: item.SprintStatus.Id,
      name: sprintStatuses.find(status => status.id === item.SprintStatus.Id)?.name ?? "",
      color: sprintStatuses.find(status => status.id === item.SprintStatus.Id)?.color ?? "blue"
    },
    endDate: dayjs(item.ExpireOn),
    startDate: dayjs(item.StartsOn)
  }) as ISprint)
  return filterSprints(items, filters, 1);
}



export const getWorkerAnalytics = async (employeeId: number): Promise<WorkerAnalyticsData> => {
  const response = await apiClient.get(`analytics/worker/${employeeId}`);

  const data: ApiWorkerAnalyticsResponse = response.data;

  return {
    tasksByProject: data.TasksByProject.map(({ Item1, Item2, Item3, Item4, Item5 }) => ({
      projectId: Item1,
      projectName: Item2,
      count: Item3,
      statuses: Object.fromEntries(
        Item4.map(({ Status, Count }) => [Status.Name, Count])
      ),
      overdue: Item5,
    })),
    tasksBySprint: data.TasksBySprint.map(({ Item1, Item2 }) => ({
      sprintId: Item1,
      count: Item2,
    })),
    tasksByTag: Object.fromEntries(
      data.TasksByTag.map(({ Tag, Count }) => [Tag.Name, Count])
    ),
    tasksByStatus: Object.fromEntries(
      data.TasksByStatus.map(({ Status, Count }) => [Status.Name, Count])
    ),
    tasksByPriority: Object.fromEntries(
      data.TasksByPriority.map(({ Priority, Count }) => [Priority.Name, Count])
    ),
    overdueTasks: data.OverdueTasks,
  };
};



export const changeWorkerPasswordQuery = async (
  workerId: number,
  newPassword: string
): Promise<void> => {
  console.log(workerId, newPassword)
  // try {
  //   await axios.put(`/api/workers/${workerId}/password`, { password: newPassword });
  // } catch (error) {
  //   throw new Error(
  //     error.response?.data?.message || "Ошибка при смене пароля"
  //   );
  // }
};