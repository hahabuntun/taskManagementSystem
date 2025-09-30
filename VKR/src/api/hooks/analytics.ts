// api/hooks/analytics.ts
import { useQuery } from "@tanstack/react-query";
import { getOrganizationAnalytics } from "../queries/organizationQueries";
import { getProjectAnalytics } from "../queries/projectsQueries";
import { getSprintAnalytics } from "../queries/sprintsQueries";
import { getWorkerAnalytics } from "../queries/workersQueries";
import { OrganizationAnalyticsData, ProjectAnalyticsData, SprintAnalyticsData, WorkerAnalyticsData } from "../../interfaces/analytics";
import { TaskStatusEnum } from "../../enums/statuses/TaskStatusEnum";
import { taskStatuses } from "../../sync/taskStatuses";
import { taskPriorities } from "../../sync/taskPriorities";
import { useGetAllTags } from "./tags";
import { TagOwnerEnum } from "../../enums/ownerEntities/TagOwnerEnum";
import { sprintStatuses } from "../../sync/sprintStatuses";

export const useProjectAnalytics = (projectId: number) => {
    const query = useQuery<ProjectAnalyticsData, Error>({
        queryKey: ['projectAnalytics', projectId],
        queryFn: () => getProjectAnalytics(projectId),
    });

    const { data: tags } = useGetAllTags(TagOwnerEnum.TASK);

    const { data, isLoading, error } = query;

    if (isLoading || error || !data) {
        return { ...query, chartData: null, totalTasks: 0, completedPercentage: 0, overduePercentage: 0 };
    }

    const totalTasks = Object.values(data.tasksByStatus).reduce((sum, value) => sum + value, 0);
    const completedTasks = data.tasksByStatus[TaskStatusEnum.COMPLETED] || 0;
    const completedPercentage = totalTasks > 0
        ? Math.round((completedTasks / totalTasks) * 100)
        : 0;
    const overduePercentage = totalTasks > 0
        ? Math.round((data.overdueTasks / totalTasks) * 100)
        : 0;

    // Get tag colors in the same order as labels
    const tagColors = Object.keys(data.tasksByTag).map(tagName => {
        const tag = tags?.find(t => t.name === tagName);
        return tag?.color || "#1890ff"; // default color if tag not found
    });

    const chartData = {
        status: {
            labels: Object.keys(data.tasksByStatus),
            datasets: [{
                data: Object.values(data.tasksByStatus),
                backgroundColor: taskStatuses.map(status => status.color),
                barThickness: 20,
            }],
        },
        priority: {
            labels: Object.keys(data.tasksByPriority),
            datasets: [{
                data: Object.values(data.tasksByPriority),
                backgroundColor: taskPriorities.map(priority => priority.color),
                barThickness: 20,
            }],
        },
        employee: {
            labels: data.tasksByEmployee.map(emp => emp.email),
            datasets: taskStatuses.map(status => ({
                label: status.name,
                data: data.tasksByEmployee.map(emp => emp.statuses[status.name] || 0),
                backgroundColor: status.color,
                barThickness: 20,
            })),
        },
        sprint: {
            labels: data.tasksBySprint.map(sprint => sprint.sprint.name),
            datasets: [{
                data: data.tasksBySprint.map(sprint => sprint.completedTasks),
                backgroundColor: data.tasksBySprint.map(sprint => {
                    const status = sprintStatuses.find(s => s.name === sprint.sprint.status.name);
                    return status?.color || "#0000FF"; // default color if status not found
                }),
                barThickness: 20,
            }],
        },
        tag: {
            labels: Object.keys(data.tasksByTag),
            datasets: [{
                data: Object.values(data.tasksByTag),
                backgroundColor: tagColors,
                barThickness: 20,
            }],
        },
    };

    return { ...query, chartData, totalTasks, completedPercentage, overduePercentage };
};

export const useSprintAnalytics = (sprintId: number) => {
    const query = useQuery<SprintAnalyticsData, Error>({
        queryKey: ['sprintAnalytics', sprintId],
        queryFn: () => getSprintAnalytics(sprintId),
    });

    const { data: tags } = useGetAllTags(TagOwnerEnum.TASK);

    const { data, isLoading, error } = query;

    if (isLoading || error || !data) {
        return { ...query, chartData: null, totalTasks: 0, completedPercentage: 0, overduePercentage: 0 };
    }

    const totalTasks = data.totalTasks;
    const completedTasks = data.tasksByStatus[TaskStatusEnum.COMPLETED] || 0;
    const completedPercentage = totalTasks > 0
        ? Math.round((completedTasks / totalTasks) * 100)
        : 0;
    const overduePercentage = totalTasks > 0
        ? Math.round((data.overdueTasks / totalTasks) * 100)
        : 0;

    // Get tag colors in the same order as labels
    const tagColors = Object.keys(data.tasksByTag).map(tagName => {
        const tag = tags?.find(t => t.name === tagName);
        return tag?.color || "#1890ff"; // default color if tag not found
    });

    const chartData = {
        status: {
            labels: Object.keys(data.tasksByStatus),
            datasets: [{
                data: Object.values(data.tasksByStatus),
                backgroundColor: taskStatuses.map(status => status.color),
                barThickness: 20,
            }],
        },
        priority: {
            labels: Object.keys(data.tasksByPriority),
            datasets: [{
                label: "Задачи по приоритетам",
                data: Object.values(data.tasksByPriority),
                backgroundColor: taskPriorities.map(priority => priority.color),
            }],
        },
        tag: {
            labels: Object.keys(data.tasksByTag),
            datasets: [{
                label: "Задачи по тегам",
                data: Object.values(data.tasksByTag),
                backgroundColor: tagColors,
            }],
        },
        employee: {
            labels: data.tasksByEmployee.map(e => e.email),
            datasets: taskStatuses.map(status => ({
                label: status.name,
                data: data.tasksByEmployee.map(e => e.statuses[status.name] || 0),
                backgroundColor: status.color,
                barThickness: 20,
            })),
        },
    };

    return { ...query, chartData, totalTasks, completedPercentage, overduePercentage };
};

export const useWorkerAnalytics = (workerId: number) => {
    const query = useQuery<WorkerAnalyticsData, Error>({
        queryKey: ['workerAnalytics', workerId],
        queryFn: () => getWorkerAnalytics(workerId),
    });

    const { data: tags } = useGetAllTags(TagOwnerEnum.TASK);

    const { data, isLoading, error } = query;

    if (isLoading || error || !data) {
        return { ...query, chartData: null, totalTasks: 0, completedPercentage: 0, overduePercentage: 0 };
    }

    const totalTasks = Object.values(data.tasksByStatus).reduce((sum, value) => sum + value, 0);
    const completedTasks = data.tasksByStatus[TaskStatusEnum.COMPLETED] || 0;
    const completedPercentage = totalTasks > 0
        ? Math.round((completedTasks / totalTasks) * 100)
        : 0;
    const overduePercentage = totalTasks > 0
        ? Math.round((data.overdueTasks / totalTasks) * 100)
        : 0;

    // Get tag colors in the same order as labels
    const tagColors = Object.keys(data.tasksByTag).map(tagName => {
        const tag = tags?.find(t => t.name === tagName);
        return tag?.color || "#1890ff"; // default color if tag not found
    });

    const chartData = {
        status: {
            labels: Object.keys(data.tasksByStatus),
            datasets: [{
                data: Object.values(data.tasksByStatus),
                backgroundColor: taskStatuses.map(status => status.color),
                barThickness: 20,
            }],
        },
        priority: {
            labels: Object.keys(data.tasksByPriority),
            datasets: [{
                data: Object.values(data.tasksByPriority),
                backgroundColor: taskPriorities.map(priority => priority.color),
                barThickness: 20,
            }],
        },
        tag: {
            labels: Object.keys(data.tasksByTag),
            datasets: [{
                data: Object.values(data.tasksByTag),
                backgroundColor: tagColors,
                barThickness: 20,
            }],
        },
        projectStatus: {
            labels: data.tasksByProject.map(proj => proj.projectName),
            datasets: taskStatuses.map(status => ({
                label: status.name,
                data: data.tasksByProject.map(proj => proj.statuses[status.name] || 0),
                backgroundColor: status.color,
                barThickness: 20,
            })),
        },
    };

    return { ...query, chartData, totalTasks, completedPercentage, overduePercentage };
};

export const useOrganizationAnalytics = () => {
    const query = useQuery<OrganizationAnalyticsData, Error>({
        queryKey: ['organizationAnalytics'],
        queryFn: () => getOrganizationAnalytics(),
    });

    const { data: tags } = useGetAllTags(TagOwnerEnum.TASK);

    const { data, isLoading, error } = query;

    if (isLoading || error || !data) {
        return { ...query, chartData: null, totalTasks: 0, completedPercentage: 0, overduePercentage: 0 };
    }

    const totalTasks = Object.values(data.tasksByStatus).reduce((sum, val) => sum + val, 0);
    const completedTasks = data.tasksByStatus[TaskStatusEnum.COMPLETED] || 0;
    const completedPercentage = totalTasks > 0
        ? Math.round((completedTasks / totalTasks) * 100)
        : 0;
    const overduePercentage = totalTasks > 0
        ? Math.round((data.overdueTasks / totalTasks) * 100)
        : 0;

    // Get tag colors in the same order as labels
    const tagColors = Object.keys(data.tasksByTag).map(tagName => {
        const tag = tags?.find(t => t.name === tagName);
        return tag?.color || "#1890ff"; // default color if tag not found
    });

    const chartData = {
        status: {
            labels: Object.keys(data.tasksByStatus),
            datasets: [{
                data: Object.values(data.tasksByStatus),
                backgroundColor: taskStatuses.map(status => status.color),
                barThickness: 20,
            }],
        },
        priority: {
            labels: Object.keys(data.tasksByPriority),
            datasets: [{
                label: "Задачи по приоритетам",
                data: Object.values(data.tasksByPriority),
                backgroundColor: taskPriorities.map(priority => priority.color),
            }],
        },
        tag: {
            labels: Object.keys(data.tasksByTag),
            datasets: [{
                label: "Задачи по тегам",
                data: Object.values(data.tasksByTag),
                backgroundColor: tagColors,
            }],
        },
        project: {
            labels: data.tasksByProject.map(p => p.projectName),
            datasets: [
                {
                    label: "Завершено",
                    data: data.tasksByProject.map(p => p.completed),
                    backgroundColor: taskStatuses.find(s => s.name === TaskStatusEnum.COMPLETED)?.color || "#0000FF",
                    barThickness: 20,
                },
                {
                    label: "Просрочено",
                    data: data.tasksByProject.map(p => p.overdue),
                    backgroundColor: "#d4380d",
                    barThickness: 20,
                },
            ],
        },
        employee: {
            labels: data.tasksByEmployee.map(e => e.email),
            datasets: taskStatuses.map(status => ({
                label: status.name,
                data: data.tasksByEmployee.map(e => e.statuses[status.name] || 0),
                backgroundColor: status.color,
                barThickness: 20,
            })),
        },
    };

    return { ...query, chartData, totalTasks, completedPercentage, overduePercentage };
};