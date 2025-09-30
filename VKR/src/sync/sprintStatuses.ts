import { SprintStatusEnum } from "../enums/statuses/SprintStatusEnum";
import { ISprintStatus } from "../interfaces/ISprintStatus";

export const sprintStatuses: ISprintStatus[] = [
    { id: 1, name: SprintStatusEnum.PLANNED, color: "#ffd666" },
    { id: 2, name: SprintStatusEnum.ACTIVE, color: '#95de64' },
    { id: 3, name: SprintStatusEnum.FINISHED, color: '#69b1ff' },
]