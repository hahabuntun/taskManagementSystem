import { Card, Flex, Select, Button, Space, Typography, Tag } from "antd";
import {
  useGetProjectMember,
  useGetProjectMembers,
} from "../../../api/hooks/projects";
import { DeleteOutlined, PlusOutlined } from "@ant-design/icons";
import { IProjectMember } from "../../../interfaces/IProjectMember";
import { IWorker } from "../../../interfaces/IWorker";
import { WorkerAvatar } from "../../../components/WorkerAvatar";
import {
  useAddTaskReceiver,
  useRemoveTaskReceiver,
} from "../../../api/hooks/projects";
import { useState } from "react";

interface IEditProjectMemberFormProps {
  projectId: number;
  workerId: number;
}

export const EditProjectMemberForm = ({
  projectId,
  workerId,
}: IEditProjectMemberFormProps) => {
  const { data: projectMember } = useGetProjectMember(projectId, workerId);
  const { data: projectMembers } = useGetProjectMembers(projectId);

  const addTaskReceiver = useAddTaskReceiver();
  const removeTaskReceiver = useRemoveTaskReceiver();

  const [newReceiverId, setNewReceiverId] = useState<number | null>(null);
  const [newTaskGiverId, setNewTaskGiverId] = useState<number | null>(null);

  const otherMembers = projectMembers?.filter(
    (member: IProjectMember) => member.workerData.id !== workerId
  );

  const currentReceivers =
    projectMember?.taskTakers.map((taker: IWorker) => taker.id) || [];
  const currentTaskGivers =
    projectMember?.taskGivers.map((giver: IWorker) => giver.id) || [];

  const handleAddReceiver = () => {
    if (newReceiverId && !currentReceivers.includes(newReceiverId)) {
      addTaskReceiver({ projectId, workerId, receiverId: newReceiverId });
      setNewReceiverId(null);
    }
  };

  const handleRemoveReceiver = (receiverId: number) => {
    if (currentReceivers.includes(receiverId)) {
      removeTaskReceiver({ projectId, workerId, receiverId });
    }
  };

  const handleAddTaskGiver = () => {
    if (newTaskGiverId && !currentTaskGivers.includes(newTaskGiverId)) {
      addTaskReceiver({
        projectId,
        workerId: newTaskGiverId,
        receiverId: workerId,
      });
      setNewTaskGiverId(null);
    }
  };

  const handleRemoveTaskGiver = (taskGiverId: number) => {
    if (currentTaskGivers.includes(taskGiverId)) {
      removeTaskReceiver({
        projectId,
        workerId: taskGiverId,
        receiverId: workerId,
      });
    }
  };

  return (
    <Flex vertical gap="middle">
      {/* Информация о сотруднике */}
      <Card
        style={{
          borderRadius: "8px",
          boxShadow: "0 2px 8px rgba(0, 0, 0, 0.05)",
        }}
      >
        <Flex gap="middle" align="center">
          <WorkerAvatar size="small" worker={projectMember?.worker} />
          <Flex vertical>
            <Typography.Title level={4} style={{ margin: 0 }}>
              {projectMember?.worker.secondName}{" "}
              {projectMember?.worker.firstName}{" "}
              {projectMember?.worker.thirdName || ""}
            </Typography.Title>
            <Typography.Text type="secondary">
              {projectMember?.worker.email}
            </Typography.Text>
            <Space style={{ marginTop: 8 }}>
              <Tag color="blue">
                <Typography.Text style={{ color: "black" }}>
                  {projectMember?.worker.workerPosition?.title ||
                    "Должность не указана"}
                </Typography.Text>
              </Tag>
              <Tag color="green">
                <Typography.Text style={{ color: "black" }}>
                  {projectMember?.worker.status?.name || "Статус не указан"}
                </Typography.Text>
              </Tag>
            </Space>
          </Flex>
        </Flex>
      </Card>

      {/* Карточка для получателей задач */}
      <Card title="Получатели задач">
        <Flex vertical gap="16px">
          <Flex gap="8px" wrap>
            <Select
              placeholder="Добавить получателя"
              value={newReceiverId}
              onChange={setNewReceiverId}
              style={{ width: "200px" }}
              allowClear
              showSearch
            >
              {otherMembers
                ?.filter(
                  (member) => !currentReceivers.includes(member.workerData.id)
                )
                .map((member) => (
                  <Select.Option
                    key={member.workerData.id}
                    value={member.workerData.id}
                  >
                    {member.workerData.email}
                  </Select.Option>
                ))}
            </Select>
            <Button
              type="primary"
              icon={<PlusOutlined />}
              onClick={handleAddReceiver}
              disabled={!newReceiverId}
            >
              Добавить
            </Button>
          </Flex>
          <Flex gap="16px" wrap>
            {currentReceivers.map((receiverId) => {
              const member = projectMembers?.find(
                (m) => m.workerData.id === receiverId
              );
              return (
                member && (
                  <Flex
                    key={receiverId}
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
                    <Button
                      icon={<DeleteOutlined />}
                      danger
                      onClick={() => handleRemoveReceiver(receiverId)}
                    />
                  </Flex>
                )
              );
            })}
          </Flex>
        </Flex>
      </Card>

      {/* Карточка для постановщиков задач */}
      <Card title="Постановщики задач">
        <Flex vertical gap="16px">
          <Flex gap="8px" wrap>
            <Select
              placeholder="Добавить постановщика"
              value={newTaskGiverId}
              onChange={setNewTaskGiverId}
              style={{ width: "200px" }}
              allowClear
              showSearch
            >
              {otherMembers
                ?.filter(
                  (member) => !currentTaskGivers.includes(member.workerData.id)
                )
                .map((member) => (
                  <Select.Option
                    key={member.workerData.id}
                    value={member.workerData.id}
                  >
                    {member.workerData.email}
                  </Select.Option>
                ))}
            </Select>
            <Button
              type="primary"
              icon={<PlusOutlined />}
              onClick={handleAddTaskGiver}
              disabled={!newTaskGiverId}
            >
              Добавить
            </Button>
          </Flex>
          <Flex gap="16px" wrap>
            {currentTaskGivers.map((giverId) => {
              const member = projectMembers?.find(
                (m) => m.workerData.id === giverId
              );
              return (
                member && (
                  <Flex
                    key={giverId}
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
                    <Button
                      icon={<DeleteOutlined />}
                      danger
                      onClick={() => handleRemoveTaskGiver(giverId)}
                    />
                  </Flex>
                )
              );
            })}
          </Flex>
        </Flex>
      </Card>
    </Flex>
  );
};
