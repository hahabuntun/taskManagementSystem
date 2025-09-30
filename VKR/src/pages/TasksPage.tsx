import { PageOwnerEnum } from "../enums/ownerEntities/PageOwnerEnum";
import { TasksFeature } from "../features/tasks/TasksFeature";
import useApplicationStore from "../stores/applicationStore";
import { Tabs, TabsProps } from "antd";

export const TasksPage = () => {
  const { user } = useApplicationStore.getState();

  if (user) {
    const items: TabsProps["items"] = [
      {
        key: "creator",
        label: "Я создатель",
        children: (
          <TasksFeature
            entityId={user.id}
            entityType={PageOwnerEnum.WORKER}
            howRelatesToTask="creator"
          />
        ),
      },
      {
        key: "executor",
        label: "Я исполнитель",
        children: (
          <TasksFeature
            entityId={user.id}
            entityType={PageOwnerEnum.WORKER}
            howRelatesToTask="executor"
          />
        ),
      },
      {
        key: "responsible",
        label: "Я ответственный",
        children: (
          <TasksFeature
            entityId={user.id}
            entityType={PageOwnerEnum.WORKER}
            howRelatesToTask="responsible"
          />
        ),
      },
      {
        key: "viewer",
        label: "Я наблюдатель",
        children: (
          <TasksFeature
            entityId={user.id}
            entityType={PageOwnerEnum.WORKER}
            howRelatesToTask="viewer"
          />
        ),
      },
    ];
    return (
      <>
        <Tabs items={items} />
      </>
    );
  }
};
