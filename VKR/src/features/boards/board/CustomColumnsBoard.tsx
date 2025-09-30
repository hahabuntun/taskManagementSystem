import { useState } from "react";
import { Typography, Button, Input, Modal, Divider, Space, Flex } from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import { useQueryClient } from "@tanstack/react-query";
import { DndProvider, useDrop } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import { BoardTaskCard } from "./BoardTaskCard";
import {
  useGetCustomBoardTasks,
  useGetCustomBoardColumns,
  useAddCustomBoardColumn,
  useChangeTaskColumn,
  useDeleteTaskFromBoard,
  useDeleteCustomBoardColumn,
} from "../../../api/hooks/boards";
import { FieldsSelector } from "../../FieldsSelector";
import { IBoardTaskWithDetails } from "../../../interfaces/IBoardTask";
import useApplicationStore from "../../../stores/applicationStore";

interface CustomColumnProps {
  column: string | undefined;
  tasks: IBoardTaskWithDetails[];
  moveTask: (taskId: number, toColumn: string | undefined) => void;
  isDragDisabled: boolean;
  boardId: number;
}

const CustomColumn = ({
  column,
  tasks,
  moveTask,
  isDragDisabled,
  boardId,
}: CustomColumnProps) => {
  const { isDarkMode } = useApplicationStore();
  const deleteColumn = useDeleteCustomBoardColumn();
  const deleteTask = useDeleteTaskFromBoard();

  const [{ isOver }, drop] = useDrop({
    accept: "TASK",
    drop: (item: { id: number; columnName: string | undefined }) => {
      if (item.columnName !== column) {
        moveTask(item.id, column);
      }
    },
    collect: (monitor) => ({ isOver: monitor.isOver() }),
  });

  const columnBackground = isDarkMode ? "#2d2d2d" : "#f5f5f5";
  const hoverBackground = isDarkMode ? "#404040" : "#e6f7ff";
  const textColor = isDarkMode ? "#ffffff" : "#000000";

  const handleDeleteColumn = async () => {
    if (column) {
      await deleteColumn({ boardId, columnName: column });
    }
  };

  return (
    <div
      ref={drop}
      className="board-column"
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
      <Space
        align="baseline"
        style={{
          width: "100%",
          justifyContent: "center",
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
          {column || "Не назначена"} ({tasks.length})
        </Typography.Title>
        {column && !isDragDisabled && (
          <Button
            type="text"
            danger
            icon={<DeleteOutlined />}
            onClick={handleDeleteColumn}
            title="Удалить колонку"
          />
        )}
      </Space>
      <Divider style={{ background: isDarkMode ? "#404040" : "#e8e8e8" }} />
      {tasks.map((boardTask) => (
        <BoardTaskCard
          key={boardTask.taskId}
          task={boardTask.task}
          columnName={column || "Не назначена"}
          moveTask={(taskId) => moveTask(taskId, column)}
          isDragDisabled={isDragDisabled}
          onDelete={() => deleteTask({ boardId, taskId: boardTask.taskId })}
        />
      ))}
    </div>
  );
};

interface CustomColumnsBoardProps {
  boardId: number;
  isDragDisabled?: boolean;
}

export const CustomColumnsBoard = ({
  boardId,
  isDragDisabled = false,
}: CustomColumnsBoardProps) => {
  const queryClient = useQueryClient();
  const { data: boardTasks = [], isLoading: isTasksLoading } =
    useGetCustomBoardTasks(boardId);
  const { data: customColumns = [], isLoading: isColumnsLoading } =
    useGetCustomBoardColumns(boardId);
  const addCustomBoardColumn = useAddCustomBoardColumn();
  const changeTaskColumn = useChangeTaskColumn();
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [newColumnName, setNewColumnName] = useState("");

  const moveTask = async (taskId: number, columnName: string | undefined) => {
    const boardTask = boardTasks.find((bt) => bt.taskId === taskId);
    if (!boardTask) return;

    // Optimistically update the UI
    queryClient.setQueryData(
      ["customBoard", boardId, "tasks"],
      (old: IBoardTaskWithDetails[] | undefined) =>
        old?.map((bt) =>
          bt.taskId === taskId
            ? { ...bt, customBoardColumnName: columnName }
            : bt
        )
    );

    try {
      // Update the backend
      await changeTaskColumn({ boardId, taskId, columnName });
    } catch (error) {
      // Revert optimistic update on error
      queryClient.setQueryData(
        ["customBoard", boardId, "tasks"],
        (old: IBoardTaskWithDetails[] | undefined) =>
          old?.map((bt) =>
            bt.taskId === taskId
              ? {
                  ...bt,
                  customBoardColumnName: boardTask.customBoardColumnName,
                }
              : bt
          )
      );
      throw error;
    }
  };

  const handleAddColumn = () => {
    if (
      newColumnName &&
      !customColumns.some((col) => col.name === newColumnName)
    ) {
      addCustomBoardColumn({
        boardId,
        options: { name: newColumnName, order: customColumns.length + 1 },
      });
      setNewColumnName("");
      setIsModalVisible(false);
    }
  };

  if (isTasksLoading || isColumnsLoading) return <div>Загрузка...</div>;

  // Sort columns: "Uncategorized" first, then custom columns by order
  const columns = [
    {
      column: undefined,
      tasks: boardTasks.filter((bt) => !bt.customBoardColumnName),
    },
    ...customColumns
      .sort((a, b) => a.order - b.order)
      .map((col) => ({
        column: col.name,
        tasks: boardTasks.filter((bt) => bt.customBoardColumnName === col.name),
      })),
  ];

  return (
    <DndProvider backend={HTML5Backend}>
      <FieldsSelector />
      {!isDragDisabled && (
        <Button
          onClick={() => setIsModalVisible(true)}
          style={{ marginLeft: "1rem" }}
        >
          Добавить колонку
        </Button>
      )}
      <div style={{ padding: 16 }}>
        <Flex gap={"16px"} style={{ overflowX: "auto", marginTop: 16 }}>
          {columns.map(({ column, tasks }) => (
            <CustomColumn
              key={column || "Не назначена"}
              column={column}
              tasks={tasks}
              moveTask={moveTask}
              isDragDisabled={isDragDisabled}
              boardId={boardId}
            />
          ))}
        </Flex>
        <Modal
          title="Добавление колонки"
          open={isModalVisible}
          onOk={handleAddColumn}
          onCancel={() => setIsModalVisible(false)}
        >
          <Input
            value={newColumnName}
            onChange={(e) => setNewColumnName(e.target.value)}
            placeholder="Введите название колонки"
          />
        </Modal>
      </div>
    </DndProvider>
  );
};
