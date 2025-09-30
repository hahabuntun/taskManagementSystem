import { Tabs, TabsProps } from "antd";
import useApplicationStore from "../stores/applicationStore";
import { BoardsFeature } from "../features/boards/BoardsFeature";
import { BoardOwnerEnum } from "../enums/ownerEntities/BoardOwnerEnum";

export const BoardsPage = () => {
  const { user } = useApplicationStore.getState();
  if (user) {
    const items: TabsProps["items"] = [
      {
        key: "projectBoards",
        label: "Проектные доски",
        children: (
          <BoardsFeature
            ownerId={user.id}
            ownerType={BoardOwnerEnum.WORKER}
            type="project"
          />
        ),
      },
      {
        key: "personalBoards",
        label: "Личные доски",
        children: (
          <BoardsFeature
            ownerId={user.id}
            ownerType={BoardOwnerEnum.WORKER}
            type="personal"
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
