import { Typography, Divider, Flex } from "antd";
import { useQueryClient } from "@tanstack/react-query";
import { DndProvider, useDrop } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import { ITask } from "../../../../interfaces/ITask";
import useApplicationStore from "../../../../stores/applicationStore";
import { BoardTaskCard } from "../../../boards/board/BoardTaskCard";
import { useEditTaskData } from "../../../../api/hooks/tasks";
import { taskStatuses } from "../../../../sync/taskStatuses";
import { FieldsSelector } from "../../../FieldsSelector";

interface StatusColumnProps {
  status: string;
  tasks: ITask[];
  moveTask: (taskId: number, toColumn: string) => void;
  isDragDisabled: boolean;
}

const StatusColumn = ({
  status,
  tasks,
  moveTask,
  isDragDisabled,
}: StatusColumnProps) => {
  const { isDarkMode } = useApplicationStore();

  const [{ isOver }, drop] = useDrop({
    accept: "TASK",
    drop: (item: { id: number; columnName: string }) => {
      if (item.columnName !== status) {
        moveTask(item.id, status);
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
        level={4}
      >
        {status} ({tasks.length})
      </Typography.Title>
      <Divider style={{ background: isDarkMode ? "#404040" : "#e8e8e8" }} />
      {tasks.map((task) => (
        <BoardTaskCard
          key={task.id}
          task={task}
          columnName={status}
          moveTask={moveTask}
          isDragDisabled={isDragDisabled}
        />
      ))}
    </div>
  );
};

interface StatusBoardViewProps {
  tasks: ITask[];
  isDragDisabled?: boolean;
}

export const StatusBoardView = ({
  tasks,
  isDragDisabled = false,
}: StatusBoardViewProps) => {
  const queryClient = useQueryClient();
  const editTask = useEditTaskData();

  const moveTask = (taskId: number, statusName: string) => {
    const task = tasks.find((t) => t.id === taskId);
    if (!task) return;

    const newStatus = taskStatuses.find((s) => s.name === statusName);
    if (!newStatus) return;

    queryClient.setQueryData(
      ["tasks"], // Use generic key since no boardId
      (old: ITask[] | undefined) =>
        old?.map((t) => (t.id === taskId ? { ...t, status: newStatus } : t))
    );

    editTask({
      taskId,
      options: {
        projectId: task.project.id,
        sprintId: task.sprint?.id,
        description: task.description,
        endDate: task.endDate,
        name: task.name,
        progress: task.progress,
        status: newStatus,
        type: task.type,
        startDate: task.startDate,
        workers: task.workers,
        storyPoints: task.storyPoints,
        priority: task.priority,
      },
    });
  };

  const statuses = taskStatuses.map((s) => s.name);
  const columns = statuses.map((status) => ({
    status,
    tasks: tasks.filter((task) => task.status.name === status),
  }));

  return (
    <DndProvider backend={HTML5Backend}>
      <FieldsSelector />
      <Flex gap={16} style={{ overflowX: "auto", padding: 16 }}>
        {columns.map(({ status, tasks }) => (
          <StatusColumn
            key={status}
            status={status}
            tasks={tasks}
            moveTask={moveTask}
            isDragDisabled={isDragDisabled}
          />
        ))}
      </Flex>
    </DndProvider>
  );
};
