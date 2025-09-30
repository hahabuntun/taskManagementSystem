import { useState, useMemo } from "react";
import {
  Card,
  Progress,
  Typography,
  Statistic,
  Divider,
  Tag,
  Select,
  Button,
  Timeline,
  Flex,
  Modal,
} from "antd";
import {
  TeamOutlined,
  PlusOutlined,
  ClockCircleOutlined,
  DeleteOutlined,
  EditOutlined,
  CheckCircleOutlined,
} from "@ant-design/icons";
import { IProject } from "../../../interfaces/IProject";
import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { LinksFeature } from "../../links/LinksFeature";
import { LinkOwnerEnum } from "../../../enums/ownerEntities/LinkOwnerEnum";
import { useGetTasks } from "../../../api/hooks/tasks";
import { PageOwnerEnum } from "../../../enums/ownerEntities/PageOwnerEnum";
import ChecklistsFeature from "../../checklists/ChecklistsFeature";
import { WorkerAvatar } from "../../../components/WorkerAvatar";
import { Bar, Pie } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
import {
  useAddMemberToProject,
  useDeleteMemberFromProject,
} from "../../../api/hooks/projects";
import { useGetSprints } from "../../../api/hooks/sprints";
import { LinkButton } from "../../../components/LinkButton";
import { useNavigate } from "react-router-dom";
import { EditProjectMemberForm } from "./EditProjectMemberForm";
import { useGetWorkerRelatedWorkers } from "../../../api/hooks/workers";
import { ITask } from "../../../interfaces/ITask";
import { CheckListOwnerEnum } from "../../../enums/ownerEntities/CheckListOwnerEnum";
import { ProjectTagManager } from "./ProjectTagManager";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend
);

const { Option } = Select;

interface ProjectDashboardProps {
  project: IProject;
}

const ProjectMain = ({ project }: ProjectDashboardProps) => {
  const [selectedMemberId, setSelectedMemberId] = useState<number | null>(null);
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [editingMember, setEditingMember] = useState<any>(null);
  const { data: tasksData } = useGetTasks({
    entityId: project.id,
    pageType: PageOwnerEnum.PROJECT,
    enabled: true,
  });
  const tasks = tasksData ?? [];
  const navigate = useNavigate();

  const deleteAsync = useDeleteMemberFromProject();
  const addAsync = useAddMemberToProject();
  const { data: sprints } = useGetSprints(PageOwnerEnum.PROJECT, project.id);

  const { data: subordinates } = useGetWorkerRelatedWorkers(
    project.manager.id,
    "subordinates"
  );

  const activeSprins = sprints?.filter(
    (sprint) => sprint.status.name === "Активный"
  );
  const milestones = tasks.filter(
    (task) => task.type === TaskTypeEnum.MILESTONE
  );

  const totalTasks = tasks.length;
  const completedTasks = tasks.filter(
    (task) => task.status.name === "Завершена"
  ).length;

  const availableMembers =
    subordinates
      ?.filter(
        (sub) =>
          !project.members
            .map((member) => member.workerData.id)
            .includes(sub.id)
      )
      .map((sub) => ({
        id: sub.id,
        email: sub.email,
      })) ?? [];

  const handleAddMember = () => {
    if (selectedMemberId) {
      addAsync({ projectId: project.id, workerId: selectedMemberId });
      setSelectedMemberId(null);
    }
  };

  const handleDeleteMember = (memberId: number) => {
    deleteAsync({ projectId: project.id, workerId: memberId });
  };

  const handleEditMember = (member: any) => {
    setEditingMember(member);
    setIsEditModalVisible(true);
  };

  const handleModalCancel = () => {
    setIsEditModalVisible(false);
    setEditingMember(null);
  };

  const taskStatusData = useMemo(() => {
    const statusCounts = tasks.reduce((acc, task) => {
      acc[task.status.name] = (acc[task.status.name] || 0) + 1;
      return acc;
    }, {} as Record<string, number>);

    return {
      labels: Object.keys(statusCounts),
      datasets: [
        {
          label: "Задачи по статусам",
          data: Object.values(statusCounts),
          backgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56",
            "#4BC0C0",
            "#9966FF",
          ],
          borderColor: ["#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF"],
          borderWidth: 1,
        },
      ],
    };
  }, [tasks]);

  const workerTaskData = useMemo(() => {
    const workerTasks = tasks.reduce((acc, task: ITask) => {
      task.workers.forEach((worker) => {
        acc[worker.workerData.email] = (acc[worker.workerData.email] || 0) + 1;
      });
      return acc;
    }, {} as Record<string, number>);

    return {
      labels: Object.keys(workerTasks),
      datasets: [
        {
          label: "Задачи на сотрудника",
          data: Object.values(workerTasks),
          backgroundColor: "#36A2EB",
          borderColor: "#36A2EB",
          borderWidth: 1,
        },
      ],
    };
  }, [tasks]);

  const taskChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { position: "top" as const },
      title: { display: true, text: "Распределение задач по статусам" },
    },
    layout: { padding: 10 },
  };

  const workerChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { position: "top" as const },
      title: { display: true, text: "Распределение задач по сотрудникам" },
    },
    scales: { y: { beginAtZero: true } },
    layout: { padding: 10 },
  };

  return (
    <Flex vertical gap="16px" style={{ padding: "16px" }}>
      <Flex gap="16px" wrap>
        <Card style={{ flex: "1 1 300px", maxWidth: "100%" }}>
          <Flex vertical>
            <Typography.Text type="secondary">Прогресс</Typography.Text>
            <Progress percent={project.progress} />
          </Flex>
        </Card>
        <Card style={{ flex: "1 1 300px", maxWidth: "100%" }}>
          <Statistic
            title="Задачи"
            value={completedTasks}
            suffix={<Typography.Text>из {totalTasks} заверш.</Typography.Text>}
            valueStyle={{ fontSize: "16px" }}
          />
        </Card>
        <Card style={{ flex: "1 1 300px", maxWidth: "100%" }}>
          <Statistic
            title="Участники"
            value={project.members.length}
            prefix={<TeamOutlined />}
            valueStyle={{ fontSize: "16px" }}
          />
        </Card>
      </Flex>

      <Card title="Детали проекта">
        <Flex vertical gap="16px">
          <Flex gap="16px" wrap justify="space-between">
            <Flex gap="8px" style={{ minWidth: "200px", flex: "1 1 45%" }}>
              <Typography.Text strong>Статус:</Typography.Text>
              <Tag color={project.status.color}>
                <Typography.Text style={{ color: "black" }}>
                  {project.status.name}
                </Typography.Text>
              </Tag>
            </Flex>
            <Flex
              gap="8px"
              align="center"
              style={{ minWidth: "200px", flex: "1 1 45%" }}
            >
              <Typography.Text strong>Менеджер:</Typography.Text>
              <WorkerAvatar worker={project.manager} size="small" />
            </Flex>
          </Flex>
          <Flex gap="16px" wrap justify="space-between">
            <Flex gap="8px" style={{ minWidth: "200px", flex: "1 1 45%" }}>
              <Typography.Text strong>Начало:</Typography.Text>
              <Typography.Text>
                {project.startDate?.format("DD.MM.YYYY") || "Не указано"}
              </Typography.Text>
            </Flex>
            <Flex gap="8px" style={{ minWidth: "200px", flex: "1 1 45%" }}>
              <Typography.Text strong>Окончание:</Typography.Text>
              <Typography.Text>
                {project.endDate?.format("DD.MM.YYYY") || "Не указано"}
              </Typography.Text>
            </Flex>
          </Flex>
          <Flex gap="8px" style={{ minWidth: "200px", flex: "1 1 45%" }}>
            <Typography.Text strong>Создан:</Typography.Text>
            <Typography.Text>
              {project.createdAt?.format("DD.MM.YYYY")}
            </Typography.Text>
          </Flex>
          <Flex
            vertical
            gap="8px"
            style={{ minWidth: "200px", flex: "1 1 45%" }}
          >
            <Typography.Text strong>Цель:</Typography.Text>
            <Typography.Text>{project.goal || "Не задана"}</Typography.Text>
          </Flex>
        </Flex>
      </Card>

      <Card title="Теги">
        <ProjectTagManager projectId={project.id} />
      </Card>

      <Card title="Участники">
        <Flex vertical gap="16px">
          <Flex gap="8px" wrap>
            <Select
              placeholder="Добавить участника"
              value={selectedMemberId}
              onChange={setSelectedMemberId}
              style={{ width: "200px" }}
              allowClear
              showSearch
            >
              {availableMembers.map((member) => (
                <Option key={member.id} value={member.id}>
                  {member.email}
                </Option>
              ))}
            </Select>
            <Button
              type="primary"
              icon={<PlusOutlined />}
              onClick={handleAddMember}
              disabled={!selectedMemberId}
            >
              Добавить
            </Button>
          </Flex>
          <Flex gap="16px" wrap>
            {project.members.map((member) => (
              <Flex
                key={member.workerData.id}
                gap="8px"
                align="center"
                style={{
                  padding: "8px",
                  border: "1px solid #f0f0f0",
                  borderRadius: "4px",
                  maxWidth: "400px",
                }}
              >
                <WorkerAvatar worker={member.workerData} size="small" />
                <Typography.Text>{member.workerData.email}</Typography.Text>
                <Flex gap="8px" style={{ marginLeft: "auto" }}>
                  <Button
                    icon={<EditOutlined />}
                    onClick={() => handleEditMember(member.workerData)}
                  />
                  <Button
                    icon={<DeleteOutlined />}
                    danger
                    onClick={() => handleDeleteMember(member.workerData.id)}
                  />
                </Flex>
              </Flex>
            ))}
          </Flex>
        </Flex>
      </Card>

      <Modal
        title={
          <Typography.Title level={3} style={{ margin: 0 }}>
            Редактирование разрешений участника
          </Typography.Title>
        }
        destroyOnClose
        open={isEditModalVisible}
        onCancel={handleModalCancel}
        footer={null}
        width={800}
        style={{
          borderRadius: "8px",
          overflow: "hidden",
        }}
        styles={{
          body: {
            padding: "24px",
          },
        }}
      >
        {editingMember && (
          <EditProjectMemberForm
            projectId={project.id}
            workerId={editingMember.id}
          />
        )}
      </Modal>

      <Flex gap="16px" wrap style={{ width: "100%" }}>
        <Card
          title="Статистика задач"
          style={{ flex: "1 1 300px", maxWidth: "100%" }}
        >
          <div style={{ position: "relative", height: "200px", width: "100%" }}>
            <Pie data={taskStatusData} options={taskChartOptions} />
          </div>
        </Card>
        <Card
          title="Статистика сотрудников"
          style={{ flex: "1 1 300px", maxWidth: "100%" }}
        >
          <div style={{ position: "relative", height: "300px", width: "100%" }}>
            <Bar data={workerTaskData} options={workerChartOptions} />
          </div>
        </Card>
      </Flex>

      <Card title="Вехи">
        <Timeline
          mode="left"
          items={milestones.map((milestone) => ({
            key: milestone.id,
            dot:
              milestone.status.name === "Завершена" ? (
                <CheckCircleOutlined style={{ color: "#52c41a" }} />
              ) : (
                <ClockCircleOutlined />
              ),
            color: milestone.status.name === "Завершена" ? "green" : "gray",
            children: (
              <Flex
                style={{
                  maxWidth: "300px",
                  flexWrap: "nowrap",
                }}
                gap="small"
                align="center"
              >
                <Typography.Text strong style={{ flexShrink: 0 }}>
                  {milestone.name}
                </Typography.Text>
                <Flex
                  gap="small"
                  align="center"
                  style={{ flexShrink: 0, marginLeft: "auto" }}
                >
                  <Typography.Text
                    type={
                      milestone.status.name === "Завершена"
                        ? "success"
                        : "secondary"
                    }
                    style={{ whiteSpace: "nowrap" }}
                  >
                    {milestone.endDate?.format("DD.MM.YYYY") || "Без даты"}
                  </Typography.Text>
                  {milestone.status.name === "Завершена" && (
                    <Tag color="green" style={{ margin: 0 }}>
                      Завершена
                    </Tag>
                  )}
                </Flex>
              </Flex>
            ),
          }))}
        />
      </Card>

      <Card title="Чек-листы">
        <ChecklistsFeature
          ownerId={project.id}
          ownerType={CheckListOwnerEnum.PROJECT}
        />
      </Card>

      <Divider orientation="left">Ссылки</Divider>
      <LinksFeature ownerId={project.id} ownerType={LinkOwnerEnum.PROJECT} />

      <Card title="Дополнительно">
        <Flex vertical align="start">
          <Typography.Text>
            Активных спринтов: {activeSprins?.length ?? 0}
          </Typography.Text>
          {activeSprins?.map((sprint) => (
            <LinkButton
              key={sprint.id} // Added key prop here
              id={sprint.id}
              handleClicked={() => navigate(`/sprints/${sprint.id}`)}
            >
              {sprint.name}
            </LinkButton>
          ))}
        </Flex>
      </Card>
    </Flex>
  );
};

export default ProjectMain;
