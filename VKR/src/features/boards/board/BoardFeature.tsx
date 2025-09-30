import { Flex, Tabs, TabsProps, Typography } from "antd";
import { IBoard } from "../../../interfaces/IBoard";
import { BoardTasksFeature } from "./BoardTasksFeature";
import { HistoryFeature } from "../../history/HistoryFeature";
import { HistoryOwnerEnum } from "../../../enums/ownerEntities/HistoryOwnerEnum";
import { SubscriptionButton } from "../../../components/buttons/SubscriptionButton";
import { NotificationOwnerEnum } from "../../../enums/ownerEntities/NotificationOwnerEnum";

interface IBoardFeatureProps {
  board: IBoard;
}

export const BoardFeature = ({ board }: IBoardFeatureProps) => {
  const tabs: TabsProps["items"] = [
    {
      key: "boardTasks",
      label: "Задачи",
      children: (
        <BoardTasksFeature boardBasic={board.boardBasis} boardId={board.id} />
      ),
    },
    {
      key: "sprintHistory",
      label: "История",
      children: (
        <HistoryFeature ownerId={board.id} ownerType={HistoryOwnerEnum.BOARD} />
      ),
    },
  ];
  return (
    <>
      <Flex align="baseline">
        <Typography.Title level={3} style={{ marginRight: "0.5rem" }}>
          Доска: {board.name}
        </Typography.Title>
        <SubscriptionButton
          entityId={board.id}
          entityType={NotificationOwnerEnum.BOARD}
        />
      </Flex>

      <Tabs items={tabs} />
    </>
  );
};
