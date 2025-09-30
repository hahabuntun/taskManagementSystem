import { Divider, Flex, Tag, Typography } from "antd";
import { WorkerAvatar } from "../../../components/WorkerAvatar";
import { IProject } from "../../../interfaces/IProject";

interface IProjectDataComponentProps {
  project: IProject;
}

export const ProjectDataComponent = ({
  project,
}: IProjectDataComponentProps) => {
  return (
    <Flex
      vertical
      style={{
        marginRight: "1rem",
        marginBottom: "1rem",
        minWidth: "300px",
      }}
    >
      <Typography.Title
        style={{
          textAlign: "center",
        }}
        level={5}
      >
        Основные данные
      </Typography.Title>
      <Flex
        justify="space-between"
        gap={"1rem"}
        style={{ padding: "0.2rem" }}
        align="center"
      >
        <p>Статус:</p>
        <Tag style={{ marginRight: 0 }} color={project.status.color}>
          <Typography.Text style={{ color: "black" }}>
            {project.status.name}
          </Typography.Text>
        </Tag>
      </Flex>
      <hr />
      <Flex
        justify="space-between"
        gap={"1rem"}
        style={{ padding: "0.2rem" }}
        align="center"
      >
        <p>Дата создания проекта:</p>
        <Typography.Text style={{ color: "gray" }}>
          {project?.createdAt.format("DD.MM.YYYY")}
        </Typography.Text>
      </Flex>
      <hr />

      <Flex
        justify="space-between"
        gap={"1rem"}
        style={{ padding: "0.2rem" }}
        align="center"
      >
        <Typography.Text style={{ marginRight: "1rem" }}>
          Менеджер:
        </Typography.Text>
        <WorkerAvatar size="small" worker={project.manager} />
      </Flex>
      <hr />
      <div style={{ padding: "0.2rem" }}>
        <Flex justify="space-between" align="center">
          <Typography.Text style={{ marginRight: "1rem" }}>
            Теги:
          </Typography.Text>
        </Flex>
      </div>

      <hr />

      <Flex
        justify="space-between"
        gap={"1rem"}
        style={{ padding: "0.2rem" }}
        align="center"
      >
        <p>Сотрудников в проекте:</p>
        <Typography.Text style={{ color: "gray" }}>
          {project.members.length}
        </Typography.Text>
      </Flex>

      <hr />

      <Divider />
    </Flex>
  );
};
