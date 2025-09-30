export interface IWorkerPositionSummary {
  id: number;
  title: string;
}

export interface IWorkerPosition {
  id: number;
  title: string;
  canAssignTasksTo: IWorkerPositionSummary[];
  canTakeTasksFrom: IWorkerPositionSummary[];
}