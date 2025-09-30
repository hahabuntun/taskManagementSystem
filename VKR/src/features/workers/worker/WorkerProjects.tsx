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
import { IProject } from "../../../interfaces/IProject";
import { useGetProjects } from "../../../api/hooks/projects";
import { FilterProjectsForm } from "../../projects/FilterProjectsForm";
import { PageOwnerEnum } from "../../../enums/ownerEntities/PageOwnerEnum";
import { LinkButton } from "../../../components/LinkButton";
import { useNavigate } from "react-router-dom";

interface IWorkerProjectsProps {
  workerId: number;
}

export const WorkerProjects = ({ workerId }: IWorkerProjectsProps) => {
  const { data: items } = useGetProjects(workerId, PageOwnerEnum.WORKER);
  const navigate = useNavigate();

  const columns: TableProps<IProject>["columns"] = [
    {
      title: "Название проекта",
      key: "project",
      render: (_, project) => {
        return (
          <LinkButton handleClicked={() => navigate(`/projects/${project.id}`)}>
            {project.name}
          </LinkButton>
        );
      },
    },
    {
      title: "Статус проекта",
      key: "status",
      render: (_, project) => {
        return (
          <Tag color={project.status.color}>
            <Typography.Text style={{ color: "black" }}>
              {project.status.name}
            </Typography.Text>
          </Tag>
        );
      },
    },
    {
      title: "Дата добавления в проект",
      key: "createdAt",
      render: (_, project) => {
        return (
          <Flex>
            {project.members
              .find((memeber) => memeber.workerData.id === workerId)
              ?.createdAt.format("DD.MM.YYYY")}
          </Flex>
        );
      },
    },
  ];

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          Проекты сотрудника
        </Typography.Title>
      </Space>

      <Collapse
        style={{ margin: "1rem auto" }}
        items={[
          {
            key: "filters",
            label: "Фильтры",
            children: <FilterProjectsForm />,
          },
        ]}
      />

      <Divider />

      <Table
        rowKey={"id"}
        columns={columns}
        dataSource={items}
        pagination={false}
      />
    </>
  );
};
