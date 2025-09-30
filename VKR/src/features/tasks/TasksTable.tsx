import {
  Button,
  Flex,
  Modal,
  Progress,
  Table,
  TableProps,
  Tag,
  Tooltip,
  Typography,
} from "antd";
import { ITask } from "../../interfaces/ITask";
import { LinkButton } from "../../components/LinkButton";
import { ITag } from "../../interfaces/ITag";
import { WorkerAvatar } from "../../components/WorkerAvatar";
import { DeleteButton } from "../../components/buttons/DeleteButton";
import { EditTaskForm } from "./task/EditTaskForm";
import { EditButton } from "../../components/buttons/EditButton";
import { IEditTaskOptions } from "../../api/options/editOptions/IEditTaskOptions";
import { DrawerEntityEnum } from "../../enums/DrawerEntityEnum";
import { useState } from "react";
import useApplicationStore from "../../stores/applicationStore";

interface ITasksListProps {
  tasks: ITask[];
  onDelete?: (item: ITask) => void;
  deleteText?: string;
  onSelect?: (item: ITask) => void;
  onEdit?: (item: ITask, options: IEditTaskOptions) => void;
}

export const TasksTable = ({
  tasks = [],
  onDelete,
  deleteText = "Удалить",
  onEdit,
  onSelect,
}: ITasksListProps) => {
  const { user, showDrawer } = useApplicationStore.getState();

  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedTask, setSelectedTask] = useState<ITask | null>(null);

  const safeTasks = tasks.filter(
    (task): task is ITask =>
      task !== undefined && task !== null && typeof task.id !== "undefined"
  );

  const handleEditClick = (task: ITask) => {
    setSelectedTask(task);
    setIsEditModalOpen(true);
  };

  // Функция для ограничения количества отображаемых тегов
  const renderTags = (tags: ITag[]) => {
    const maxVisibleTags = 3; // Максимальное количество видимых тегов
    if (!tags || tags.length === 0) return null;

    const visibleTags = tags.slice(0, maxVisibleTags);
    const hiddenTagsCount = tags.length - maxVisibleTags;

    return (
      <Tooltip
        title={tags.map((tag) => (
          <Tag key={tag.name} color={tag.color}>
            <Typography.Text style={{ color: "black" }}>
              {tag.name}
            </Typography.Text>
          </Tag>
        ))}
      >
        <Flex gap={4}>
          {visibleTags.map((tag) => (
            <Tag key={tag.name} color={tag.color}>
              <Typography.Text style={{ color: "black" }}>
                {tag.name}
              </Typography.Text>
            </Tag>
          ))}
          {hiddenTagsCount > 0 && (
            <Tag color="default">+{hiddenTagsCount} больше</Tag>
          )}
        </Flex>
      </Tooltip>
    );
  };

  const columns: TableProps<ITask>["columns"] = [
    {
      title: "Действия",
      key: "action",
      render: (_, record) => (
        <Flex style={{ maxWidth: "150px" }} vertical gap={4}>
          {onDelete && (
            <DeleteButton
              itemId={record.id}
              handleClicked={() => onDelete?.(record)}
              text={deleteText}
            />
          )}
          {onEdit && (
            <EditButton
              onClick={() => handleEditClick(record)}
              itemId={record.id}
              text="Изменить"
            />
          )}
          {onSelect && (
            <Button
              size="small"
              onClick={() => onSelect?.(record)}
              type="primary"
            >
              Выбрать
            </Button>
          )}
        </Flex>
      ),
    },
    {
      title: "Название",
      key: "name",
      render: (_, record) => (
        <LinkButton
          handleClicked={() => showDrawer(DrawerEntityEnum.TASK, record.id)}
        >
          {record.name}
        </LinkButton>
      ),
    },
    {
      title: "Статус",
      key: "status",
      render: (_, record) => (
        <Flex>
          <Tag color={record.status.color}>
            <Typography.Text style={{ color: "black" }}>
              {record.status.name}
            </Typography.Text>
          </Tag>
        </Flex>
      ),
    },
    {
      title: "Приоритет",
      key: "priority",
      render: (_, record) => (
        <Flex align="center" gap={8}>
          {/* Добавляем точку слева */}
          <span
            style={{
              width: "8px", // Размер точки
              height: "8px", // Размер точки
              borderRadius: "50%", // Сделаем круглой
              backgroundColor: record.priority.color, // Цвет из данных приоритета
            }}
          />
          {/* Название приоритета */}
          <span>{record.priority.name}</span>
        </Flex>
      ),
    },
    {
      title: "Тэги",
      key: "tags",
      render: (_, record) => renderTags(record.tags),
    },
    {
      title: "Прогресс",
      key: "progress",
      render: (_, record) => (
        <Progress
          percent={record.progress || 0} // Используйте поле progress из данных задачи
          status={record.progress >= 100 ? "success" : undefined} // Опционально: изменение статуса при достижении 100%
          style={{ width: "100px" }} // Установите желаемую ширину
        />
      ),
    },
    {
      title: "Сотрудники",
      key: "workers",
      render: (_, record) => {
        const maxVisibleAvatars = 6; // Максимальное количество видимых аватарок
        const visibleWorkers = record.workers.slice(0, maxVisibleAvatars) || [];
        const hiddenWorkers = record.workers.slice(maxVisibleAvatars) || [];

        return (
          <Tooltip
            title={
              hiddenWorkers.length > 0 && (
                <Flex>
                  {hiddenWorkers.map((worker) => (
                    <WorkerAvatar
                      key={worker.workerData.id}
                      size="small"
                      worker={worker.workerData}
                    />
                  ))}
                </Flex>
              )
            }
            placement="topRight"
            trigger="hover"
          >
            <Flex>
              {visibleWorkers.map((worker) => (
                <WorkerAvatar
                  key={worker.workerData.id}
                  size="small"
                  worker={worker.workerData}
                />
              ))}
              {hiddenWorkers.length > 0 && (
                <Tag style={{ alignContent: "center" }} color="default">
                  +{hiddenWorkers.length}
                </Tag>
              )}
            </Flex>
          </Tooltip>
        );
      },
    },
    {
      title: "Дедлайн",
      key: "endDate",
      render: (_, record) => {
        return <Flex>{record.endDate?.format("DD.MM.YYYY")}</Flex>;
      },
    },
    {
      title: "Проект",
      key: "project",
      render: (_, record) => {
        return (
          <LinkButton handleClicked={() => console.log()}>
            {record.project.name}
          </LinkButton>
        );
      },
    },
  ];

  return (
    <>
      <Modal
        title="Редактирование задачи"
        open={isEditModalOpen}
        destroyOnClose
        onCancel={() => setIsEditModalOpen(false)}
        footer={null}
        style={{ minWidth: "800px", marginTop: "1rem" }}
      >
        {selectedTask && user && (
          <EditTaskForm
            onUpdated={() => {
              setIsEditModalOpen(false);
            }}
            user={user}
            projectId={selectedTask.project.id}
            taskId={selectedTask.id}
          />
        )}
      </Modal>
      <Table
        style={{ overflowX: "scroll" }}
        pagination={false}
        rowKey={"id"}
        columns={columns}
        dataSource={safeTasks}
      />
    </>
  );
};
