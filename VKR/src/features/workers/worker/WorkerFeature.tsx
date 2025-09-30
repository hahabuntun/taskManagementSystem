import {
  useDeleteWorker,
  useEditWorker,
  useGetWorker,
} from "../../../api/hooks/workers";
import { Button, Divider, Flex, Popover, Tabs, Tag, Typography } from "antd";
import { CheckCircleOutlined, MinusOutlined } from "@ant-design/icons";
import { WorkerProjects } from "./WorkerProjects";
import { RelatedWorkersFeature } from "./RelatedWorkersFeature";
import { FilesFeature } from "../../files/FilesFeature";
import { HistoryFeature } from "../../history/HistoryFeature";
import { WorkerAvatar } from "../../../components/WorkerAvatar";
import { useMemo } from "react";
import { FileOwnerEnum } from "../../../enums/ownerEntities/FileOwnerEnum";
import { HistoryOwnerEnum } from "../../../enums/ownerEntities/HistoryOwnerEnum";
import { DeleteButton } from "../../../components/buttons/DeleteButton";
import { EditButton } from "../../../components/buttons/EditButton";
import EmployeeAnalytics from "../../analytics/WorkerAnalytics";
import { EditWorkerForm } from "./EditWorkerForm";
import { IEditWorkerOptions } from "../../../api/options/editOptions/IEditWorkerOptions";
import { ChangeWorkerPasswordForm } from "./ChangeWorkerPassword";
import useApplicationStore from "../../../stores/applicationStore";

interface IWorkerFeatureProps {
  workerId: number;
}

export const WorkerFeature = ({ workerId }: IWorkerFeatureProps) => {
  const memoizedWorkerId = useMemo(() => workerId, [workerId]);

  const { hideDrawer } = useApplicationStore.getState();
  const { user } = useApplicationStore.getState();
  // Используем мемоизированный workerId для вызова useGetWorker
  const { data: worker, refetch } = useGetWorker(memoizedWorkerId);

  const editAsync = useEditWorker(() => refetch());
  const deleteAsync = useDeleteWorker(() => hideDrawer());

  if (!worker) return null;

  // Мемоизируем вкладки, чтобы они не пересоздавались при каждом ререндере
  const tabItems = [
    {
      key: "Projects",
      label: "Проекты",
      children: <WorkerProjects workerId={workerId} />,
    },
    {
      key: "Subordinates",
      label: "Подчиненные",
      children: (
        <RelatedWorkersFeature
          relationType="subordinates"
          workerId={workerId}
        />
      ),
    },
    {
      key: "Directors",
      label: "Начальники",
      children: (
        <RelatedWorkersFeature relationType="directors" workerId={workerId} />
      ),
    },
    {
      key: "Files",
      label: "Файлы",
      children: (
        <FilesFeature ownerId={workerId} ownerType={FileOwnerEnum.WORKER} />
      ),
    },
    {
      key: "Analytics",
      label: "Аналитика",
      children: <EmployeeAnalytics employeeId={workerId} />,
    },
    {
      key: "History",
      label: "История",
      children: (
        <HistoryFeature
          ownerId={workerId}
          ownerType={HistoryOwnerEnum.WORKER}
        />
      ),
    },
  ];

  return (
    <>
      {/* Верхняя секция с информацией о сотруднике */}
      <Flex justify="start" gap={"2rem"} align="center">
        <WorkerAvatar size="small" worker={worker} />
        <Flex vertical>
          <Typography.Text strong>
            {`${worker.firstName} ${worker.secondName} ${worker.thirdName}`}
          </Typography.Text>
          <Typography.Text type="secondary">
            Email: {worker.email}
          </Typography.Text>
          <Typography.Text type="secondary">
            Дата добавления: {worker?.createdAt.format("DD.MM.YYYY")}
          </Typography.Text>
          <Typography.Text type="secondary">
            Статус сотрудника:{" "}
            <Tag color={worker.status.color}>
              <Typography.Text style={{ color: "black" }}>
                {worker.status.name}
              </Typography.Text>
            </Tag>
          </Typography.Text>
          <Typography.Text type="secondary">
            Должность: {worker?.workerPosition.title}
          </Typography.Text>

          <Flex>
            <Typography.Text type="secondary" style={{ marginRight: "0.5rem" }}>
              Является администратором
            </Typography.Text>
            {worker.isAdmin ? (
              <CheckCircleOutlined style={{ color: "green" }} />
            ) : (
              <MinusOutlined style={{ color: "#dc4446" }} />
            )}
          </Flex>
          <Flex>
            <Typography.Text type="secondary" style={{ marginRight: "0.5rem" }}>
              Является менеджером
            </Typography.Text>
            {worker.isManager ? (
              <CheckCircleOutlined style={{ color: "green" }} />
            ) : (
              <MinusOutlined style={{ color: "#dc4446" }} />
            )}
          </Flex>
        </Flex>
      </Flex>

      <Flex wrap style={{ marginTop: "1rem" }}>
        <DeleteButton
          style={{ marginRight: "1rem" }}
          text="Удалить сотрудника"
          itemId={worker.id}
          onClick={() => deleteAsync({ workerId })}
        />
        {user?.isAdmin && (
          <Popover
            trigger={"click"}
            content={
              <EditWorkerForm
                worker={worker}
                onEditItem={(itemId: number, options: IEditWorkerOptions) =>
                  editAsync({ workerId: itemId, options })
                }
              />
            }
          >
            <EditButton
              style={{ marginRight: "1rem" }}
              text="Изменить данные"
              itemId={worker.id}
            />
          </Popover>
        )}

        {(user?.id === worker.id || user?.isAdmin) && (
          <Popover
            trigger={"click"}
            content={<ChangeWorkerPasswordForm worker={worker} />}
          >
            <Button size="small" style={{ marginRight: "1rem" }}>
              Сменить пароль
            </Button>
          </Popover>
        )}
      </Flex>
      {/* Разделительная линия */}
      <Divider />
      {/* Вкладки с дополнительной информацией */}
      <Tabs
        style={{ marginBottom: "1rem" }}
        destroyInactiveTabPane={true}
        defaultActiveKey="Projects"
        items={tabItems}
      />
    </>
  );
};
