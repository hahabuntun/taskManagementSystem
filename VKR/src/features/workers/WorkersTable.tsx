import { Flex, Table, TableProps, Typography, Space, Popover, Tag } from "antd";
import { CheckCircleOutlined, MinusOutlined } from "@ant-design/icons";
import { IWorker } from "../../interfaces/IWorker";
import { EditButton } from "../../components/buttons/EditButton";
import { ProfileButton } from "../../components/buttons/ProfileButton";
import { DeleteButton } from "../../components/buttons/DeleteButton";
import { WorkerAvatar } from "../../components/WorkerAvatar";
import { IEditWorkerOptions } from "../../api/options/editOptions/IEditWorkerOptions";
import useApplicationStore from "../../stores/applicationStore";
import { DrawerEntityEnum } from "../../enums/DrawerEntityEnum";
import { EditWorkerForm } from "./worker/EditWorkerForm";

const { Text } = Typography;

interface IWorkersTableProps {
  items: IWorker[];
  onEditItem?: (itemId: number, options: IEditWorkerOptions) => void;
  onDeleteItem?: (itemId: number) => void;
  isDeletable: boolean;
  isEditable: boolean;
}

export const WorkersTable = ({
  items,
  onDeleteItem,
  onEditItem,
  isDeletable = true,
  isEditable = true,
}: IWorkersTableProps) => {
  const { user, showDrawer } = useApplicationStore.getState();

  const columns: TableProps<IWorker>["columns"] = [
    {
      title: "Действия",
      key: "action",
      render: (_, worker) => {
        return (
          <Flex vertical gap={4}>
            <ProfileButton
              onClick={() => showDrawer(DrawerEntityEnum.WORKER, worker.id)}
              text="Профиль"
              itemId={worker.id}
            />
            {user?.isAdmin && isEditable && (
              <Popover
                trigger="click"
                destroyTooltipOnHide
                content={() => (
                  <EditWorkerForm worker={worker} onEditItem={onEditItem} />
                )}
              >
                <EditButton itemId={worker.id} text="Изменить" />
              </Popover>
            )}
            {user?.isAdmin && isDeletable && (
              <DeleteButton
                handleClicked={onDeleteItem}
                text="Удалить"
                itemId={worker.id}
              />
            )}
          </Flex>
        );
      },
    },
    {
      title: "Сотрудник",
      key: "worker",
      render: (_, worker) => <WorkerAvatar size="small" worker={worker} />,
    },
    {
      title: "Должность",
      key: "workerType",
      render: (_, worker) => <Text>{worker.workerPosition.title}</Text>,
    },
    {
      title: "Статус",
      key: "status",
      render: (_, worker) => (
        <Tag color={worker.status.color}>
          <Typography.Text style={{ color: "black" }}>
            {worker.status.name}
          </Typography.Text>
        </Tag>
      ),
    },
    {
      title: "Дата создания",
      key: "createdAt",
      dataIndex: "createdAt",
      render: (_, worker) => {
        return worker.createdAt.format("DD.MM.YYYY");
      },
    },
    {
      title: "Разрешения",
      key: "permissions",
      render: (_, worker) => (
        <Space direction="vertical" style={{ width: "100%" }}>
          <div
            style={{
              display: "flex",
              justifyContent: "start",
              alignItems: "center",
            }}
          >
            <Text style={{ marginRight: "0.5rem" }}>
              Является администратором
            </Text>
            {worker.isAdmin ? (
              <CheckCircleOutlined style={{ color: "green" }} />
            ) : (
              <MinusOutlined style={{ color: "#dc4446" }} />
            )}
          </div>
          <div
            style={{
              display: "flex",
              justifyContent: "start",
              alignItems: "center",
            }}
          >
            <Text style={{ marginRight: "0.5rem" }}>Является менеджером</Text>
            {worker.isManager ? (
              <CheckCircleOutlined style={{ color: "green" }} />
            ) : (
              <MinusOutlined style={{ color: "#dc4446" }} />
            )}
          </div>
        </Space>
      ),
    },
  ];
  return (
    <Table
      rowKey={"id"}
      columns={columns}
      dataSource={items}
      pagination={false}
    />
  );
};
