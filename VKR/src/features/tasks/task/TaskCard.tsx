import { Card, Flex, Progress, Space, Tag, Typography } from "antd";
import { ITask } from "../../../interfaces/ITask";
import { WorkerAvatar } from "../../../components/WorkerAvatar";
import { LinkButton } from "../../../components/LinkButton";
import { DeleteButton } from "../../../components/buttons/DeleteButton";
import { DrawerEntityEnum } from "../../../enums/DrawerEntityEnum";
import useApplicationStore from "../../../stores/applicationStore";
import { TaskWorkerTypeEnum } from "../../../enums/TaskWorkerTypeEnum";

interface ITaskCardProps {
  task: ITask;
  minWidth?: string;
  maxWidth?: string;
  onDelete?: (taskId: number) => void;
  fieldsToShow?: Record<string, boolean>;
}

export const TaskCard = ({
  task,
  minWidth = "250px",
  maxWidth,
  onDelete,
  fieldsToShow = {
    name: true,
    description: true,
    type: true,
    storyPoints: true,
    progress: true,
    createdAt: true,
    startDate: true,
    endDate: true,
    status: true,
    priority: true,
    project: true,
    sprint: true,
    creator: true,
    workers: true,
    viewers: true,
    responsible: true,
    tags: true,
  },
}: ITaskCardProps) => {
  const { showDrawer } = useApplicationStore.getState();

  const executors = task.workers.filter(
    (worker) => worker.taskWorkerType === TaskWorkerTypeEnum.EXECUTOR
  );
  const viewers = task.workers.filter(
    (worker) => worker.taskWorkerType === TaskWorkerTypeEnum.VIEWER
  );
  const responsible = task.workers.find((worker) => worker.isResponsible);

  return (
    <Card
      style={{
        minWidth: minWidth,
        maxWidth: maxWidth,
        borderRadius: "8px",
        boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
        overflow: "hidden",
      }}
    >
      <Flex
        vertical
        gap="8px"
        style={{
          marginBottom: "1rem",
          width: "100%",
          overflow: "hidden",
        }}
      >
        {/* Название задачи */}
        <LinkButton
          handleClicked={() => showDrawer(DrawerEntityEnum.TASK, task.id)}
        >
          {task.name}
        </LinkButton>

        {/* Описание */}
        {fieldsToShow.description && (
          <Typography.Text
            type="secondary"
            style={{ fontSize: "16px", lineHeight: "1.6", maxWidth: "100%" }}
          >
            {task.description}
          </Typography.Text>
        )}

        {/* Остальные поля */}
        {[
          ...(fieldsToShow.type
            ? [{ label: "Тип:", value: task.type, key: "type" }]
            : []),
          ...(fieldsToShow.storyPoints
            ? [
                {
                  label: "Стори поинты:",
                  value: task.storyPoints,
                  key: "storyPoints",
                },
              ]
            : []),
          ...(fieldsToShow.progress
            ? [
                {
                  label: "Прогресс:",
                  value: (
                    <Progress
                      percent={task.progress}
                      size="small"
                      style={{ width: "100%", maxWidth: "150px" }}
                    />
                  ),
                  key: "progress",
                },
              ]
            : []),
          ...(fieldsToShow.createdAt
            ? [
                {
                  label: "Создано:",
                  value: task.createdAt?.format("DD.MM.YYYY"),
                  key: "createdAt",
                },
              ]
            : []),
          ...(fieldsToShow.startDate
            ? [
                {
                  label: "Начало:",
                  value: task.startDate?.format("DD.MM.YYYY"),
                  key: "startDate",
                },
              ]
            : []),
          ...(fieldsToShow.endDate
            ? [
                {
                  label: "Окончание:",
                  value: task.endDate?.format("DD.MM.YYYY"),
                  key: "endDate",
                },
              ]
            : []),
          ...(fieldsToShow.status
            ? [{ label: "Статус:", value: task.status.name, key: "status" }]
            : []),
          ...(fieldsToShow.priority
            ? [
                {
                  label: "Приоритет:",
                  value: task.priority.name,
                  key: "priority",
                },
              ]
            : []),
          ...(fieldsToShow.project
            ? [{ label: "Проект:", value: task.project.name, key: "project" }]
            : []),
          ...(fieldsToShow.sprint && task.sprint
            ? [{ label: "Спринт:", value: task.sprint.name, key: "sprint" }]
            : []),
          ...(fieldsToShow.creator
            ? [
                {
                  label: "Создатель:",
                  value: (
                    <Space>
                      <WorkerAvatar size="small" worker={task.creator} />
                    </Space>
                  ),
                  key: "creator",
                },
              ]
            : []),
          ...(fieldsToShow.workers
            ? [
                {
                  label: "Исполнители:",
                  value: (
                    <Space wrap style={{ maxWidth: "100%" }}>
                      {executors.length > 0 ? (
                        executors
                          .slice(0, 3)
                          .map((worker) => (
                            <WorkerAvatar
                              key={worker.workerData.id}
                              size="small"
                              worker={worker.workerData}
                            />
                          ))
                      ) : (
                        <Typography.Text type="secondary">
                          Нет исполнителей
                        </Typography.Text>
                      )}
                      {executors.length > 3 && (
                        <Typography.Text type="secondary">
                          +{executors.length - 3}
                        </Typography.Text>
                      )}
                    </Space>
                  ),
                  key: "workers",
                },
              ]
            : []),
          ...(fieldsToShow.viewers
            ? [
                {
                  label: "Наблюдатели:",
                  value: (
                    <Space wrap style={{ maxWidth: "100%" }}>
                      {viewers.length > 0 ? (
                        viewers
                          .slice(0, 3)
                          .map((worker) => (
                            <WorkerAvatar
                              key={worker.workerData.id}
                              size="small"
                              worker={worker.workerData}
                            />
                          ))
                      ) : (
                        <Typography.Text type="secondary">
                          Нет наблюдателей
                        </Typography.Text>
                      )}
                      {viewers.length > 3 && (
                        <Typography.Text type="secondary">
                          +{viewers.length - 3}
                        </Typography.Text>
                      )}
                    </Space>
                  ),
                  key: "viewers",
                },
              ]
            : []),
          ...(fieldsToShow.responsible
            ? [
                {
                  label: "Ответственный:",
                  value: responsible ? (
                    <Space>
                      <WorkerAvatar
                        size="small"
                        worker={responsible.workerData}
                      />
                    </Space>
                  ) : (
                    <Typography.Text type="secondary">
                      Не назначен
                    </Typography.Text>
                  ),
                  key: "responsible",
                },
              ]
            : []),
          ...(fieldsToShow.tags
            ? [
                {
                  label: "Теги:",
                  value: (
                    <Space wrap style={{ maxWidth: "100%" }}>
                      {task.tags.slice(0, 3).map((tag) => (
                        <Tag
                          key={tag.name}
                          color={tag.color}
                          style={{
                            borderRadius: "4px",
                            maxWidth: "100px",
                            overflow: "hidden",
                            textOverflow: "ellipsis",
                          }}
                        >
                          <Typography.Text ellipsis style={{ color: "black" }}>
                            {tag.name}
                          </Typography.Text>
                        </Tag>
                      ))}
                      {task.tags.length > 3 && (
                        <Typography.Text type="secondary">
                          +{task.tags.length - 3}
                        </Typography.Text>
                      )}
                    </Space>
                  ),
                  key: "tags",
                },
              ]
            : []),
        ].map((item, index) => (
          <div key={index} style={{ display: "flex", gap: "8px" }}>
            <Typography.Text>{item.label}</Typography.Text>
            <Typography.Text type="secondary" style={{ flex: 1 }}>
              {item.value}
            </Typography.Text>
          </div>
        ))}
      </Flex>
      {onDelete && (
        <DeleteButton
          style={{ width: "100%" }}
          onClick={() => onDelete?.(task.id)}
          itemId={task.id}
          text="Убрать с доски"
        />
      )}
    </Card>
  );
};
