import { IFile } from "../../interfaces/IFile";
import { IHistoryItem } from "../../interfaces/IHistoryItem";
import { IProject } from "../../interfaces/IProject";
import { ISprint } from "../../interfaces/ISprint";
import { ITask } from "../../interfaces/ITask";
import { IWorker } from "../../interfaces/IWorker";
import { IFileFilterOptions } from "../options/filterOptions/IFileFilterOptions";
import { IHistoryFilterOptions } from "../options/filterOptions/IHistoryFilterOptions";
import { IProjectFilterOptions } from "../options/filterOptions/IProjectFilterOptions";
import { ISprintFilterOptions } from "../options/filterOptions/ISprintFilterOptions";
import { ITaskFilterOptions } from "../options/filterOptions/ITaskFilterOptions";
import { IWorkerFilterOptions } from "../options/filterOptions/IWorkerFilterOptions";
import { IBoard } from "../../interfaces/IBoard";
import dayjs from "dayjs"; // Import dayjs for conversion
import { INotification } from "../../interfaces/INotification";
import { INotificationFilterOptions } from "../options/filterOptions/INotificationFilterOptions";
import { ITaskTemplate } from "../../interfaces/ITaskTemplate";
import { ITaskTemplateFilterOptions } from "../options/filterOptions/ITaskTemplateFilterOptions";

// Helper function to compare dates (works with Date objects)
const isDateInRange = (date: Date | undefined, start: Date | undefined, end: Date | undefined): boolean => {
  if (!date) return false;
  const dateTime = date.getTime();
  const startTime = start ? start.getTime() : Number.NEGATIVE_INFINITY;
  const endTime = end ? end.getTime() : Number.POSITIVE_INFINITY;
  return dateTime >= startTime && dateTime <= endTime;
};

// Convert Dayjs to Date if necessary
const toDate = (date: Date | dayjs.Dayjs | undefined): Date | undefined => {
  if (!date) return undefined;
  return new Date(date.toISOString());
};

export const filterTasks = (
  tasks: ITask[],
  filters: ITaskFilterOptions,
  page: number,
  limit?: number
): ITask[] => {
  let filteredTasks = tasks.filter((task) => {
    return (
      (!filters.name || task.name.toLowerCase().includes(filters.name.toLowerCase())) &&
      (!filters.description || task.description.toLowerCase().includes(filters.description.toLowerCase())) &&
      (!filters.type || task.type === filters.type) &&
      (!filters.createdFrom || isDateInRange(toDate(task.createdAt), toDate(filters.createdFrom), undefined)) &&
      (!filters.createdTill || isDateInRange(toDate(task.createdAt), undefined, toDate(filters.createdTill))) &&
      (!filters.startedFrom || (task.startDate && isDateInRange(toDate(task.startDate), toDate(filters.startedFrom), undefined))) &&
      (!filters.startedTill || (task.startDate && isDateInRange(toDate(task.startDate), undefined, toDate(filters.startedTill)))) &&
      (!filters.endDateFrom || (task.endDate && isDateInRange(toDate(task.endDate), toDate(filters.endDateFrom), undefined))) &&
      (!filters.endDateTill || (task.endDate && isDateInRange(toDate(task.endDate), undefined, toDate(filters.endDateTill)))) &&
      (!filters.status || task.status.name === filters.status.name) &&
      (!filters.priority || task.priority.name === filters.priority.name) &&
      (!filters.creator || task.creator.id === filters.creator.id) &&
      (!filters.tags || filters.tags.every((tag) => task.tags.some((t) => t.name === tag.name)))
    );
  });

  if (limit !== undefined) {
    const start = (page - 1) * limit;
    const end = start + limit;
    filteredTasks = filteredTasks.slice(start, end);
  }

  return filteredTasks;
};

export const filterProjects = (
  projects: IProject[],
  filters: IProjectFilterOptions,
  page: number,
  limit?: number
): IProject[] => {
  let filteredProjects = projects.filter((project) => {
    return (
      (!filters.name || project.name.toLowerCase().includes(filters.name.toLowerCase())) &&
      (!filters.status || project.status.name === filters.status.name) &&
      (!filters.manager || project.manager.id === filters.manager.id) &&
      (!filters.tags || filters.tags.every((tag) => project.tags.some((t) => t.name === tag.name))) &&
      (!filters.createdFrom || isDateInRange(toDate(project.createdAt), toDate(filters.createdFrom), undefined)) &&
      (!filters.createdTill || isDateInRange(toDate(project.createdAt), undefined, toDate(filters.createdTill))) &&
      (!filters.startedFrom || (project.startDate && isDateInRange(toDate(project.startDate), toDate(filters.startedFrom), undefined))) &&
      (!filters.startedTill || (project.startDate && isDateInRange(toDate(project.startDate), undefined, toDate(filters.startedTill)))) &&
      (!filters.endDateFrom || (project.endDate && isDateInRange(toDate(project.endDate), toDate(filters.endDateFrom), undefined))) &&
      (!filters.endDateTill || (project.endDate && isDateInRange(toDate(project.endDate), undefined, toDate(filters.endDateTill))))
    );
  });

  if (limit !== undefined) {
    const start = (page - 1) * limit;
    const end = start + limit;
    filteredProjects = filteredProjects.slice(start, end);
  }

  return filteredProjects;
};

export const filterWorkers = (
  workers: IWorker[],
  filters: IWorkerFilterOptions,
  page: number,
  limit?: number
): IWorker[] => {
  let filteredWorkers = workers.filter((worker) => {
    return (
      (!filters.firstName || worker.firstName.toLowerCase().includes(filters.firstName.toLowerCase())) &&
      (!filters.secondName || worker.secondName.toLowerCase().includes(filters.secondName.toLowerCase())) &&
      (!filters.thirdName || worker.thirdName.toLowerCase().includes(filters.thirdName.toLowerCase())) &&
      (!filters.email || worker.email.toLowerCase().includes(filters.email.toLowerCase())) &&
      (!filters.createdFrom || isDateInRange(toDate(worker.createdAt), toDate(filters.createdFrom), undefined)) &&
      (!filters.createdTill || isDateInRange(toDate(worker.createdAt), undefined, toDate(filters.createdTill))) &&
      (!filters.status || worker.status.name === filters.status.name) &&
      (!filters.workerPosition || worker.workerPosition.title === filters.workerPosition.title) &&
      (!filters.isAdmin || worker.isAdmin === filters.isAdmin) &&
      (!filters.isManager || worker.isManager === filters.isManager)
    );
  });

  if (limit !== undefined) {
    const start = (page - 1) * limit;
    const end = start + limit;
    filteredWorkers = filteredWorkers.slice(start, end);
  }

  return filteredWorkers;
};

export const filterFiles = (
  files: IFile[],
  filters: IFileFilterOptions,
  page: number,
  limit?: number
): IFile[] => {
  let filteredFiles = files.filter((file) => {
    return (
      (!filters.name || file.name.toLowerCase().includes(filters.name.toLowerCase())) &&
      (!filters.creator || file.creator?.id === filters.creator.id) &&
      (!filters.createdFrom || isDateInRange(toDate(file.createdAt), toDate(filters.createdFrom), undefined)) &&
      (!filters.createdTill || isDateInRange(toDate(file.createdAt), undefined, toDate(filters.createdTill)))
    );
  });

  if (limit !== undefined) {
    const start = (page - 1) * limit;
    const end = start + limit;
    filteredFiles = filteredFiles.slice(start, end);
  }

  return filteredFiles;
};

export const filterHistory = (
  history: IHistoryItem[],
  filters: IHistoryFilterOptions,
  page: number,
  limit?: number
): IHistoryItem[] => {
  let filteredHistory = history.filter((item) => {
    return (
      (!filters.text || item.message.toLowerCase().includes(filters.text.toLowerCase())) &&
      (!filters.responsibleWorker || item.responsibleWorker?.id === filters.responsibleWorker.id) &&
      (!filters.createdFrom || isDateInRange(toDate(item.createdAt), toDate(filters.createdFrom), undefined)) &&
      (!filters.createdTill || isDateInRange(toDate(item.createdAt), undefined, toDate(filters.createdTill)))
    );
  });

  if (limit !== undefined) {
    const start = (page - 1) * limit;
    const end = start + limit;
    filteredHistory = filteredHistory.slice(start, end);
  }

  return filteredHistory;
};

export const filterSprints = (
  sprints: ISprint[],
  filters: ISprintFilterOptions,
  page: number = 1,
  limit?: number
): ISprint[] => {
  let filteredSprints = sprints.filter((sprint) => {
    return (
      (!filters.name || sprint.name.toLowerCase().includes(filters.name.toLowerCase())) &&
      (!filters.startDate || (sprint.startDate && isDateInRange(toDate(sprint.startDate), toDate(filters.startDate), undefined))) &&
      (!filters.endDate || (sprint.endDate && isDateInRange(toDate(sprint.endDate), undefined, toDate(filters.endDate)))) &&
      (!filters.status || sprint.status.name === filters.status.name)
    );
  });

  if (limit !== undefined) {
    const start = (page - 1) * limit;
    const end = start + limit;
    filteredSprints = filteredSprints.slice(start, end);
  }

  return filteredSprints;
};

export const filterBoards = (
  boards: IBoard[],
  filters: ITaskFilterOptions,
  page: number = 1,
  limit?: number
): IBoard[] => {
  let filteredBoards = boards.filter((board) => {
    return (
      (!filters.name || board.name.toLowerCase().includes(filters.name.toLowerCase())) &&
      (!filters.createdFrom || (board.createdAt && isDateInRange(toDate(board.createdAt), toDate(filters.createdFrom), undefined))) &&
      (!filters.createdTill || (board.createdAt && isDateInRange(toDate(board.createdAt), undefined, toDate(filters.createdTill))))
    );
  });

  if (limit !== undefined) {
    const start = (page - 1) * limit;
    const end = start + limit;
    filteredBoards = filteredBoards.slice(start, end);
  }

  return filteredBoards;
};



export const filterNotifications = (
  notifications: INotification[],
  filters: INotificationFilterOptions,
  page: number = 1,
  limit?: number
): INotification[] => {
  let filteredNotifications = notifications.filter((notification) => {
    return (
      (!filters.type || notification.relatedEntity.type === filters.type) &&
      (!filters.name ||
        notification.relatedEntity.name.toLowerCase().includes(filters.name.toLowerCase())) &&
      (!filters.message ||
        notification.message.toLowerCase().includes(filters.message.toLowerCase())) &&
      (!filters.createdFrom ||
        isDateInRange(toDate(notification.createdAt), toDate(filters.createdFrom), undefined)) &&
      (!filters.createdTill ||
        isDateInRange(toDate(notification.createdAt), undefined, toDate(filters.createdTill))) &&
      (filters.isRead === undefined || notification.isRead === filters.isRead) &&
      (!filters.responsibleWorker ||
        notification.responsibleWorker.id === filters.responsibleWorker.id)
    );
  });

  if (limit !== undefined) {
    const start = (page - 1) * limit;
    const end = start + limit;
    filteredNotifications = filteredNotifications.slice(start, end);
  }

  return filteredNotifications;
};




export const filterTaskTemplates = (
  templates: ITaskTemplate[],
  filters: ITaskTemplateFilterOptions,
  page: number = 1,
  limit?: number
): ITaskTemplate[] => {
  let filteredTemplates = templates.filter((template) => {
    return (
      (!filters.name || template.name.toLowerCase().includes(filters.name.toLowerCase())) &&
      (!filters.taskName || (template.taskName && template.taskName.toLowerCase().includes(filters.taskName.toLowerCase()))) &&
      (!filters.description || (template.description && template.description.toLowerCase().includes(filters.description.toLowerCase()))) &&
      (!filters.type || template.type === filters.type) &&
      (!filters.createdFrom || isDateInRange(toDate(template.createdAt), toDate(filters.createdFrom), undefined)) &&
      (!filters.createdTill || isDateInRange(toDate(template.createdAt), undefined, toDate(filters.createdTill))) &&
      (!filters.startedFrom || (template.startDate && isDateInRange(toDate(template.startDate), toDate(filters.startedFrom), undefined))) &&
      (!filters.startedTill || (template.startDate && isDateInRange(toDate(template.startDate), undefined, toDate(filters.startedTill)))) &&
      (!filters.endDateFrom || (template.endDate && isDateInRange(toDate(template.endDate), toDate(filters.endDateFrom), undefined))) &&
      (!filters.endDateTill || (template.endDate && isDateInRange(toDate(template.endDate), undefined, toDate(filters.endDateTill)))) &&
      (!filters.status || template.status?.name === filters.status.name) &&
      (!filters.priority || template.priority?.name === filters.priority.name) &&
      (!filters.tags || filters.tags.every((tag) => template.tags.some((t) => t.name === tag.name)))
    );
  });

  if (limit !== undefined) {
    const start = (page - 1) * limit;
    const end = start + limit;
    filteredTemplates = filteredTemplates.slice(start, end);
  }

  return filteredTemplates;
};