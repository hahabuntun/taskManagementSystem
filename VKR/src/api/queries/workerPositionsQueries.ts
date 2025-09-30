import { apiClient } from "../../config/axiosConfig";
import { IWorkerPosition, IWorkerPositionSummary } from "../../interfaces/IWorkerPosition";
import { IAddWorkerPositionOptions } from "../options/createOptions/IAddWorkerPositionOptions";
import { IEditWorkerPositionOptions } from "../options/editOptions/IEditWorkerPositionOptions";

// Get all worker positions
export const getWorkerPositionsQuery = async (): Promise<IWorkerPosition[]> => {
  try {
    const response = await apiClient.get('/worker-positions');

    return response.data.map((item: any) => ({
      id: item.Id,
      title: item.Title,
      createdOn: item.CreatedOn,
      canAssignTasksTo: item.TaskTakers.map((taker: any) => ({
        id: taker.Id,
        title: taker.Title
      })) as IWorkerPositionSummary[],
      assignedTasksBy: item.TaskGivers.map((giver: any) => ({
        id: giver.Id,
        title: giver.Title
      })) as IWorkerPositionSummary[]
    }));
  } catch (error: any) {

    throw new Error(`Failed to fetch worker positions: ${error.response?.data?.errors || error.message}`);
  }
};

// Get worker position by ID
export const getWorkerPositionQuery = async (workerPositionId: number): Promise<IWorkerPosition> => {
  try {
    const response = await apiClient.get(`/worker-positions/${workerPositionId}`);

    const item = response.data;
    return {
      id: item.Id,
      title: item.Title,
      canAssignTasksTo: item.TaskTakers.map((taker: any) => ({
        id: taker.Id,
        title: taker.title
      })) as IWorkerPositionSummary[],
      canTakeTasksFrom: item.TaskGivers.map((giver: any) => ({
        id: giver.Id,
        title: giver.Title
      })) as IWorkerPositionSummary[]
    };
  } catch (error: any) {

    throw new Error(`Failed to fetch worker position: ${error.response?.data?.errors || error.message}`);
  }
};

// Add worker position
export const addWorkerPositionQuery = async (
  data: IAddWorkerPositionOptions
): Promise<IWorkerPosition> => {
  try {
    const response = await apiClient.post('/worker-positions', {
      title: data.title,
      taskGiverIds: [],
      taskTakerIds: []
    });

    const item = response.data;
    return {
      id: item.Id,
      title: item.Title,
      canAssignTasksTo: item.TaskTakers.map((taker: any) => ({
        id: taker.Id,
        title: taker.Title
      })) as IWorkerPositionSummary[],
      canTakeTasksFrom: item.TaskGivers.map((giver: any) => ({
        id: giver.Id,
        title: giver.Title
      })) as IWorkerPositionSummary[]
    };
  } catch (error: any) {

    throw new Error(`Failed to add worker position: ${error.response?.data?.errors || error.message}`);
  }
};

// Edit worker position
export const editWorkerPositionQuery = async (
  workerPositionId: number,
  data: IEditWorkerPositionOptions
): Promise<IWorkerPosition> => {
  try {
    const response = await apiClient.put(`/worker-positions/${workerPositionId}`, {
      title: data.title,
      taskGiverIds: data.canTakeTasksFromIds.length > 0 ? data.canTakeTasksFromIds : [],
      taskTakerIds: data.canAssignTasksToIds.length > 0 ? data.canAssignTasksToIds : []
    });

    const item = response.data;
    return {
      id: item.Id,
      title: item.Title,
      canAssignTasksTo: item.TaskTakers.map((taker: any) => ({
        id: taker.Id,
        title: taker.Title
      })) as IWorkerPositionSummary[],
      canTakeTasksFrom: item.TaskGivers.map((giver: any) => ({
        id: giver.Id,
        title: giver.Title
      })) as IWorkerPositionSummary[]
    };
  } catch (error: any) {

    throw new Error(`Failed to edit worker position: ${error.response?.data?.errors || error.message}`);
  }
};

// Remove worker position
export const removeWorkerPositionQuery = async (
  workerPositionId: number
): Promise<boolean> => {
  try {
    const response = await apiClient.delete(`/worker-positions/${workerPositionId}`);
    return response.status === 204;
  } catch (error: any) {

    throw new Error(`Failed to delete worker position: ${error.response?.data?.errors || error.message}`);
  }
};