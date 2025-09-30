import {
  Form,
  Input,
  Select,
  DatePicker,
  Button,
  Tag,
  Slider,
  Divider,
  Typography,
} from "antd";
import { IProject } from "../../../interfaces/IProject";
import dayjs from "dayjs";
import { projectStatuses } from "../../../sync/projectStatuses";
import { useEditProject } from "../../../api/hooks/projects";

const { TextArea } = Input;

interface IEditProjectFormProps {
  project: IProject;
}

interface IFormValues {
  name: string;
  statusId: number;
  description?: string;
  goal?: string;
  startDate?: Date;
  endDate?: Date;
  progress: number;
}

export const EditProjectForm = ({ project }: IEditProjectFormProps) => {
  const [form] = Form.useForm();
  const editAsync = useEditProject();

  const onFinish = (values: IFormValues) => {
    editAsync({
      projectId: project.id,
      options: {
        name: values.name,
        description: values.description,
        goal: values.goal,
        progress: values.progress,
        statusId: values.statusId,
        startDate: values.startDate ? dayjs(values.startDate) : undefined,
        endDate: values.endDate ? dayjs(values.endDate) : undefined,
      },
    });
    form.resetFields();
  };

  return (
    <Form
      initialValues={{
        name: project.name,
        description: project.description,
        goal: project.goal,
        progress: project.progress,
        statusId: project.status.id,
        startDate: project.startDate ? dayjs(project.startDate) : undefined,
        endDate: project.endDate ? dayjs(project.endDate) : undefined,
      }}
      form={form}
      onFinish={onFinish}
      style={{ minWidth: "400px" }}
    >
      {/* Название проекта */}
      <Form.Item
        name="name"
        label="Название"
        rules={[{ required: true, message: "Введите название проекта!" }]}
      >
        <Input />
      </Form.Item>

      {/* Описание проекта */}
      <Form.Item
        name="description"
        label="Описание"
        rules={[{ required: true, message: "Введите описание проекта!" }]}
      >
        <TextArea rows={4} />
      </Form.Item>

      {/* Цель */}
      <Form.Item name="goal" label="Цель">
        <TextArea rows={4} />
      </Form.Item>

      {/* Прогресс проекта */}
      <Form.Item name="progress" label="Прогресс (%)">
        <Slider min={0} max={100} step={1} />
      </Form.Item>

      {/* Статус проекта */}
      <Form.Item
        name="statusId"
        label="Статус"
        rules={[{ required: true, message: "Выберите статус проекта!" }]}
      >
        <Select
          placeholder="Выберите статус"
          options={projectStatuses?.map((status) => ({
            value: status.id,
            label: (
              <Tag color={status.color}>
                <Typography.Text style={{ color: "black" }}>
                  {status.name}
                </Typography.Text>
              </Tag>
            ),
          }))}
        />
      </Form.Item>

      {/* Дата начала */}
      <Form.Item name="startDate" label="Дата начала">
        <DatePicker style={{ width: "100%" }} />
      </Form.Item>

      {/* Дата окончания */}
      <Form.Item name="endDate" label="Дата окончания">
        <DatePicker style={{ width: "100%" }} />
      </Form.Item>

      <Divider style={{ marginBottom: "1rem" }} />

      {/* Кнопка отправки формы */}
      <Form.Item>
        <Button type="primary" htmlType="submit">
          Обновить
        </Button>
      </Form.Item>
    </Form>
  );
};
