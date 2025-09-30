import { Typography, Divider, Flex } from "antd";
import { useQueryClient } from "@tanstack/react-query";
import { DndProvider, useDrop } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import { ITask } from "../../../../interfaces/ITask";
import useApplicationStore from "../../../../stores/applicationStore";
import { BoardTaskCard } from "../../../boards/board/BoardTaskCard";
import { taskPriorities } from "../../../../sync/taskPriorities";
import { useEditTaskData } from "../../../../api/hooks/tasks";
import { FieldsSelector } from "../../../FieldsSelector";

interface PriorityColumnProps {
  priority: string;
  tasks: ITask[];
  moveTask: (taskId: number, toColumn: string) => void;
  isDragDisabled: boolean;
}

const PriorityColumn = ({
  priority,
  tasks,
  moveTask,
  isDragDisabled,
}: PriorityColumnProps) => {
  const { isDarkMode } = useApplicationStore();

  const [{ isOver }, drop] = useDrop({
    accept: "TASK",
    drop: (item: { id: number; columnName: string }) => {
      if (item.columnName !== priority) {
        moveTask(item.id, priority);
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
        {priority} ({tasks.length})
      </Typography.Title>
      <Divider style={{ background: isDarkMode ? "#404040" : "#e8e8e8" }} />
      {tasks.map((task) => (
        <BoardTaskCard
          key={task.id}
          task={task}
          columnName={priority}
          moveTask={moveTask}
          isDragDisabled={isDragDisabled}
        />
      ))}
    </div>
  );
};

interface PriorityBoardViewProps {
  tasks: ITask[];
  isDragDisabled?: boolean;
}

export const PriorityBoardView = ({
  tasks,
  isDragDisabled = false,
}: PriorityBoardViewProps) => {
  const queryClient = useQueryClient();
  const editTask = useEditTaskData();

  const moveTask = (taskId: number, priorityName: string) => {
    const task = tasks.find((t) => t.id === taskId);
    if (!task) return;

    const newPriority = taskPriorities.find((p) => p.name === priorityName);
    if (!newPriority) return;

    queryClient.setQueryData(
      ["tasks"], // Use generic key since no boardId
      (old: ITask[] | undefined) =>
        old?.map((t) => (t.id === taskId ? { ...t, priority: newPriority } : t))
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
        status: task.status,
        type: task.type,
        startDate: task.startDate,
        workers: task.workers,
        storyPoints: task.storyPoints,
        priority: newPriority,
      },
    });
  };

  const priorities = taskPriorities.map((p) => p.name);
  const columns = priorities.map((priority) => ({
    priority,
    tasks: tasks.filter((task) => task.priority.name === priority),
  }));

  return (
    <DndProvider backend={HTML5Backend}>
      <FieldsSelector />
      <Flex gap={16} style={{ overflowX: "auto", padding: 16 }}>
        {columns.map(({ priority, tasks }) => (
          <PriorityColumn
            key={priority}
            priority={priority}
            tasks={tasks}
            moveTask={moveTask}
            isDragDisabled={isDragDisabled}
          />
        ))}
      </Flex>
    </DndProvider>
  );
};
