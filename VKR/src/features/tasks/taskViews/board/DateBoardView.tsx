import React from "react";
import { Typography, Divider, Flex } from "antd";
import dayjs from "dayjs";
import { DndProvider } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import useApplicationStore from "../../../../stores/applicationStore";
import { ITask } from "../../../../interfaces/ITask";
import { BoardTaskCard } from "../../../boards/board/BoardTaskCard";
import { FieldsSelector } from "../../../FieldsSelector";

interface DeadlineColumnProps {
  category: string;
  tasks: ITask[];
}

const DeadlineColumn: React.FC<DeadlineColumnProps> = ({ category, tasks }) => {
  const { isDarkMode } = useApplicationStore();

  const columnBackground = isDarkMode ? "#2d2d2d" : "#f5f5f5";
  const textColor = isDarkMode ? "#ffffff" : "#000000";

  return (
    <div
      style={{
        width: "290px",
        minHeight: "70vh",
        background: columnBackground,
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
        {category} ({tasks.length})
      </Typography.Title>
      <Divider style={{ background: isDarkMode ? "#404040" : "#e8e8e8" }} />
      {tasks.map((task) => (
        <BoardTaskCard
          key={task.id}
          task={task}
          columnName={category}
          moveTask={() => {}}
          isDragDisabled={true}
        />
      ))}
    </div>
  );
};

interface DateBoardViewProps {
  tasks: ITask[];
}

export const DateBoardView: React.FC<DateBoardViewProps> = ({ tasks }) => {
  const getDeadlineCategory = (endDate: dayjs.Dayjs | undefined): string => {
    if (!endDate) return "Без дедлайна";
    const today = dayjs();
    if (endDate.isBefore(today, "day")) return "Просрочено";
    if (endDate.isSame(today, "day")) return "Сегодня";
    if (endDate.isSame(today, "week")) return "На этой неделе";
    return "Будущее";
  };

  const categories = [
    "Просрочено",
    "Сегодня",
    "На этой неделе",
    "Будущее",
    "Без дедлайна",
  ];
  const columns = categories.map((category) => ({
    category,
    tasks: tasks.filter(
      (task) => getDeadlineCategory(task.endDate) === category
    ),
  }));

  return (
    <DndProvider backend={HTML5Backend}>
      <FieldsSelector />
      <Flex gap={16} style={{ overflowX: "auto", padding: 16 }}>
        {columns.map(({ category, tasks }) => (
          <DeadlineColumn key={category} category={category} tasks={tasks} />
        ))}
      </Flex>
    </DndProvider>
  );
};
