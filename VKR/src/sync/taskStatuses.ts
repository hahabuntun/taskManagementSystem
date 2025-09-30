import { TaskStatusEnum } from "../enums/statuses/TaskStatusEnum";
import { ITaskStatus } from "../interfaces/ITaskStatus";

export const taskStatuses: ITaskStatus[] = [
    {
        id: 1,
        name: TaskStatusEnum.PENDING,
        color: "#ffd666", // Золотой - ожидание, подготовка.
    },
    {
        id: 2,
        name: TaskStatusEnum.IN_WORK,
        color: "#95de64", // Зеленый - активность, движение.
    },
    {
        id: 3,
        name: TaskStatusEnum.AWAITING_CHECK,
        color: "#ff9c6e", // Оранжевый - анализ, переходный этап.
    },
    {
        id: 4,
        name: TaskStatusEnum.COMPLETED,
        color: "#69b1ff", // Синий - завершение, успех.
    },
    {
        id: 5,
        name: TaskStatusEnum.ARCHIVED,
        color: "#f0f0f0", // Серый - архив, неактивность.
    },
];
