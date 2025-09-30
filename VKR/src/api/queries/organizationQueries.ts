import { IOrganization } from "../../interfaces/IOrganization";
import { filterFiles, filterHistory } from '../dummyData/filters';
import { IHistoryItem } from '../../interfaces/IHistoryItem';
import { IHistoryFilterOptions } from '../options/filterOptions/IHistoryFilterOptions';
import { IFile } from '../../interfaces/IFile';
import { IAddFileOptions } from '../options/createOptions/IAddFileOptions';
import { IFileFilterOptions } from '../options/filterOptions/IFileFilterOptions';

import dayjs from "dayjs";
import { FileOwnerEnum } from '../../enums/ownerEntities/FileOwnerEnum';
import { HistoryOwnerEnum } from '../../enums/ownerEntities/HistoryOwnerEnum';
import { IWorkerFields } from '../../interfaces/IWorkerFields';
import { ApiOrganizationAnalyticsResponse, OrganizationAnalyticsData } from '../../interfaces/analytics';
import { apiClient } from '../../config/axiosConfig';
import { getWorkersQuery } from './workersQueries';

export const getOrganizationQuery = async (): Promise<IOrganization> => {
  const response = await apiClient.get('/organization');
  const data = response.data;

  if (!Array.isArray(data) || data.length === 0) {
    throw new Error('No organization data received');
  }

  const org = data[0]; // Take the first organization

  return {
    id: org.Id,
    name: org.Name,
    owner: {
      id: org.Owner.Id,
      firstName: org.Owner.Name,
      secondName: org.Owner.SecondName,
      thirdName: org.Owner.ThirdName ?? '',
      email: org.Owner.Email,
    },
    description: '', // Not provided in OrganizationDTO
    createdAt: dayjs(org.CreatedOn),
  };

};



// get all organization files
export const getOrganizationFilesQuery = async (
  orgId: number,
  filters: IFileFilterOptions,
  page: number,
  limit?: number
): Promise<IFile[]> => {
  const response = await apiClient.get(`/${FileOwnerEnum.ORGANIZATION}/${orgId}/files`);
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
      id: orgId,
      name: "Org",
      type: FileOwnerEnum.ORGANIZATION
    }
  }) as IFile)

  return filterFiles(items, filters, page, limit);
};

export const getAvailalbeOrganizationFilesResponsibleWorkers = async (
): Promise<IWorkerFields[]> => {
  return await getWorkersQuery({})
};

// add organization file
export const addOrganizationFileQuery = async (
  data: IAddFileOptions,
): Promise<boolean> => {
  const formData = new FormData();
  formData.append('file', data.file); // Append the file
  formData.append('title', data.name); // Append the title
  if (data.description) formData.append('description', data.description);

  const response = await apiClient.post(`/${FileOwnerEnum.ORGANIZATION}/${1}/files`, formData, {
    headers: {
      "Content-Type": "multipart/form-data", // Явно указываем для ясности
    },
  });

  console.log(response)

  return response.status === 201
};

// delete organization file
export const removeOrganizationFileQuery = async (
  itemId: number
): Promise<boolean> => {
  const response = await apiClient.delete(
    `/${FileOwnerEnum.ORGANIZATION}/${1}/files/${itemId}`,
  );

  return response.status === 204;
};

export const downloadOrganizationFileQuery = async (
  orgId: number,
  fileId: number
): Promise<Blob> => {
  const response = await apiClient.get(`/${FileOwnerEnum.ORGANIZATION}/${orgId}/files/${fileId}`, {
    responseType: 'blob', // Expect binary data
  });

  if (response.status !== 200) {
    throw new Error('Failed to download file');
  }

  return response.data as Blob;
};




//get worker history
export const getOrganizationHistoryQuery = async (
  orgId: number,
  filters: IHistoryFilterOptions,
  page: number,
  limit?: number
): Promise<IHistoryItem[]> => {
  const response = await apiClient.get(`/history/${HistoryOwnerEnum.ORGANIZATION}/${orgId}`)
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

export const getAvailalbeOrganizationHistoryResponsibleWorkers = async (
): Promise<IWorkerFields[]> => {
  return await getWorkersQuery({})
};

//remove worker history
export const removeOrganizationHistoryQuery = async (): Promise<boolean> => {
  const response = await apiClient.delete(`/history/${HistoryOwnerEnum.ORGANIZATION}/1`)
  return response.status === 200;
};

//remove one worker history item
export const removeOrganizationHistoryItemQuery = async (
  itemId: number
): Promise<boolean> => {
  const response = await apiClient.delete(`/history/${itemId}`)
  return response.status === 200;
};


export const getOrganizationAnalytics = async (): Promise<OrganizationAnalyticsData> => {
  const response = await apiClient.get(`analytics/organization/${1}`)

  console.log(response)

  const data: ApiOrganizationAnalyticsResponse = response.data;

  const resp = {
    tasksByStatus: Object.fromEntries(
      data.TasksByStatus.map(({ Status, Count }) => [Status.Name, Count])
    ),
    tasksByPriority: Object.fromEntries(
      data.TasksByPriority.map(({ Priority, Count }) => [Priority.Name, Count])
    ),
    tasksByTag: Object.fromEntries(
      data.TasksByTag.map(({ Tag, Count }) => [Tag.Name, Count])
    ),
    tasksByProject: data.TasksByProject.map(({ Item1, Item2, Item3 }) => ({
      projectName: Item1,
      completed: Item2,
      overdue: Item3,
    })),
    tasksByEmployee: data.TasksByEmployee.map(({ Item1, Item2 }) => ({
      email: Item1.Email,
      statuses: Object.fromEntries(
        Item2.map(({ Status, Count }) => [Status.Name, Count])
      ),
    })),
    overdueTasks: data.OverdueTasks,
  };
  console.log(resp)
  return resp
};