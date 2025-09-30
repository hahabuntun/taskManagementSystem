import {
  Form,
  Input,
  Select,
  Button,
  DatePicker,
  Slider,
  ConfigProvider,
  InputNumber,
  Typography,
  Tag,
  Divider,
} from "antd";
import { useEffect, useState } from "react";
import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { IAddTaskOptions } from "../../../api/options/createOptions/IAddTaskOptions";
import { useAddTask } from "../../../api/hooks/tasks";
import { useGetSprints } from "../../../api/hooks/sprints";
import { PageOwnerEnum } from "../../../enums/ownerEntities/PageOwnerEnum";
import TextArea from "antd/es/input/TextArea";
import { taskStatuses } from "../../../sync/taskStatuses";
import { taskPriorities } from "../../../sync/taskPriorities";
import { TaskStatusEnum } from "../../../enums/statuses/TaskStatusEnum";
import { TaskPriorityEnum } from "../../../enums/TaskPriorityEnum";
import { IWorker } from "../../../interfaces/IWorker";
import { Dayjs } from "dayjs";
import { useGetProjects } from "../../../api/hooks/projects";

interface IFormValues {
  name: string;
  description: string;
  status: string;
  priority: string;
  type: TaskTypeEnum;
  progress: number;
  sprintId?: number;
  startDate?: Dayjs;
  endDate?: Dayjs;
  storyPoints?: number;
  projectId?: number;
}

interface IAddTaskFormProps {
  user: IWorker;
}

export const AddTaskFormNoProject = ({ user }: IAddTaskFormProps) => {
  const addAsync = useAddTask();
  const [form] = Form.useForm<IFormValues>();
  const [selectedProjectId, setSelectedProjectId] = useState<number | null>(
    null
  );

  const { data: projects = [], isLoading: projectsLoading } = useGetProjects(
    user.id,
    PageOwnerEnum.WORKER
  );
  const { data: sprints, isLoading: sprintsLoading } = useGetSprints(
    PageOwnerEnum.PROJECT,
    selectedProjectId ?? 0,
    !!selectedProjectId
  );

  useEffect(() => {
    if (selectedProjectId) {
      form.setFieldsValue({ sprintId: undefined });
    }
  }, [selectedProjectId, form]);

  const handleFinish = (values: IFormValues) => {
    if (!values.projectId) {
      form.setFields([
        {
          name: "projectId",
          errors: ["Пожалуйста, выберите проект"],
        },
      ]);
      return;
    }

    const status = taskStatuses.find((s) => s.name === values.status);
    const priority = taskPriorities.find((p) => p.name === values.priority);

    if (status && priority) {
      const task: IAddTaskOptions = {
        name: values.name,
        description: values.description,
        type: values.type,
        progress: values.progress,
        startDate: values.startDate,
        endDate: values.endDate,
        status,
        priority,
        storyPoints: values.storyPoints,
        workers: [], // Workers managed separately
        sprint: values.sprintId
          ? sprints?.find((s) => s.id === values.sprintId)
          : undefined,
        existingTagIds: [],
      };
      addAsync({ projectId: values.projectId, creator: user, options: task });
      form.resetFields();
      setSelectedProjectId(null);
    }
  };

  return (
    <ConfigProvider
      getPopupContainer={(trigger) => trigger?.parentElement || document.body}
    >
      <Form
        form={form}
        layout="horizontal"
        onFinish={handleFinish}
        style={{ marginTop: "2rem" }}
        initialValues={{
          name: "",
          description: "",
          status: TaskStatusEnum.PENDING,
          priority: TaskPriorityEnum.NORMAL,
          type: Object.values(TaskTypeEnum)[0],
          progress: 0,
          sprintId: undefined,
          projectId: undefined,
        }}
      >
        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Проект"
          name="projectId"
          rules={[{ required: true, message: "Выберите проект" }]}
        >
          <Select
            placeholder="Выберите проект"
            loading={projectsLoading}
            onChange={(value) => setSelectedProjectId(value)}
            allowClear
          >
            {projects.map((project) => (
              <Select.Option key={project.id} value={project.id}>
                {project.name}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Название задачи"
          name="name"
          rules={[{ required: true, message: "Введите название задачи" }]}
        >
          <Input placeholder="Введите название" />
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Описание"
          name="description"
        >
          <TextArea rows={4} placeholder="Введите описание" />
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Статус"
          name="status"
          rules={[{ required: true, message: "Выберите статус" }]}
        >
          <Select placeholder="Выберите статус">
            {taskStatuses.map((status) => (
              <Select.Option key={status.name} value={status.name}>
                <Tag color={status.color}>
                  <Typography.Text style={{ color: "black" }}>
                    {status.name}
                  </Typography.Text>
                </Tag>
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Приоритет"
          name="priority"
          rules={[{ required: true, message: "Выберите приоритет" }]}
        >
          <Select placeholder="Выберите приоритет">
            {taskPriorities.map((priority) => (
              <Select.Option key={priority.name} value={priority.name}>
                <Tag color={priority.color}>
                  <Typography.Text style={{ color: "black" }}>
                    {priority.name}
                  </Typography.Text>
                </Tag>
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Тип задачи"
          name="type"
          rules={[{ required: true, message: "Выберите тип задачи" }]}
        >
          <Select placeholder="Выберите тип">
            {Object.values(TaskTypeEnum).map((type) => (
              <Select.Option key={type} value={type}>
                {type.charAt(0).toUpperCase() + type.slice(1).toLowerCase()}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Стори поинты"
          name="storyPoints"
        >
          <InputNumber placeholder="Введите сторипоинты" />
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Прогресс"
          name="progress"
        >
          <Slider min={0} max={100} />
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Спринт"
          name="sprintId"
        >
          <Select
            placeholder="Выберите спринт"
            allowClear
            loading={sprintsLoading}
            disabled={!selectedProjectId}
          >
            {sprints?.map((sprint) => (
              <Select.Option key={sprint.id} value={sprint.id}>
                {sprint.name}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Дата начала"
          name="startDate"
        >
          <DatePicker format="DD.MM.YYYY" style={{ width: "100%" }} />
        </Form.Item>

        <Form.Item
          style={{ marginBottom: "0.5rem" }}
          label="Дата окончания"
          name="endDate"
        >
          <DatePicker format="DD.MM.YYYY" style={{ width: "100%" }} />
        </Form.Item>

        <Divider style={{ marginBottom: "1rem" }} />

        <Form.Item>
          <Button type="primary" htmlType="submit">
            Создать задачу
          </Button>
        </Form.Item>
      </Form>
    </ConfigProvider>
  );
};
