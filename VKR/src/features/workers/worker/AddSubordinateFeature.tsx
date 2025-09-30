import { AddButton } from "../../../components/buttons/AddButton";
import { ProfileButton } from "../../../components/buttons/ProfileButton";
import { IWorker } from "../../../interfaces/IWorker";
import {
  Collapse,
  Divider,
  Flex,
  Space,
  Table,
  TableProps,
  Tag,
  Typography,
} from "antd";
import { CheckCircleOutlined, MinusOutlined } from "@ant-design/icons";
import { WorkerAvatar } from "../../../components/WorkerAvatar";
import { FilterWorkersForm } from "./../FilterWorkersForm";
import { IWorkerFilterOptions } from "../../../api/options/filterOptions/IWorkerFilterOptions";
import { useGetAvailableSubordinates } from "../../../api/hooks/workers";
import { IAddSubordinateOptions } from "../../../api/options/createOptions/IAddSubordinatesOptions";
import { DrawerEntityEnum } from "../../../enums/DrawerEntityEnum";
import useApplicationStore from "../../../stores/applicationStore";

const { Text } = Typography;

interface IAddSubordinateFeatureProps {
  workerId: number;
  onAdded?: (options: IAddSubordinateOptions) => void;
}

export const AddSubordinateFeature = ({
  onAdded,
  workerId,
}: IAddSubordinateFeatureProps) => {
  const { showDrawer } = useApplicationStore.getState();

  const { data, setFilters, refetch } = useGetAvailableSubordinates(workerId);

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
            <AddButton
              onClick={() => {
                onAdded?.({ workerId: worker.id });
                refetch();
              }}
              text="Добавить"
            />
          </Flex>
        );
      },
    },
    {
      title: "Сотрудник",
      key: "worker",
      render: (_, worker) => {
        return <WorkerAvatar size="small" worker={worker} />;
      },
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
      render: (date) => date.format("DD.MM.YYYY"), // Format date for readability
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

  const handleFilterApply = (filters: IWorkerFilterOptions) => {
    setFilters(filters);
  };

  return (
    <div style={{ height: "80vh", overflowY: "auto", marginBottom: "2rem" }}>
      <Collapse
        style={{ margin: "1rem auto" }}
        items={[
          {
            key: "filters",
            label: "Фильтры",
            children: <FilterWorkersForm onFilterApply={handleFilterApply} />,
          },
        ]}
      />

      <Divider />
      <Table
        rowKey={"id"}
        pagination={false}
        style={{ marginBottom: "1rem" }}
        columns={columns}
        dataSource={data}
      />
    </div>
  );
};
