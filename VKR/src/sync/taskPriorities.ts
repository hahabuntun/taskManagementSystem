import { TaskPriorityEnum } from "../enums/TaskPriorityEnum";
import { ITaskPriority } from "../interfaces/ITaskPriority";

export const taskPriorities: ITaskPriority[] = [
    {
        id: 1,
        name: TaskPriorityEnum.LOW,
        color: "#8BC34A", // Светло-зеленый - спокойствие, низкая срочность.
    },
    {
        id: 2,
        name: TaskPriorityEnum.NORMAL,
        color: "#FFC107", // Ярко-желтый - умеренная важность, внимание.
    },
    {
        id: 3,
        name: TaskPriorityEnum.HIGH,
        color: "#fa8c16", // Оранжево-красный - высокая срочность, напряжение.
    },
    {
        id: 4,
        name: TaskPriorityEnum.CRITICAL,
        color: "#F44336", // Красный - критическая важность, экстренная ситуация.
    },
];