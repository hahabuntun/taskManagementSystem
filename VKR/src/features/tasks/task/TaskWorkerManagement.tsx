import { useState, useEffect } from "react";
import { Select, Space, Tag, Button, Checkbox, notification } from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import { TaskWorkerTypeEnum } from "../../../enums/TaskWorkerTypeEnum";
import { WorkerAvatar } from "../../../components/WorkerAvatar";
import {
  useGetTask,
  useGetAllTaskTakersForWorker,
  useAddTaskExecutor,
  useRemoveTaskExecutor,
  useUpdateTaskExecutorResponsible,
  useAddTaskObserver,
  useRemoveTaskObserver,
} from "../../../api/hooks/tasks";
import useApplicationStore from "../../../stores/applicationStore";
import { ITaskWorkerForm } from "../../../interfaces/ITaskWorker";

interface TaskWorkerManagerProps {
  taskId: number;
  projectId: number;
}

export const TaskWorkerManager = ({
  taskId,
  projectId,
}: TaskWorkerManagerProps) => {
  const [taskWorkers, setTaskWorkers] = useState<ITaskWorkerForm[]>([]);
  const { user } = useApplicationStore.getState();
  const { data: task, isLoading: taskLoading } = useGetTask(taskId);
  const { data: workers, isLoading: workersLoading } =
    useGetAllTaskTakersForWorker(projectId, user?.id ?? -1);
  const addExecutorMutation = useAddTaskExecutor();
  const removeExecutorMutation = useRemoveTaskExecutor();
  const updateResponsibleMutation = useUpdateTaskExecutorResponsible();
  const addObserverMutation = useAddTaskObserver();
  const removeObserverMutation = useRemoveTaskObserver();

  // Initialize task workers from task data
  useEffect(() => {
    if (task?.workers) {
      const executors = task.workers
        .filter((w) => w.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR)
        .map((w) => ({
          workerId: w.workerData.id,
          taskWorkerType: TaskWorkerTypeEnum.EXECUTOR,
          isResponsible: w.isResponsible || false,
        }));
      const observers = task.workers
        .filter((w) => w.taskWorkerType === TaskWorkerTypeEnum.VIEWER)
        .map((w) => ({
          workerId: w.workerData.id,
          taskWorkerType: TaskWorkerTypeEnum.VIEWER,
          isResponsible: false,
        }));
      setTaskWorkers([...executors, ...observers]);
    }
  }, [task]);

  const handleAddWorker = async (
    workerId: number | null,
    role: TaskWorkerTypeEnum
  ) => {
    if (!workerId) return;

    const newWorker: ITaskWorkerForm = {
      workerId,
      taskWorkerType: role,
      isResponsible: false,
    };
    const updatedWorkers = [...taskWorkers, newWorker];
    setTaskWorkers(updatedWorkers);

    try {
      if (role === TaskWorkerTypeEnum.EXECUTOR) {
        await addExecutorMutation.mutateAsync({
          taskId,
          executor: { workerId, isResponsible: false },
        });
      } else {
        await addObserverMutation.mutateAsync({
          taskId,
          observer: { workerId },
        });
      }
      notification.success({ message: "Сотрудник добавлен" });
    } catch {
      notification.error({ message: "Ошибка при добавлении сотрудника" });
      setTaskWorkers(taskWorkers); // Revert on failure
    }
  };

  const handleRemoveWorker = async (workerId: number) => {
    const worker = taskWorkers.find((w) => w.workerId === workerId);
    if (!worker) return;

    const updatedWorkers = taskWorkers.filter((w) => w.workerId !== workerId);
    setTaskWorkers(updatedWorkers);

    try {
      if (worker.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR) {
        await removeExecutorMutation.mutateAsync({ taskId, workerId });
      } else {
        await removeObserverMutation.mutateAsync({ taskId, workerId });
      }
      notification.success({ message: "Сотрудник удален" });
    } catch {
      notification.error({ message: "Ошибка при удалении сотрудника" });
      setTaskWorkers(taskWorkers); // Revert on failure
    }
  };

  const handleToggleResponsible = async (
    workerId: number,
    checked: boolean
  ) => {
    const updatedWorkers = taskWorkers.map((w) => ({
      ...w,
      isResponsible:
        w.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR &&
        w.workerId === workerId
          ? checked
          : w.isResponsible, // Only update the selected worker, preserve others
    }));

    // Check if any executor will remain responsible after the update
    const hasResponsible = updatedWorkers.some(
      (w) => w.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR && w.isResponsible
    );

    setTaskWorkers(updatedWorkers);

    try {
      await updateResponsibleMutation.mutateAsync({
        taskId,
        workerId: checked ? workerId : null, // Send null if unchecking and no other responsible
      });
      notification.success({
        message:
          hasResponsible || checked
            ? "Ответственный обновлен"
            : "Ответственный удален",
      });
    } catch (error: any) {
      notification.error(
        error.response?.data?.message || "Ошибка при обновлении ответственного"
      );
      setTaskWorkers(taskWorkers); // Revert on failure
    }
  };

  const handleWorkerChange = (workerId: number | null) => {
    handleAddWorker(workerId, TaskWorkerTypeEnum.EXECUTOR);
  };

  const handleObserverChange = (workerId: number | null) => {
    handleAddWorker(workerId, TaskWorkerTypeEnum.VIEWER);
  };

  if (taskLoading || workersLoading) {
    return <div>Загрузка...</div>;
  }

  // Combine workers with current user, avoiding duplicates
  const availableWorkers =
    user && workers
      ? [
          ...(workers.some((w) => w.id === user.id) ? [] : [user]), // Add user if not in workers
          ...workers,
        ].filter((w) => !taskWorkers.some((tw) => tw.workerId === w.id)) // Exclude assigned workers
      : workers?.filter(
          (w) => !taskWorkers.some((tw) => tw.workerId === w.id)
        ) || [];

  return (
    <Space direction="vertical" style={{ width: "100%" }}>
      <Space wrap>
        <Select
          placeholder="Добавить исполнителя"
          onChange={handleWorkerChange}
          style={{ width: "200px" }}
          allowClear
        >
          {availableWorkers.map((worker) => (
            <Select.Option key={worker.id} value={worker.id}>
              {worker.email}
            </Select.Option>
          ))}
        </Select>
        <Select
          placeholder="Добавить наблюдателя"
          onChange={handleObserverChange}
          style={{ width: "200px" }}
          allowClear
        >
          {availableWorkers.map((worker) => (
            <Select.Option key={worker.id} value={worker.id}>
              {worker.email}
            </Select.Option>
          ))}
        </Select>
      </Space>

      <Space direction="vertical" style={{ width: "100%" }}>
        {taskWorkers.map((tw) => {
          const worker =
            workers?.find((w) => w.id === tw.workerId) ||
            (tw.workerId === user?.id ? user : null);
          return (
            <Space key={tw.workerId} wrap>
              <Space>
                {worker && <WorkerAvatar worker={worker} size="small" />}
                <Tag
                  color={
                    tw.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR
                      ? "blue"
                      : "gray"
                  }
                >
                  {tw.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR
                    ? "Исполнитель"
                    : "Наблюдатель"}
                  {tw.isResponsible && " - Ответственный"}
                </Tag>
              </Space>
              {tw.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR && (
                <Checkbox
                  checked={tw.isResponsible}
                  onChange={(e) =>
                    handleToggleResponsible(tw.workerId, e.target.checked)
                  }
                >
                  Ответственный
                </Checkbox>
              )}
              <Button
                size="small"
                danger
                icon={<DeleteOutlined />}
                onClick={() => handleRemoveWorker(tw.workerId)}
              />
            </Space>
          );
        })}
      </Space>
    </Space>
  );
};
