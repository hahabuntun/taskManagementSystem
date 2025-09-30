import { Gantt, IGanttTask, WillowDark, Willow } from "wx-react-gantt";
import "wx-react-gantt/dist/gantt.css";
import "./gant.css";
import { useEffect, useRef, useMemo } from "react";

import dayjs from "dayjs";
import { ITask } from "../../../../interfaces/ITask";
import { TaskTypeEnum } from "../../../../enums/TaskTypeEnum";
import { TaskRelationshipTypeEnum } from "../../../../enums/TaskRelationshipTypeEnum";
import useApplicationStore from "../../../../stores/applicationStore";
import { DrawerEntityEnum } from "../../../../enums/DrawerEntityEnum";

export function convertTaskToGanttTask(task: ITask): IGanttTask {
  const start = task.startDate
    ? task.startDate.toDate()
    : task.createdAt.toDate();

  // Если endDate не указан, то end будет undefined
  let end: Date | undefined = task.endDate ? task.endDate.toDate() : undefined;

  // Если разница между start и end равна 0, добавляем 1 день к end
  if (
    end &&
    task.startDate &&
    task.endDate?.diff(task.startDate, "day") === 0
  ) {
    end = dayjs(end).add(1, "day").toDate();
  }

  // Рассчитываем duration как разницу между end и start в днях
  const duration =
    end && start
      ? Math.round(dayjs(end).diff(dayjs(start), "day")) // Разница в днях, округленная до целого
      : 1; // Если нет endDate или startDate, duration = 1

  return {
    id: task.id,
    start,
    end,
    duration, // Добавляем duration
    text: task.name,
    progress: task.progress,
    type: getGanttTaskType(task.type),
  };
}

function getGanttTaskType(taskType: TaskTypeEnum): IGanttTask["type"] {
  switch (taskType) {
    case TaskTypeEnum.MILESTONE:
      return "milestone";
    case TaskTypeEnum.SUMMARY_TASK:
      return "summary";
    default:
      return "task";
  }
}

const taskTypes = [
  { id: "task", label: "Задача" },
  { id: "summary", label: "Сводная задача" },
  { id: "milestone", label: "Веха" },
];

// Interface for links as expected by wx-react-gantt
interface IGanttLink {
  id: number | string;
  source: number | string;
  target: number | string;
  type: "s2s" | "s2e" | "e2s" | "e2e"; // Start-to-Start, Start-to-End, End-to-Start, End-to-End
}

interface IChartProps {
  tasks: ITask[];
  isDarkMode: boolean;
}

export const GanttChart = ({ tasks, isDarkMode }: IChartProps) => {
  // Memoize chartTasks to prevent unnecessary recalculations
  const chartTasks: IGanttTask[] = useMemo(
    () => tasks.map((task) => convertTaskToGanttTask(task)),
    [tasks]
  );

  // Generate links from task relationships
  const links: IGanttLink[] = useMemo(() => {
    const allLinks: IGanttLink[] = [];
    let linkId = 1; // Simple incrementing ID for links

    tasks.forEach((task) => {
      if (task.relationships) {
        task.relationships
          .filter((r) => r.relationType !== TaskRelationshipTypeEnum.SUBTASK) // Exclude subtasks
          .forEach((relationship) => {
            const linkType = mapRelationshipTypeToGanttType(
              relationship.relationType
            );
            allLinks.push({
              id: linkId++,
              source: task.id, // Source is the current task
              target: relationship.relatedTaskId, // Target is the related task
              type: linkType,
            });
          });
      }
    });

    return allLinks;
  }, [tasks]);

  const apiRef = useRef<any>(null);
  const { showDrawer } = useApplicationStore.getState();

  useEffect(() => {
    if (apiRef.current) {
      apiRef.current.intercept("select-task", (data: any) => {
        showDrawer(DrawerEntityEnum.TASK, data.id);
        return false; // Prevent default behavior
      });
    }
  }, [showDrawer]);

  const columns = [
    { id: "text", header: "Название", flexgrow: 2 },
    { id: "start", header: "Начало", flexgrow: 1, align: "center" },
    { id: "duration", header: "Длительность", align: "center", flexgrow: 1 },
  ];

  // Use a key that includes both tasks and isDarkMode to force re-render
  const ganttKey = useMemo(
    () =>
      JSON.stringify({
        isDarkMode,
        tasks: chartTasks.map((t) => ({
          id: t.id,
          start: t.start.toISOString(),
          end: t.end?.toISOString(),
        })),
        links, // Include links in the key to force re-render on link changes
      }),
    [chartTasks, isDarkMode, links]
  );

  // Helper function to map TaskRelationshipTypeEnum to Gantt link types
  function mapRelationshipTypeToGanttType(
    relationType: TaskRelationshipTypeEnum
  ): IGanttLink["type"] {
    switch (relationType) {
      case TaskRelationshipTypeEnum.START_START:
        return "s2s";
      case TaskRelationshipTypeEnum.START_FINISH:
        return "s2e";
      case TaskRelationshipTypeEnum.FINISH_START:
        return "e2s";
      case TaskRelationshipTypeEnum.FINISH_FINISH:
        return "e2e";
      default:
        return "e2s"; // Default to end-to-start if unknown
    }
  }

  if (isDarkMode) {
    return (
      <WillowDark>
        <div className="wx-willow-dark-theme gantt-container user-select">
          <Gantt
            key={ganttKey}
            readonly={true}
            columns={columns}
            taskTypes={taskTypes}
            init={(api: any) => (apiRef.current = api)}
            tasks={chartTasks}
            links={links} // Pass links to Gantt
          />
        </div>
      </WillowDark>
    );
  } else {
    return (
      <Willow>
        <div className="gantt-container user-select">
          <Gantt
            className={"user-select"}
            key={ganttKey}
            readonly={true}
            init={(api: any) => (apiRef.current = api)}
            tasks={chartTasks}
            links={links} // Pass links to Gantt
            columns={columns}
            taskTypes={taskTypes}
          />
        </div>
      </Willow>
    );
  }
};
