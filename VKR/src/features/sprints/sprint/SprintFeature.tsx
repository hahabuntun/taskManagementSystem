import { Flex, Tabs, TabsProps, Tag, Typography } from "antd";
import { HistoryFeature } from "../../history/HistoryFeature";
import { HistoryOwnerEnum } from "../../../enums/ownerEntities/HistoryOwnerEnum";
import { useGetSprint } from "../../../api/hooks/sprints";
import { useGetProject } from "../../../api/hooks/projects";
import { LinkButton } from "../../../components/LinkButton";
import { useNavigate } from "react-router-dom";
import { NotificationOwnerEnum } from "../../../enums/ownerEntities/NotificationOwnerEnum";
import { SubscriptionButton } from "../../../components/buttons/SubscriptionButton";
import SprintAnalytics from "../../analytics/SprintAnalytics";
import { TasksFeature } from "../../tasks/TasksFeature";
import { PageOwnerEnum } from "../../../enums/ownerEntities/PageOwnerEnum";

interface ISprintFeatureProps {
  sprintId: number;
}

export const SprintFeature = ({ sprintId }: ISprintFeatureProps) => {
  const { data: sprint } = useGetSprint(sprintId);
  const { data: project } = useGetProject(
    sprint?.projectId ?? -1000,
    !!sprint?.projectId
  );

  const navigate = useNavigate();

  const tabs: TabsProps["items"] = [
    {
      key: "sprintTasks",
      label: "Задачи",
      children: (
        <TasksFeature
          projectId={sprint?.projectId}
          entityId={sprintId}
          entityType={PageOwnerEnum.SPRINT}
        />
      ),
    },
    {
      key: "sprintAnalytics",
      label: "Аналитика",
      children: <SprintAnalytics sprintId={sprintId} />,
    },
    {
      key: "sprintHistory",
      label: "История",
      children: (
        <HistoryFeature
          ownerId={sprintId}
          ownerType={HistoryOwnerEnum.SPRINT}
        />
      ),
    },
  ];

  return (
    <Flex vertical>
      <div>
        {/* Header */}
        <Flex align="baseline">
          <Typography.Title level={3} style={{ marginRight: "0.5rem" }}>
            Спринт: {sprint?.name}
          </Typography.Title>
          {sprint && (
            <SubscriptionButton
              entityId={sprint?.id}
              entityType={NotificationOwnerEnum.SPRINT}
            />
          )}
        </Flex>

        {project && (
          <LinkButton handleClicked={() => navigate(`/projects/${project.id}`)}>
            {project.name}
          </LinkButton>
        )}
      </div>

      {/* Details */}
      <Flex vertical gap={"8px"}>
        {sprint?.startDate && sprint?.endDate && (
          <Typography.Text>
            <strong>Период:</strong> {sprint.startDate.format("DD.MM.YYYY")} –{" "}
            {sprint.endDate.format("DD.MM.YYYY")}
          </Typography.Text>
        )}
        {sprint?.goal && <Typography.Text>Цель: {sprint.goal}</Typography.Text>}
        {sprint?.status && (
          <div>
            Статус:{" "}
            <Tag color={sprint.status.color}>
              <Typography.Text style={{ color: "black" }}>
                {sprint.status.name}
              </Typography.Text>
            </Tag>
          </div>
        )}
      </Flex>

      {/* Tabs */}
      <div style={{ flex: 1 }}>
        <Tabs items={tabs} />
      </div>
    </Flex>
  );
};
