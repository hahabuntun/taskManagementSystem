import { filterBoards, filterSprints } from './../dummyData/filters';
import { IBoard } from "../../interfaces/IBoard";
import { IFile } from "../../interfaces/IFile";
import { IHistoryItem } from "../../interfaces/IHistoryItem";
import { ILink } from "../../interfaces/ILink";
import { IProject } from "../../interfaces/IProject"
import { IProjectMember } from "../../interfaces/IProjectMember";
import { ISprint } from "../../interfaces/ISprint";
import { ITask } from "../../interfaces/ITask";
import { IWorker } from "../../interfaces/IWorker";
import { filterFiles, filterHistory, filterProjects, filterTasks } from "../dummyData/filters";
import { IAddBoardOptions } from "../options/createOptions/IAddBoardOptions";
import { IAddFileOptions } from "../options/createOptions/IAddFileOptions";
import { IAddLinkOptions } from "../options/createOptions/IAddLinkOptions";
import { IAddProjectOptions } from "../options/createOptions/IAddProjectOptions";
import { IAddSprintOptions } from "../options/createOptions/IAddSprintOptions";
import { IAddTaskOptions } from "../options/createOptions/IAddTaskOptions";
import { IEditLinkOptions } from "../options/editOptions/IEditLinkOptions";
import { IEditProjectOptions } from "../options/editOptions/IEditProjectOptions";
import { IBoardFilterOptopns } from "../options/filterOptions/IBoardFilterOptions";
import { IFileFilterOptions } from "../options/filterOptions/IFileFilterOptions";
import { IHistoryFilterOptions } from "../options/filterOptions/IHistoryFilterOptions";
import { IProjectFilterOptions } from "../options/filterOptions/IProjectFilterOptions";
import { ISprintFilterOptions } from "../options/filterOptions/ISprintFilterOptions";
import { ITaskFilterOptions } from "../options/filterOptions/ITaskFilterOptions";


import dayjs from "dayjs";
import { BoardOwnerEnum } from '../../enums/ownerEntities/BoardOwnerEnum';
import { FileOwnerEnum } from '../../enums/ownerEntities/FileOwnerEnum';
import { LinkOwnerEnum } from '../../enums/ownerEntities/LinkOwnerEnum';
import { HistoryOwnerEnum } from '../../enums/ownerEntities/HistoryOwnerEnum';
import { IWorkerFields } from '../../interfaces/IWorkerFields';
import { ApiProjectAnalyticsResponse, ProjectAnalyticsData, statusNameMapping } from '../../interfaces/analytics';
import { taskStatuses } from '../../sync/taskStatuses';
import { taskPriorities } from '../../sync/taskPriorities';
import { apiClient } from '../../config/axiosConfig';
import { projectStatuses } from '../../sync/projectStatuses';
import { workerStatuses } from '../../sync/workerStatuses';
import utc from 'dayjs/plugin/utc';
import { TaskWorkerTypeEnum } from '../../enums/TaskWorkerTypeEnum';
import { TaskTypeEnum } from '../../enums/TaskTypeEnum';
import { ITaskWorker } from '../../interfaces/ITaskWorker';
import { sprintStatuses } from '../../sync/sprintStatuses';
import { getWorkersQuery } from './workersQueries';
import { mapBackendToBasis, mapBoardBasisToBackend } from './boardsQueries';
import { ISprintStatus } from '../../interfaces/ISprintStatus';
dayjs.extend(utc);

// get projects
export const getProjectsQuery = async (
  filters: IProjectFilterOptions,
  page: number,
  limit?: number
): Promise<IProject[]> => {
  // Добавляем параметры пагинации и фильтры в запрос
  const params = new URLSearchParams();
  params.append('page', page.toString());
  if (limit) params.append('limit', limit.toString());
  // Пример добавления фильтров (зависит от API)

  const response = await apiClient.get(`projects?${params.toString()}`);
  const items = response.data;


  const res = items.map((item: any) => ({
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
        isAdmin: member.CanManageWorkers, // Отсутствует CanManageWorkers
        isManager: member.CanManageProjects, // Отсутствует CanManageProjects
        status: {
          id: member.WorkerStatus.Id,
          name: workerStatuses.find(status => status.id === member.WorkerStatus.Id)?.name,
          color: workerStatuses.find(status => status.id === member.WorkerStatus.Id)?.color
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
    tags: item.Tags.map((tag: any) => ({ id: tag.Id, name: tag.Name, color: tag.Color })) || [],
    goal: item.Goal || ''
  }));

  return filterProjects(res, filters, 1);
};

// get project
export const getProjectQuery = async (
  projectId: number
): Promise<IProject> => {
  try {
    const response = await apiClient.get(`projects/${projectId}`);
    const item = response.data;

    if (!item) {
      throw new Error(`Project with ID ${projectId} not found`);
    }

    return {
      id: item.Id,
      name: item.Name || '',
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
        isAdmin: item.Manager.CanManageWorkers ?? false,
        isManager: item.Manager.CanManageProjects ?? false,
        status: {
          id: item.Manager.WorkerStatus?.Id ?? 0,
          name: item.Manager.WorkerStatus?.Name ?? 'Unknown',
          color: workerStatuses.find(status => status.id === (item.Manager.WorkerStatus?.Id ?? 0))?.color ?? 'blue'
        },
        workerPosition: {
          id: item.Manager.WorkerPosition.Id,
          title: item.Manager.WorkerPosition.Title,
          canAssignTasksTo: item.Manager.WorkerPosition.TaskGivers?.map((pos: any) => pos.Id) ?? [],
          canTakeTasksFrom: item.Manager.WorkerPosition.TaskTakers?.map((pos: any) => pos.Id) ?? []
        }
      },
      members: item.Workers?.map((member: any) => ({
        workerData: {
          id: member.Id,
          firstName: member.Name,
          secondName: member.SecondName,
          thirdName: member.ThirdName || '',
          email: member.Email,
          createdAt: dayjs(member.CreatedOn),
          isAdmin: member.CanManageWorkers ?? false,
          isManager: member.CanManageProjects ?? false,
          status: {
            id: member.WorkerStatus.Id,
            name: workerStatuses.find(status => status.id === member.WorkerStatus.Id)?.name,
            color: workerStatuses.find(status => status.id === member.WorkerStatus.Id)?.color
          },
          workerPosition: {
            id: member.WorkerPosition.Id,
            title: member.WorkerPosition.Title,
            canAssignTasksTo: member.WorkerPosition.TaskGivers?.map((pos: any) => pos.Id) ?? [],
            canTakeTasksFrom: member.WorkerPosition.TaskTakers?.map((pos: any) => pos.Id) ?? []
          }
        },
        taskTakers: [],
        createdAt: dayjs(member.CreatedOn)
      })) ?? [],
      progress: item.Progress || 0,
      status: {
        id: item.ProjectStatusId ?? 0,
        name: item.ProjectStatusName || projectStatuses.find(status => status.id === (item.ProjectStatusId ?? 0))?.name || 'Unknown',
        color: projectStatuses.find(status => status.id === (item.ProjectStatusId ?? 0))?.color || 'blue'
      },
      startDate: item.StartDate ? dayjs(item.StartDate) : undefined,
      endDate: item.EndDate ? dayjs(item.EndDate) : undefined,
      tags: item.Tags.map((tag: any) => ({ id: tag.Id, name: tag.Name, color: tag.Color })) || [],
      goal: item.Goal || ''
    };
  } catch (error) {
    throw error; // Или вернуть заглушку, если требуется
  }
};

export const addProjectQuery = async (
  managerId: number,
  data: IAddProjectOptions
): Promise<boolean> => {
  try {
    const response = await apiClient.post(`projects`, {
      name: data.name,
      startDate: data.startDate ? data.startDate.utc().toISOString() : null, // Convert to UTC ISO string
      endDate: data.endDate ? data.endDate.utc().toISOString() : null, // Convert to UTC ISO string
      managerId: managerId,
      organizationId: 1, // Consider making dynamic
      projectStatusId: data.statusId,
      members: [],
      description: data.description || '',
      goal: data.goal || '',
      progress: data.progress || 0,
    });

    return response.status === 200;
  } catch (error) {
    return false;
  }
};

// remove project
export const removeProjectQuery = async (
  projectId: number,
): Promise<boolean> => {
  try {
    const response = await apiClient.delete(`projects/${projectId}`);
    return response.status === 200;
  } catch (error) {
    return false;
  }
};

export const editProjectDataQuery = async (
  projectId: number,
  data: IEditProjectOptions
): Promise<boolean> => {
  try {
    const response = await apiClient.put(`projects/${projectId}`, {
      name: data.name,
      goal: data.goal,
      progress: data.progress,
      description: data.description,
      projectStatusId: data.statusId,
      startDate: data.startDate ? data.startDate.utc().toISOString() : null, // Convert to UTC ISO string
      endDate: data.endDate ? data.endDate.utc().toISOString() : null, // Convert to UTC ISO string
    });

    return response.status === 200

  } catch (error) {
    throw error;
  }
};







export const getProjectTasksQuery = async (
  projectId: number,
  filters: ITaskFilterOptions,
  page: number,
  limit?: number
): Promise<ITask[]> => {
  const response = await apiClient.get(`/tasks`); // Adjust endpoint if needed (e.g., `/projects/${projectId}/tasks`)
  const tasks: ITask[] = response.data.map((item: any) => {


    // Combine Executors and Observers into workers
    const workers: ITaskWorker[] = [
      ...item.Executors.map((executor: any) => ({
        workerData: {
          id: executor.Id,
          name: executor.Name,
          email: executor.Email,
          secondName: executor.SecondName || "",
          thirdName: "",
        },
        isResponsible: item.ResponsibleWorker && item.ResponsibleWorker.Id === executor.id, // Set based on ResponsibleWorker if needed
        taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
      })),
      ...item.Observers.map((observer: any) => ({
        workerData: {
          id: observer.Id,
          name: observer.Name,
          email: observer.Email,
          secondName: observer.SecondName || "",
          thirdName: "",
        },
        isResponsible: false,
        taskWorkerType: TaskWorkerTypeEnum.VIEWER,
      })),
    ];

    // Set ResponsibleWorker if it matches an executor
    const responsibleWorker = item.ResponsibleWorker;
    if (responsibleWorker) {
      const responsibleIndex = workers.findIndex(
        (w) => w.workerData.id === responsibleWorker.Id
      );
      if (responsibleIndex !== -1) {
        workers[responsibleIndex].isResponsible = true;
      } else {
        workers.push({
          workerData: {
            id: responsibleWorker.Id,
            firstName: responsibleWorker.Name,
            email: responsibleWorker.Email,
            secondName: responsibleWorker.SecondName || "",
            thirdName: "",
          },
          isResponsible: true,
          taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
        });
      }
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
        name:
          taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.name ??
          "Не определен",
        color:
          taskStatuses.find((status) => status.id === item.TaskStatus.Id)?.color ??
          "blue",
      },
      priority: {
        id: item.TaskPriority.Id,
        name:
          taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)
            ?.name ?? "Не определен",
        color:
          taskPriorities.find((priority) => priority.id === item.TaskPriority.Id)
            ?.color ?? "blue",
      },
      creator: {
        id: item.Creator.Id,
        name: item.Creator.Name,
        email: item.Creator.Email,
        secondName: item.Creator.SecondName || "",
        thirdName: "",
      },
      tags: item.TagDTOs.map((tag: any) => ({
        id: tag.Id,
        name: tag.Name,
        color: tag.Color,
      })),
      workers,
      sprint: item.Sprint
        ? {
          id: item.Sprint.Id,
          name: item.Sprint.Title,
          projectId: item.Sprint.ProjectId,
          status: sprintStatuses.find(
            (status) => status.id === item.Sprint.SprintStatusId
          ),
        }
        : undefined,
      storyPoints: item.StoryPoints || undefined, // Not in response, assume undefined
      checklists: item.Checklists || [], // Not in response, assume empty
      relationships: item.RelatedTasks.map((rel: any) => ({
        taskId: item.Id, // Primary task ID
        relatedTaskId: rel.Task.Id,
        relationType: rel.RelationshipType,
        lag: undefined, // Not provided in response
      })),
    };
  });


  return filterTasks(tasks, filters, page, limit).filter(
    (task) => task.project.id === projectId
  );
};

// add project task
export const addTaskToProjectQuery = async (
  projectId: number,
  data: IAddTaskOptions,
  creator: IWorker
): Promise<ITask> => {

  const response = await apiClient.post(`/tasks`, {
    name: data.name,
    progress: data.progress,
    description: data.description,
    startDate: data.startDate ? data.startDate.utc().toISOString() : null, // Convert to UTC ISO string
    endDate: data.endDate ? data.endDate.utc().toISOString() : null,
    projectId: projectId,
    creatorId: creator.id,
    taskTypeId: data.type === TaskTypeEnum.TASK ? 1 : 2,
    taskStatusId: data.status.id,
    taskPriorityId: data.priority.id,
    sprintId: data.sprint?.id,
    storyPoints: data.storyPoints
  });


  return response.data
};



// get project members
export const getProjectMembersQuery = async (
  projectId: number
): Promise<IProjectMember[]> => {
  const response = await apiClient.get(`/project-member-management/${projectId}/members`);

  const items = response.data;


  const res = items.map((item: any) => ({
    createdAt: dayjs(item.CreatedAt),
    workerData: {
      createdAt: dayjs(item.Worker.CreatedOn),
      isAdmin: item.Worker.CanManageWorkers,
      isManager: item.Worker.CanManageProjects,
      email: item.Worker.Email,
      firstName: item.Worker.Name,
      secondName: item.Worker.SecondName,
      status: {
        id: item.Worker.WorkerStatus.Id,
        name: workerStatuses.find(status => status.id === item.Worker.WorkerStatus.Id)?.name ?? "Не указан",
        color: workerStatuses.find(status => status.id === item.Worker.WorkerStatus.Id)?.color ?? "blue"
      },
      thirdName: item.Worker.ThirdName,
      id: item.Worker.Id,
      workerPosition: {
        id: item.Worker.WorkerPosition.Id,
        canAssignTasksTo: [],
        canTakeTasksFrom: [],
        title: item.Worker.WorkerPosition.Title,
      },
    },
    taskTakers: item.TaskTakers.map((taker: any) => (taker.Id))

  }) as IProjectMember)

  return res;
};

// get project member permissions
export const getProjectMemberQuery = async (
  projectId: number,
  workerId: number
): Promise<{
  worker: IWorker;
  taskGivers: IWorker[];
  taskTakers: IWorker[];
}> => {
  const response = await apiClient.get(`/project-member-management/${projectId}/members/${workerId}`);

  const res = {
    worker: {
      id: response.data.Worker.Id,
      firstName: response.data.Worker.Name,
      secondName: response.data.Worker.SecondName,
      thirdName: response.data.Worker.ThirdName,
      email: response.data.Worker.Email,
      createdAt: dayjs(response.data.Worker.CreatedOn),
      isAdmin: response.data.Worker.CanManageWorkers,
      isManager: response.data.Worker.CanManageProjects,
      status: {
        id: response.data.Worker.WorkerStatus.Id,
        name: workerStatuses.find(status => status.id === response.data.Worker.WorkerStatus.Id)?.name ?? "Не указан",
        color: workerStatuses.find(status => status.id === response.data.Worker.WorkerStatus.Id)?.color ?? "blue"
      },
      workerPosition: {
        id: response.data.Worker.WorkerPosition.Id,
        title: response.data.Worker.WorkerPosition.Title,
        canAssignTasksTo: [],
        canTakeTasksFrom: []
      }
    },
    taskGivers: response.data.TaskGivers.map((giver: any) => ({
      id: giver.Id,
      firstName: giver.Name,
      secondName: giver.SecondName,
      thirdName: giver.ThirdName,
      email: giver.Email,
      createdAt: dayjs(giver.CreatedOn),
      isAdmin: giver.CanManageWorkers,
      isManager: giver.CanManageProjects,
      status: {
        id: giver.WorkerStatus.Id,
        name: workerStatuses.find(status => status.id === giver.WorkerStatus.Id)?.name ?? "Не указан",
        color: workerStatuses.find(status => status.id === giver.WorkerStatus.Id)?.color ?? "blue"
      },
      workerPosition: {
        id: giver.WorkerPosition.Id,
        title: giver.WorkerPosition.Title,
        canAssignTasksTo: [],
        canTakeTasksFrom: []
      }
    }) as IWorker),
    taskTakers: response.data.TaskTakers.map((taker: any) => ({
      id: taker.Id,
      firstName: taker.Name,
      secondName: taker.SecondName,
      thirdName: taker.ThirdName,
      email: taker.Email,
      createdAt: dayjs(taker.CreatedOn),
      isAdmin: taker.CanManageWorkers,
      isManager: taker.CanManageProjects,
      status: {
        id: taker.WorkerStatus.Id,
        name: workerStatuses.find(status => status.id === taker.WorkerStatus.Id)?.name ?? "Не указан",
        color: workerStatuses.find(status => status.id === taker.WorkerStatus.Id)?.color ?? "blue"
      },
      workerPosition: {
        id: taker.WorkerPosition.Id,
        title: taker.WorkerPosition.Title,
        canAssignTasksTo: [],
        canTakeTasksFrom: []
      }
    }) as IWorker)
  } as {
    worker: IWorker;
    taskGivers: IWorker[];
    taskTakers: IWorker[];
  };
  return res


};



export const addMemberToProjectQuery = async (
  projectId: number,
  workerId: number,
): Promise<boolean> => {
  try {
    const response = await apiClient.post(
      `/project-member-management/${projectId}/members`, {
      workerId: workerId
    }
    );
    return response.status === 200;
  } catch (error) {
    throw new Error("Could not add member to project");
  }
};

// remove member from project
export const removeMemberFromProjectQuery = async (
  projectId: number,
  workerId: number
): Promise<boolean> => {
  const response = await apiClient.delete(`/project-member-management/${projectId}/members/${workerId}`);
  return response.status === 200
};


export const addTaskReceiverQuery = async (
  projectId: number,
  workerId: number,
  receiverId: number
): Promise<boolean> => {
  const response = await apiClient.post(`project-member-management/${projectId}/members/${workerId}/subordinates`, {
    workerId: receiverId
  });
  return response.status === 200;
};


export const removeTaskReceiverQuery = async (
  projectId: number,
  workerId: number,
  receiverId: number
): Promise<boolean> => {
  const response = await apiClient.delete(`project-member-management/${projectId}/members/${workerId}/subordinates/${receiverId}`);
  return response.status === 200;
};


// get all project files
export const getProjectFilesQuery = async (
  projectId: number,
  filters: IFileFilterOptions,
  page: number,
  limit?: number
): Promise<IFile[]> => {
  const response = await apiClient.get(`/${FileOwnerEnum.PROJECT}/${projectId}/files`);
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
      id: projectId,
      name: "project",
      type: FileOwnerEnum.PROJECT
    }
  }) as IFile)

  return filterFiles(items, filters, page, limit);
};

export const getAvailalbeProjectFilesResponsibleWorkers = async (
): Promise<IWorkerFields[]> => {
  return await getWorkersQuery({})
};

// add project file
export const addProjectFileQuery = async (
  projectId: number,
  data: IAddFileOptions,
): Promise<boolean> => {
  const formData = new FormData();
  formData.append('file', data.file); // Append the file
  formData.append('title', data.name); // Append the title
  if (data.description) formData.append('description', data.description);

  const response = await apiClient.post(`/${FileOwnerEnum.PROJECT}/${projectId}/files`, formData, {
    headers: {
      "Content-Type": "multipart/form-data", // Явно указываем для ясности
    },
  });


  return response.status === 201
};

// delete project file
export const removeProjectFileQuery = async (
  projectId: number,
  fileId: number,
): Promise<boolean> => {
  const response = await apiClient.delete(
    `/${FileOwnerEnum.PROJECT}/${projectId}/files/${fileId}`,
  );

  return response.status === 204;
};


// download project file
export const downloadProjectFileQuery = async (projectId: number, fileId: number) => {
  const response = await apiClient.get(`/${FileOwnerEnum.PROJECT}/${projectId}/files/${fileId}`, {
    responseType: 'blob', // Expect binary data
  });

  if (response.status !== 200) {
    throw new Error('Failed to download file');
  }

  return response.data as Blob;
};


//get worker history
export const getProjectHistoryQuery = async (
  projectId: number,
  filters: IHistoryFilterOptions,
  page: number,
  limit?: number
): Promise<IHistoryItem[]> => {
  const response = await apiClient.get(`/history/${HistoryOwnerEnum.PROJECT}/${projectId}`)
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

export const getAvailalbeProjectHistoryResponsibleWorkers = async (
  projectId: number
): Promise<IWorkerFields[]> => {
  console.log(projectId);
  const workers = await getWorkersQuery({});
  return workers;
};

//remove worker history
export const removeProjectHistoryQuery = async (
  projectId: number
): Promise<boolean> => {
  const response = await apiClient.delete(`/history/${HistoryOwnerEnum.PROJECT}/${projectId}`)
  return response.status === 200;
};

//remove one worker history item
export const removeProjectHistoryItemQuery = async (
  itemId: number
): Promise<boolean> => {
  const response = await apiClient.delete(`/history/${itemId}`)
  return response.status === 200;
};


// get project links
export const getProjectLinksQuery = async (
  projectId: number,
): Promise<ILink[]> => {
  const response = await apiClient.get(`/projects/${projectId}/links`);


  const res = response.data.map((link: any) => ({
    id: link.Id,
    name: link.Description,
    link: link.Link,
    owner: {
      id: projectId,
      name: "",
      type: LinkOwnerEnum.PROJECT
    }
  }) as ILink)

  return res;
}

// add project link
export const addProjectLinkQuery = async (
  projectId: number,
  data: IAddLinkOptions
): Promise<boolean> => {
  const response = await apiClient.post(`/projects/${projectId}/links`, {
    link: data.link,
    description: data.name
  });

  return response.status === 201
};

// edit project link
export const editProjectLinkQuery = async (
  projectId: number,
  linkId: number,
  data: IEditLinkOptions
): Promise<boolean> => {
  const response = await apiClient.put(`/projects/${projectId}/links/${linkId}`, {
    link: data.link,
    description: data.name
  });

  return response.status === 204
};

// remove project link
export const removeProjectLinkQuery = async (
  projectId: number,
  linkId: number,
): Promise<boolean> => {
  const response = await apiClient.delete(`/projects/${projectId}/links/${linkId}`);

  return response.status === 204
};

// Get project boards (this query looks correct as is)
export const getProjectBoardsQuery = async (
  projectId: number,
  filters: IBoardFilterOptopns,
  page: number,
  limit?: number
): Promise<IBoard[]> => {
  const response = await apiClient.get(`/boards/project/${projectId}`);
  const project = await getProjectQuery(projectId);
  const items = response.data.map((item: any) => ({
    id: item.Id,
    boardBasis: mapBackendToBasis(item.Basis),
    createdAt: dayjs(item.CreatedOn),
    name: item.Name,
    owner: {
      id: projectId,
      name: project.name,
      type: BoardOwnerEnum.PROJECT
    },
  }) as IBoard)
  return filterBoards(items, filters, page, limit);
};

// Add a project board
export const addProjectBoardQuery = async (
  projectId: number,
  data: IAddBoardOptions,
  creator: IWorker
): Promise<IBoard> => {
  const payload = {
    name: data.name,
    description: '', // Provide a default empty string if description is not provided
    projectId: projectId, // Set the projectId from the parameter
    basis: mapBoardBasisToBackend(data.basis), // Map the basis enum to the backend numeric value
  };

  const response = await apiClient.post(`/boards/project/${projectId}`, payload, {
    headers: {
      "X-Creator-Id": creator.id.toString(), // Pass the creator ID in headers if required by the API
    },
  });

  return response.data;
};




export const getProjectSprintsQuery = async (
  projectId: number,
  filters: ISprintFilterOptions
): Promise<ISprint[]> => {
  const response = await apiClient.get(`/sprints/project/${projectId}`);

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

// add sprint to project
export const addSprintToProjectQuery = async (
  projectId: number,
  data: IAddSprintOptions
): Promise<boolean> => {
  const response = await apiClient.post(`/sprints`, {
    title: data.name,
    goal: "",
    startsOn: data.startDate ? data.startDate.utc().toISOString() : null, // Convert to UTC ISO string
    expireOn: data.endDate ? data.endDate.utc().toISOString() : null,
    projectId: projectId,
    sprintStatusId: data.status.id
  })

  return response.status === 200;
};




export const getProjectAnalytics = async (projectId: number): Promise<ProjectAnalyticsData> => {
  const response = await apiClient.get(`analytics/project/${projectId}`);

  const data: ApiProjectAnalyticsResponse = response.data;

  console.log(data);

  const res: ProjectAnalyticsData = {
    tasksByStatus: Object.fromEntries(
      data.TasksByStatus.map(({ Status, Count }) => [statusNameMapping[Status.Name] || Status.Name, Count])
    ),
    tasksByPriority: Object.fromEntries(
      data.TasksByPriority.map(({ Priority, Count }) => [Priority.Name, Count])
    ),
    tasksByTag: Object.fromEntries(
      data.TasksByTag.map(({ Tag, Count }) => [Tag.Name, Count])
    ),
    tasksByEmployee: data.TasksByEmployee.map(({ Item1, Item2 }) => ({
      email: Item1.Email,
      statuses: Object.fromEntries(
        Item2.map(({ Status, Count }) => [statusNameMapping[Status.Name] || Status.Name, Count])
      ),
    })),
    tasksBySprint: data.TasksBySprint.map(({ Sprint, completedTasksNum }) => ({
      sprint: {
        id: Sprint.Id,
        name: Sprint.Title,
        projectId: projectId, // Use input projectId since ProjectName is empty
        status: { id: Sprint.SprintStatusId, name: Sprint.SprintStatus.Name, color: Sprint.SprintStatus.Color } as ISprintStatus,
        startDate: Sprint.StartsOn ? dayjs(Sprint.StartsOn) : undefined,
        endDate: Sprint.ExpireOn ? dayjs(Sprint.ExpireOn) : undefined,
        goal: Sprint.Goal || undefined,
      },
      completedTasks: completedTasksNum,
    })),
    overdueTasks: data.OverdueTasks,
  };

  return res;
};