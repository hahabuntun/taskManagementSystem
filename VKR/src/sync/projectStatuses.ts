import { ProjectStatusEnum } from "../enums/statuses/ProjectStatusEnum";
import { IProjectStatus } from "../interfaces/IProjectStatus";

export const projectStatuses: IProjectStatus[] = [
    {
        id: 1,
        name: ProjectStatusEnum.INITIALIZING,
        color: "#ffd666", // Золотой - символ начала, подготовки, важности.
    },
    {
        id: 2,
        name: ProjectStatusEnum.IN_WORK,
        color: "#95de64", // Зеленый - активность, движение, прогресс.
    },
    {
        id: 3,
        name: ProjectStatusEnum.AWAITING_CHECK,
        color: "#ff9c6e", // Оранжевый - ожидание, анализ, переходный этап.
    },
    {
        id: 4,
        name: ProjectStatusEnum.COMPLETED,
        color: "#69b1ff", // Синий - завершение, стабильность, успех.
    },
    {
        id: 5,
        name: ProjectStatusEnum.ARCHIVED,
        color: "#f0f0f0",
    },
];