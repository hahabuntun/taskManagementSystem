import { Divider, Flex, Modal, Tabs, TabsProps, Typography } from "antd";
import { RelatedTasksFeature } from "./RelatedTasksFeature";
import { TaskCommentsFeature } from "./TaskCommentsFeature";
import { HistoryFeature } from "../../history/HistoryFeature";
import { FilesFeature } from "../../files/FilesFeature";
import { TaskDetails } from "./TaskDetails";
import { ITask } from "../../../interfaces/ITask";
import { FileOwnerEnum } from "../../../enums/ownerEntities/FileOwnerEnum";
import { HistoryOwnerEnum } from "../../../enums/ownerEntities/HistoryOwnerEnum";
import { LinksFeature } from "../../links/LinksFeature";
import { LinkOwnerEnum } from "../../../enums/ownerEntities/LinkOwnerEnum";
import { SubscriptionButton } from "../../../components/buttons/SubscriptionButton";
import { AddButton } from "../../../components/buttons/AddButton";
import { EditButton } from "../../../components/buttons/EditButton";
import { DeleteButton } from "../../../components/buttons/DeleteButton";
import { CreateTemplateFromTask } from "../taskTemplates/CreateTemplateFromTast";
import { EditTaskForm } from "./EditTaskForm";
import { useDeleteTask } from "../../../api/hooks/tasks";
import { useState } from "react";
import useApplicationStore from "../../../stores/applicationStore";
import { NotificationOwnerEnum } from "../../../enums/ownerEntities/NotificationOwnerEnum";

interface ITasksFeatureProps {
  task: ITask;
}

export const TaskFeature = ({ task }: ITasksFeatureProps) => {
  const { user, hideDrawer } = useApplicationStore.getState();
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const deleteAsync = useDeleteTask(() => hideDrawer());
  const [isCreateTemplateModalOpen, setIsCreateTemplateModalOpen] =
    useState<boolean>();

  const items: TabsProps["items"] = [
    {
      key: "main",
      label: "Главная",
      children: <TaskDetails task={task} />,
    },
    {
      key: "relatedTasks",
      label: "Связанные задачи",
      children: (
        <RelatedTasksFeature taskId={task.id} projectId={task.project.id} />
      ),
    },
    {
      key: "comments",
      label: "Комментарии",
      children: (
        <TaskCommentsFeature
          taskId={task.id}
          currentUser={{ firstName: "ivean", id: 1, lastName: "ivan" }}
        />
      ),
    },
    {
      key: "files",
      label: "Файлы",
      children: (
        <FilesFeature ownerId={task.id} ownerType={FileOwnerEnum.TASK} />
      ),
    },
    {
      key: "links",
      label: "Ссылки",
      children: (
        <LinksFeature ownerId={task.id} ownerType={LinkOwnerEnum.TASK} />
      ),
    },
    {
      key: "history",
      label: "История",
      children: (
        <HistoryFeature ownerId={task.id} ownerType={HistoryOwnerEnum.TASK} />
      ),
    },
  ];
  return (
    <>
      <Typography.Title level={3}>{task.name}</Typography.Title>

      <Flex wrap gap={"0.5rem"} justify="start">
        <SubscriptionButton
          style={{ marginRight: "1rem" }}
          entityId={task.id}
          entityType={NotificationOwnerEnum.TASK}
        />
        <DeleteButton
          style={{ marginRight: "1rem" }}
          onClick={() => deleteAsync({ task: task })}
          text="Удалить задачу"
          itemId={task.id}
        />
        <EditButton
          style={{ marginRight: "1rem" }}
          onClick={() => setIsEditModalOpen(true)}
          text="Изменить задачу"
          itemId={task.id}
        />
        <AddButton
          style={{ marginRight: "1rem" }}
          onClick={() => setIsCreateTemplateModalOpen(true)}
          text="Создать шаблон"
        />
      </Flex>
      <Divider />
      <Tabs items={items} />

      <Modal
        title="Создать шаблон по задаче"
        open={isCreateTemplateModalOpen}
        onCancel={() => setIsCreateTemplateModalOpen(false)}
        footer={null}
        width={600}
      >
        <CreateTemplateFromTask task={task} />
      </Modal>

      <Modal
        title="Редактирование задачи"
        open={isEditModalOpen}
        destroyOnClose
        onCancel={() => setIsEditModalOpen(false)}
        footer={null}
        style={{ minWidth: "800px", marginTop: "1rem" }}
      >
        {user && (
          <EditTaskForm
            onUpdated={() => {
              setIsEditModalOpen(false);
            }}
            user={user}
            projectId={task.project.id}
            taskId={task.id}
          />
        )}
      </Modal>
    </>
  );
};
