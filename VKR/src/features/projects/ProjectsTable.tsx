import { IProject } from "../../interfaces/IProject";
import { Flex, Table, TableProps, Tag, Tooltip, Typography } from "antd";
import { LinkButton } from "../../components/LinkButton";
import { useNavigate } from "react-router-dom";
import { WorkerAvatar } from "../../components/WorkerAvatar";
import { DeleteButton } from "../../components/buttons/DeleteButton";
import { EditButton } from "../../components/buttons/EditButton";
import { ITag } from "../../interfaces/ITag";
import useApplicationStore from "../../stores/applicationStore";
import { useState } from "react";
import { Modal } from "antd";
import { EditProjectForm } from "./project/EditProjectForm";
import { useDeleteProject } from "../../api/hooks/projects";

interface IProjectsTableProps {
  items: IProject[];
}

export const ProjectsTable = ({ items }: IProjectsTableProps) => {
  const navigate = useNavigate();
  const { user } = useApplicationStore.getState();
  const deleteAsync = useDeleteProject();

  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedProject, setSelectedProject] = useState<IProject | null>(null);

  const handleEditClick = (project: IProject) => {
    setSelectedProject(project);
    setIsEditModalOpen(true);
  };

  // Функция для ограничения количества отображаемых тегов
  const renderTags = (tags: ITag[]) => {
    const maxVisibleTags = 3;
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

  const columns: TableProps<IProject>["columns"] = [
    {
      title: "Действия",
      key: "action",
      render: (_, record) => (
        <Flex style={{ maxWidth: "150px" }} vertical gap={4}>
          {user && (user?.isAdmin || user.id === record.manager.id) && (
            <>
              <DeleteButton
                itemId={record.id}
                onClick={() => deleteAsync({ projectId: record.id })}
                text="Удалить"
              />
              <EditButton
                itemId={record.id}
                text="Изменить"
                onClick={() => handleEditClick(record)}
              />
            </>
          )}
        </Flex>
      ),
    },
    {
      title: "Название",
      key: "projectName",
      render: (_, record) => (
        <LinkButton handleClicked={() => navigate(`/projects/${record.id}`)}>
          {record.name}
        </LinkButton>
      ),
    },
    {
      title: "Менеджер",
      key: "manager",
      render: (_, record) => (
        <WorkerAvatar size="small" worker={record.manager} />
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
      title: "Тэги",
      key: "tags",
      render: (_, record) => renderTags(record.tags),
    },
    {
      title: "Дата создания",
      key: "createdAt",
      render: (_, record) => {
        return <Flex>{record.createdAt.format("DD.MM.YYYY")}</Flex>;
      },
    },
    {
      title: "Сотрудники",
      key: "workers",
      render: (_, record) => {
        const maxVisibleAvatars = 6;
        const visibleWorkers = record.members.slice(0, maxVisibleAvatars) || [];
        const hiddenWorkers = record.members.slice(maxVisibleAvatars) || [];

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
  ];

  return (
    <>
      <Modal
        title="Редактирование проекта"
        open={isEditModalOpen}
        destroyOnClose
        onCancel={() => setIsEditModalOpen(false)}
        footer={null}
        style={{ minWidth: "800px", marginTop: "1rem" }}
      >
        {selectedProject && <EditProjectForm project={selectedProject} />}
      </Modal>
      <Table
        rowKey={"id"}
        pagination={false}
        columns={columns}
        dataSource={items}
      />
    </>
  );
};
