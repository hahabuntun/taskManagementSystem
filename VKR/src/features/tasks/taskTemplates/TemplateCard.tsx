import {
  Card,
  Progress,
  Space,
  Tag,
  Typography,
  Divider,
  Flex,
  Modal,
} from "antd";
import { ITaskTemplate } from "../../../interfaces/ITaskTemplate";
import { LinkButton } from "../../../components/LinkButton";
import { DrawerEntityEnum } from "../../../enums/DrawerEntityEnum";
import { DeleteButton } from "../../../components/buttons/DeleteButton";
import { EditButton } from "../../../components/buttons/EditButton";
import { AddButton } from "../../../components/buttons/AddButton";
import { CreateTaskFromTemplate } from "./CreateTaskFromTemplate";
import { CreateTemplateFromTemplate } from "./CreateTemplateFromTemplate";
import { EditTemplate } from "./EditTemplate";
import useApplicationStore from "../../../stores/applicationStore";
import { useState } from "react";
import { useDeleteTaskTemplate } from "../../../api/hooks/tasks";

interface TemplateCardProps {
  template: ITaskTemplate;
}

export const TemplateCard = ({ template }: TemplateCardProps) => {
  const deleteAsync = useDeleteTaskTemplate();
  const { user, showDrawer } = useApplicationStore.getState();

  // Состояния для модальных окон
  const [isCreateTaskModalVisible, setIsCreateTaskModalVisible] =
    useState(false);
  const [isCreateTemplateModalVisible, setIsCreateTemplateModalVisible] =
    useState(false);
  const [isEditTemplateModalVisible, setIsEditTemplateModalVisible] =
    useState(false);

  // Обработчики открытия модальных окон
  const showCreateTaskModal = () => setIsCreateTaskModalVisible(true);
  const showCreateTemplateModal = () => setIsCreateTemplateModalVisible(true);
  const showEditTemplateModal = () => setIsEditTemplateModalVisible(true);

  // Обработчики закрытия модальных окон
  const handleCancelCreateTask = () => setIsCreateTaskModalVisible(false);
  const handleCancelCreateTemplate = () =>
    setIsCreateTemplateModalVisible(false);
  const handleCancelEditTemplate = () => setIsEditTemplateModalVisible(false);

  // Обработчики отправки формы
  const handleCreateTask = () => {
    setIsCreateTaskModalVisible(false);
  };

  const handleCreateTemplate = () => {
    setIsCreateTemplateModalVisible(false);
  };

  const handleEditTemplate = () => {
    setIsEditTemplateModalVisible(false);
  };

  return (
    <Card
      style={{
        minWidth: "300px",
        borderRadius: "8px",
        boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
        margin: "16px 0",
      }}
    >
      <Space direction="vertical" size="small">
        <LinkButton
          handleClicked={() =>
            showDrawer(DrawerEntityEnum.TASK_TEMPLATE, template.id)
          }
        >
          {template.name}
        </LinkButton>
        {!!template.taskName && (
          <Typography.Text>
            Название задачи: {template.taskName}
          </Typography.Text>
        )}
        {!!template.description && (
          <Typography.Text>{template.description}</Typography.Text>
        )}

        {!!template.type && (
          <Typography.Text>Тип: {template.type}</Typography.Text>
        )}

        {!!template.storyPoints && (
          <Typography.Text>
            Количество сторипоинтов: {template.storyPoints ?? "Не указано"}
          </Typography.Text>
        )}

        {!!template.status && (
          <Typography.Text>
            <Tag color={template.status?.color}>
              <Typography.Text style={{ color: "black" }}>
                {template.status?.name}
              </Typography.Text>
            </Tag>
          </Typography.Text>
        )}

        {!!template.priority && (
          <Typography.Text>
            Приоритет: {template.priority?.name}
          </Typography.Text>
        )}

        {!!template.progress && (
          <Space>
            <Typography.Text>Прогресс:</Typography.Text>
            <Progress
              style={{ minWidth: "200px" }}
              percent={template.progress}
            />
          </Space>
        )}

        {template.tags.length > 0 && (
          <Space>
            {template.tags.map((tag) => (
              <Tag key={tag.name} color={tag.color}>
                <Typography.Text style={{ color: "black" }}>
                  {tag.name}
                </Typography.Text>
              </Tag>
            ))}
          </Space>
        )}
        <Typography.Text type="secondary">
          Создано: {template.createdAt.format("DD.MM.YYYY")}
        </Typography.Text>
      </Space>

      {/* Кнопки действий, выровненные справа */}
      <Divider />
      <Flex wrap justify="end" gap="16px" style={{ paddingTop: "8px" }}>
        <DeleteButton
          onClick={() => deleteAsync({ templateId: template.id })}
          text="Удалить шаблон"
          itemId={template.id}
        />
        <EditButton
          itemId={template.id}
          text="Изменить шаблон"
          onClick={showEditTemplateModal}
        />
        <AddButton
          text="Создать задачу по шаблону"
          onClick={showCreateTaskModal}
        />
        <AddButton
          text="Создать копию шаблона"
          onClick={showCreateTemplateModal}
        />
      </Flex>

      {/* Модальное окно для создания задачи */}
      {user && (
        <Modal
          title="Создать задачу на основе шаблона"
          open={isCreateTaskModalVisible}
          onCancel={handleCancelCreateTask}
          footer={null}
          width={600}
        >
          <CreateTaskFromTemplate
            template={template}
            user={user}
            onSubmit={handleCreateTask}
          />
        </Modal>
      )}

      {/* Модальное окно для создания копии шаблона */}
      <Modal
        title="Создать копию шаблона"
        open={isCreateTemplateModalVisible}
        onCancel={handleCancelCreateTemplate}
        footer={null}
        width={600}
      >
        <CreateTemplateFromTemplate
          template={template}
          onSubmit={handleCreateTemplate}
        />
      </Modal>

      {/* Модальное окно для редактирования шаблона */}
      <Modal
        title="Редактировать шаблон"
        open={isEditTemplateModalVisible}
        onCancel={handleCancelEditTemplate}
        footer={null}
        width={600}
      >
        <EditTemplate template={template} onSubmit={handleEditTemplate} />
      </Modal>
    </Card>
  );
};
