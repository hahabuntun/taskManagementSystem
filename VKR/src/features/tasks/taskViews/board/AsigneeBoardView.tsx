import { useMemo } from "react";
import { Typography, Divider, Flex } from "antd";
import { DndProvider, useDrop } from "react-dnd";
import { HTML5Backend } from "react-dnd-html5-backend";
import { IWorkerFields } from "../../../../interfaces/IWorkerFields";
import { ITask } from "../../../../interfaces/ITask";
import useApplicationStore from "../../../../stores/applicationStore";
import { BoardTaskCard } from "../../../boards/board/BoardTaskCard";
import { useUpdateTaskResponsibleWorker } from "../../../../api/hooks/tasks";
import { FieldsSelector } from "../../../FieldsSelector";
import { WorkerAvatar } from "../../../../components/WorkerAvatar";

interface AssigneeColumnProps {
  assignee: IWorkerFields | null;
  tasks: ITask[];
  moveTask: (taskId: number, toColumn: string) => void;
  isDragDisabled: boolean;
}

const AssigneeColumn = ({
  assignee,
  tasks,
  moveTask,
  isDragDisabled,
}: AssigneeColumnProps) => {
  const { isDarkMode } = useApplicationStore();

  const [{ isOver }, drop] = useDrop({
    accept: "TASK",
    drop: (item: { id: number; columnName: string }) => {
      const targetColumn = assignee?.email || "Без исполнителя";
      if (item.columnName !== targetColumn) {
        moveTask(item.id, targetColumn);
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
      <Flex
        align="center"
        justify="center"
        gap={8}
        style={{ padding: "8px 0" }}
      >
        {assignee ? (
          <>
            <WorkerAvatar size="default" worker={assignee} />
            <Typography.Title
              style={{
                textAlign: "center",
                color: textColor,
                margin: 0,
              }}
              level={5}
            >
              {assignee.email}
            </Typography.Title>
          </>
        ) : (
          <Typography.Title
            style={{
              textAlign: "center",
              color: textColor,
              margin: 0,
            }}
            level={5}
          >
            Не назначен ответственный ({tasks.length})
          </Typography.Title>
        )}
      </Flex>
      <Divider style={{ background: isDarkMode ? "#404040" : "#e8e8e8" }} />
      {tasks.map((task) => (
        <BoardTaskCard
          key={task.id}
          task={task}
          columnName={assignee?.email || "Без исполнителя"}
          moveTask={moveTask}
          isDragDisabled={isDragDisabled}
        />
      ))}
    </div>
  );
};

interface AssigneeBoardViewProps {
  tasks: ITask[];
  isDragDisabled?: boolean;
}

export const AssigneeBoardView = ({
  tasks,
  isDragDisabled = false,
}: AssigneeBoardViewProps) => {
  const updateResponsibleWorkerAsync = useUpdateTaskResponsibleWorker();

  const getResponsibleWorker = (task: ITask): IWorkerFields | null => {
    const responsible = task.workers.find((w) => w.isResponsible)?.workerData;
    return responsible ? { ...responsible } : null;
  };

  const moveTask = (taskId: number, toColumn: string) => {
    const task = tasks.find((t) => t.id === taskId);
    if (!task) return;

    const toAssignee =
      tasks
        .flatMap((t) => t.workers.map((w) => w.workerData))
        .find((w) => w.email === toColumn) || null;

    updateResponsibleWorkerAsync({
      taskId,
      workerId: toAssignee?.id ?? null,
    });
  };

  const assignees = useMemo(() => {
    const uniqueAssigneesMap = new Map<string, IWorkerFields>();
    tasks.forEach((task) => {
      const worker = getResponsibleWorker(task);
      if (worker && !uniqueAssigneesMap.has(worker.email)) {
        uniqueAssigneesMap.set(worker.email, worker);
      }
    });

    const uniqueAssignees = Array.from(uniqueAssigneesMap.values()).sort(
      (a, b) =>
        `${a.firstName} ${a.secondName || ""}`.localeCompare(
          `${b.firstName} ${b.secondName || ""}`
        )
    );
    return [null, ...uniqueAssignees];
  }, [tasks]);

  const columns = useMemo(
    () =>
      assignees.map((assignee) => ({
        assignee,
        tasks: tasks.filter((task) => {
          const worker = getResponsibleWorker(task);
          return worker?.email === assignee?.email || (!worker && !assignee);
        }),
      })),
    [tasks, assignees]
  );

  return (
    <DndProvider backend={HTML5Backend}>
      <FieldsSelector />
      <Flex gap={16} style={{ overflowX: "auto", padding: 16 }}>
        {columns.map(({ assignee, tasks }) => (
          <AssigneeColumn
            key={assignee?.email ?? "no-assignee"}
            assignee={assignee}
            tasks={tasks}
            moveTask={moveTask}
            isDragDisabled={isDragDisabled}
          />
        ))}
      </Flex>
    </DndProvider>
  );
};
