import { Typography, Divider, Flex } from "antd";
import { useQueryClient } from "@tanstack/react-query";
import { DndProvider, useDrop } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import { ITask } from "../../../interfaces/ITask";
import { useChangeTaskSprint, useGetTasks } from "../../../api/hooks/tasks";
import { useGetSprints } from "../../../api/hooks/sprints";
import { FieldsSelector } from "../../FieldsSelector";
import { PageOwnerEnum } from "../../../enums/ownerEntities/PageOwnerEnum";
import useApplicationStore from "../../../stores/applicationStore";
import { BoardTaskCard } from "./BoardTaskCard";

interface SprintColumnProps {
  sprint: string | undefined;
  tasks: ITask[];
  moveTask: (taskId: number, toColumn: string | undefined) => void;
  isDragDisabled: boolean;
  onDelete?: (taskId: number) => void;
}

const SprintColumn = ({
  sprint,
  tasks,
  moveTask,
  isDragDisabled,
  onDelete,
}: SprintColumnProps) => {
  const { isDarkMode } = useApplicationStore();

  const sumOfStoryPoints = tasks
    .map((item) => item.storyPoints)
    .filter((num) => num !== null && num !== undefined)
    .reduce((sum, num) => sum + num, 0);

  const [{ isOver }, drop] = useDrop({
    accept: "TASK",
    drop: (item: { id: number; columnName: string }) => {
      if (item.columnName !== sprint) {
        moveTask(item.id, sprint);
      }
    },
    collect: (monitor) => ({ isOver: monitor.isOver() }),
  });

  const columnBackground = isDarkMode ? "#2d2d2d" : "#f5f5f5";
  const hoverBackground = isDarkMode ? "#404040" : "#e6f7ff";
  const textColor = isDarkMode ? "#ffffff" : "#000000";

  return (
    <div
      ref={drop}
      style={{
        width: "290px",
        minHeight: "70vh",
        background: isOver ? hoverBackground : columnBackground,
        padding: 8,
        borderRadius: 4,
        boxShadow: isDarkMode
          ? "0 2px 8px rgba(0, 0, 0, 0.2)"
          : "0 2px 8px rgba(0, 0, 0, 0.1)",
      }}
    >
      <Typography.Title
        style={{
          textAlign: "center",
          color: textColor,
          margin: 0,
          padding: "8px 0",
        }}
        level={5}
      >
        {sprint || "Не назначен спринт"} ({tasks.length})
      </Typography.Title>

      <Typography.Text
        style={{
          textAlign: "center",
          margin: 0,
          display: "block",
        }}
        type="secondary"
      >
        Стори поинты: {sumOfStoryPoints}
      </Typography.Text>
      <Divider style={{ background: isDarkMode ? "#404040" : "#e8e8e8" }} />
      {tasks.length === 0 ? (
        <Typography.Text
          style={{ color: textColor, textAlign: "center", display: "block" }}
        >
          Нет задач
        </Typography.Text>
      ) : (
        tasks.map((task) => (
          <BoardTaskCard
            key={task.id}
            task={task}
            columnName={sprint || "Не назначен спринт"} // Fixed typo
            moveTask={moveTask}
            isDragDisabled={isDragDisabled}
            onDelete={onDelete}
          />
        ))
      )}
    </div>
  );
};

interface SprintBoardProps {
  projectId: number;
  isDragDisabled?: boolean;
}

export const SprintBoard = ({
  projectId,
  isDragDisabled = false,
}: SprintBoardProps) => {
  const queryClient = useQueryClient();
  const {
    data: tasksData,
    isLoading: isTasksLoading,
    error: tasksError,
  } = useGetTasks({
    entityId: projectId,
    pageType: PageOwnerEnum.PROJECT,
    enabled: true,
  });
  const {
    data: sprintsData,
    isLoading: isSprintsLoading,
    error: sprintsError,
  } = useGetSprints(PageOwnerEnum.PROJECT, projectId);
  const changeTaskSprint = useChangeTaskSprint();

  const sprints = sprintsData ?? [];

  const tasks = tasksData ?? [];

  const moveTask = (taskId: number, sprintName: string | undefined) => {
    const task = tasks.find((t) => t.id === taskId);
    if (!task) {
      return;
    }

    const newSprint =
      sprintName === "Не назначен спринт"
        ? undefined
        : sprints.find((s) => s.name === sprintName);

    queryClient.setQueryData(
      [PageOwnerEnum.PROJECT, "tasks", projectId],
      (old: ITask[] | undefined) =>
        old?.map((t) =>
          t.id === taskId
            ? {
                ...t,
                sprint: newSprint
                  ? { id: newSprint.id, name: newSprint.name }
                  : undefined,
              }
            : t
        )
    );

    changeTaskSprint({
      task,
      sprintId: newSprint?.id,
    });
  };

  if (isTasksLoading || isSprintsLoading) return <div>Загрузка...</div>;
  if (tasksError) return <div>Ошибка загрузки задач: {tasksError.message}</div>;
  if (sprintsError)
    return <div>Ошибка загрузки спринтов: {sprintsError.message}</div>;

  const columns = [
    {
      sprint: undefined,
      tasks: tasks.filter((task) => !task.sprint || !task.sprint.name), // Updated condition
    },
    ...sprints.map((sprint) => ({
      sprint: sprint.name,
      tasks: tasks.filter((task) => task.sprint?.name === sprint.name),
    })),
  ];

  return (
    <DndProvider backend={HTML5Backend}>
      <FieldsSelector />
      <Flex gap={16} style={{ overflowX: "auto", padding: 16 }}>
        {columns.map(({ sprint, tasks }) => (
          <SprintColumn
            key={sprint ?? "no-sprint"}
            sprint={sprint}
            tasks={tasks}
            moveTask={moveTask}
            isDragDisabled={isDragDisabled}
          />
        ))}
      </Flex>
    </DndProvider>
  );
};
