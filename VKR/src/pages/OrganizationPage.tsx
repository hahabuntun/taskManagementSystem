import { HistoryFeature } from "../features/history/HistoryFeature";
import { useGetOrganizationData } from "../api/hooks/organization";
import { Divider, Flex, Tabs, TabsProps, Typography } from "antd";
import { FilesFeature } from "../features/files/FilesFeature";
import { PositionsFeature } from "../features/positions/PositionsFeature";
import { WorkersFeature } from "../features/workers/WorkersFeature";
import { ProjectsFeature } from "../features/projects/ProjectsFeature";
import { PageOwnerEnum } from "../enums/ownerEntities/PageOwnerEnum";
import { FileOwnerEnum } from "../enums/ownerEntities/FileOwnerEnum";
import { HistoryOwnerEnum } from "../enums/ownerEntities/HistoryOwnerEnum";
import { WorkerAvatar } from "../components/WorkerAvatar";
import { NotificationOwnerEnum } from "../enums/ownerEntities/NotificationOwnerEnum";
import { SubscriptionButton } from "../components/buttons/SubscriptionButton";
import OrganizationAnalytics from "../features/analytics/OrganizationAnalytics";

export const OrganizationPage = () => {
  const { data } = useGetOrganizationData();

  if (data) {
    const items: TabsProps["items"] = [
      {
        key: "orgProjects",
        label: "Проекты",
        children: (
          <ProjectsFeature
            entityId={data.id}
            entityType={PageOwnerEnum.ORGANIZATION}
          />
        ),
      },
      {
        key: "orgWorkers",
        label: "Сотрудники",
        children: <WorkersFeature />,
      },
      {
        key: "orgWorkerPositions",
        label: "Должности",
        children: <PositionsFeature />,
      },
      {
        key: "orgFiles",
        label: "Файлы",
        children: (
          <FilesFeature
            ownerId={data.id}
            ownerType={FileOwnerEnum.ORGANIZATION}
          />
        ),
      },
      {
        key: "orgAnalytics",
        label: "Аналитика",
        children: <OrganizationAnalytics />,
      },
      {
        key: "orgHistory",
        label: "История",
        children: (
          <HistoryFeature
            ownerId={data?.id}
            ownerType={HistoryOwnerEnum.ORGANIZATION}
          />
        ),
      },
    ];

    return (
      <div>
        {/* Блок с информацией об организации */}
        <Flex vertical style={{ marginBottom: "24px" }}>
          <Flex align="baseline">
            <Typography.Title style={{ marginRight: "0.5rem" }} level={3}>
              {data.name}
            </Typography.Title>
            <SubscriptionButton
              entityId={data.id}
              entityType={NotificationOwnerEnum.ORGANIZATION}
            />
          </Flex>
          <Typography.Text type="secondary">{data.description}</Typography.Text>
          <Typography.Text type="secondary">
            Дата создания: {data.createdAt.format("DD.MM.YYYY HH:mm")}
          </Typography.Text>
          <Flex gap="0.5rem">
            <Typography.Text type="secondary">Владелец:</Typography.Text>
            <WorkerAvatar size="small" worker={data.owner} />
          </Flex>
        </Flex>

        <Divider />

        {/* Вкладки */}
        <Tabs destroyInactiveTabPane={true} items={items} />
      </div>
    );
  } else {
    return <></>;
  }
};
