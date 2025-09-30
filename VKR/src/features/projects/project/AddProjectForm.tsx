import {
  Form,
  Input,
  Select,
  DatePicker,
  Button,
  Tag,
  Divider,
  Slider,
  Typography,
} from "antd";
import { IAddProjectOptions } from "../../../api/options/createOptions/IAddProjectOptions";
import dayjs, { Dayjs } from "dayjs";
import { projectStatuses } from "../../../sync/projectStatuses";

const { TextArea } = Input;

interface IAddProjectFormProps {
  onAdded?: (options: IAddProjectOptions) => void;
}

interface IFormValues {
  name: string;
  statusId: number;
  description: string;
  goal: string;
  startDate?: Dayjs;
  endDate?: Dayjs;
  progress: number;
}

export const AddProjectForm = ({ onAdded }: IAddProjectFormProps) => {
  const [form] = Form.useForm();

  const onFinish = (values: IFormValues) => {
    onAdded?.({
      name: values.name,
      statusId: values.statusId,
      description: values.description,
      goal: values.goal,
      startDate: values.startDate ? dayjs(values.startDate) : undefined,
      endDate: values.endDate ? dayjs(values.endDate) : undefined,
      progress: values.progress,
    });
    form.resetFields();
  };

  return (
    <Form form={form} onFinish={onFinish} layout="horizontal">
      {/* Existing form items remain the same until Tags section */}
      <Form.Item
        style={{ marginBottom: "0.5rem" }}
        name="name"
        label="Название"
        rules={[{ required: true, message: "Введите название проекта!" }]}
      >
        <Input />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "0.5rem" }}
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

      <Form.Item
        style={{ marginBottom: "0.5rem" }}
        name="description"
        label="Описание"
        rules={[{ required: true, message: "Введите описание проекта!" }]}
      >
        <TextArea rows={4} />
      </Form.Item>

      <Form.Item style={{ marginBottom: "0.5rem" }} name="goal" label="Цель">
        <TextArea rows={4} />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "0.5rem" }}
        name="startDate"
        label="Дата начала"
      >
        <DatePicker style={{ width: "100%" }} />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "0.5rem" }}
        name="endDate"
        label="Дата окончания"
      >
        <DatePicker style={{ width: "100%" }} />
      </Form.Item>

      <Form.Item name="progress" label="Прогресс (%)">
        <Slider min={0} max={100} step={1} />
      </Form.Item>

      <Divider style={{ marginBottom: "1rem" }} />

      <Form.Item style={{ marginBottom: "0.5rem" }}>
        <Button type="primary" htmlType="submit">
          Создать
        </Button>
      </Form.Item>
    </Form>
  );
};
