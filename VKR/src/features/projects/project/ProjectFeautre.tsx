import { Flex, Tabs, TabsProps, Typography } from "antd";
import { HistoryFeature } from "../../history/HistoryFeature";
import { FilesFeature } from "../../files/FilesFeature";
import { TasksFeature } from "../../tasks/TasksFeature";
import { BoardsFeature } from "../../boards/BoardsFeature";
import { SprintsFeature } from "../../sprints/SprintsFeature";
import { PageOwnerEnum } from "../../../enums/ownerEntities/PageOwnerEnum";
import { FileOwnerEnum } from "../../../enums/ownerEntities/FileOwnerEnum";
import { BoardOwnerEnum } from "../../../enums/ownerEntities/BoardOwnerEnum";
import { HistoryOwnerEnum } from "../../../enums/ownerEntities/HistoryOwnerEnum";
import ProjectMain from "./../project/ProjectMain";
import { useGetProject } from "../../../api/hooks/projects";
import { CalendarFeature } from "../../tasks/taskViews/calendar/CalendarFeature";
import { SubscriptionButton } from "../../../components/buttons/SubscriptionButton";
import { NotificationOwnerEnum } from "../../../enums/ownerEntities/NotificationOwnerEnum";
import ProjectAnalytics from "../../analytics/ProjectAnalytics";

interface IProjectFeatureProps {
  projectId: number;
}

export const ProjectFeature = ({ projectId }: IProjectFeatureProps) => {
  const { data } = useGetProject(projectId);
  if (data) {
    const tabs: TabsProps["items"] = [
      {
        key: "projectDashboard",
        label: "Главная",
        children: <ProjectMain project={data} />,
      },
      {
        key: "projectTasks",
        label: "Задачи",
        children: (
          <TasksFeature
            entityType={PageOwnerEnum.PROJECT}
            entityId={projectId}
          />
        ),
      },
      {
        key: "projectCalendar",
        label: "Календарь",
        children: (
          <CalendarFeature
            ownerId={projectId}
            ownerType={PageOwnerEnum.PROJECT}
          />
        ),
      },
      {
        key: "projectBoards",
        label: "Доски",
        children: (
          <BoardsFeature
            ownerId={projectId}
            ownerType={BoardOwnerEnum.PROJECT}
          />
        ),
      },
      {
        key: "projectSprints",
        label: "Спринты",
        children: (
          <SprintsFeature
            entityId={projectId}
            entityType={PageOwnerEnum.PROJECT}
          />
        ),
      },
      {
        key: "projectFiles",
        label: "Файлы",
        children: (
          <FilesFeature ownerId={projectId} ownerType={FileOwnerEnum.PROJECT} />
        ),
      },
      {
        key: "projectAnalytics",
        label: "Аналитика",
        children: <ProjectAnalytics projectId={projectId} />,
      },
      {
        key: "projectHistory",
        label: "История",
        children: (
          <HistoryFeature
            ownerId={projectId}
            ownerType={HistoryOwnerEnum.PROJECT}
          />
        ),
      },
    ];
    return (
      <>
        {/* Заголовок проекта */}
        <div
          style={{
            padding: "16px",
            borderRadius: "4px",
          }}
        >
          <Flex align="baseline">
            <Typography.Title style={{ marginRight: "0.5rem" }} level={3}>
              {data.name}
            </Typography.Title>
            <SubscriptionButton
              entityId={data.id}
              entityType={NotificationOwnerEnum.PROJECT}
            />
          </Flex>

          <Typography.Text type="secondary">{data.description}</Typography.Text>
          <Tabs items={tabs} />
        </div>
      </>
    );
  }
};
