import { useEffect } from "react";
import {
  Form,
  Input,
  Select,
  Button,
  Slider,
  InputNumber,
  Typography,
  Divider,
  Tag,
  DatePicker,
} from "antd";
import { ITaskTemplate } from "../../../interfaces/ITaskTemplate";
import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { ITaskTemplateOptions } from "../../../api/options/ITaskTemplateOptions";
import { taskPriorities } from "../../../sync/taskPriorities";
import { taskStatuses } from "../../../sync/taskStatuses";
import { useEditTaskTemplate } from "../../../api/hooks/tasks";

const { TextArea } = Input;
const { Text } = Typography;

interface EditTemplateProps {
  template: ITaskTemplate;
  onSubmit?: () => void;
}

export const EditTemplate = ({ template, onSubmit }: EditTemplateProps) => {
  const editAsync = useEditTaskTemplate();
  const [form] = Form.useForm();

  useEffect(() => {
    form.setFieldsValue({
      name: template.name,
      taskName: template.taskName ?? undefined,
      description: template.description ?? undefined,
      type: template.type ?? undefined,
      priority: template.priority?.id ?? undefined,
      status: template.status?.id ?? undefined,
      progress: template.progress ?? undefined,
      startDate: template.startDate ?? undefined,
      endDate: template.endDate ?? undefined,
      storyPoints: template.storyPoints ?? undefined,
    });
  }, [template, form]);

  const handleFinish = async (values: any) => {
    const updatedTemplate: ITaskTemplateOptions = {
      name: values.name,
      taskName: values.taskName ?? undefined,
      description: values.description ?? undefined,
      type: values.type ?? undefined,
      priority: values.priority
        ? taskPriorities.find((p) => p.id === values.priority) ?? undefined
        : undefined,
      status: values.status
        ? taskStatuses.find((s) => s.id === values.status) ?? undefined
        : undefined,
      tags: template.tags ?? [],
      progress: values.progress ?? undefined,
      startDate: values.startDate ?? undefined,
      endDate: values.endDate ?? undefined,
      storyPoints: values.storyPoints ?? undefined,
      links: [],
    };

    try {
      await editAsync({ templateId: template.id, options: updatedTemplate });
      onSubmit?.();
      form.resetFields();
    } catch (error) {
      console.error("Error updating template:", error);
    }
  };

  return (
    <Form
      form={form}
      onFinish={handleFinish}
      layout="vertical"
      style={{ minWidth: "400px" }}
    >
      <Form.Item
        label="Название шаблона"
        name="name"
        rules={[{ required: true, message: "Введите название шаблона" }]}
      >
        <Input />
      </Form.Item>

      <Form.Item label="Название задачи" name="taskName">
        <Input />
      </Form.Item>

      <Form.Item label="Описание" name="description">
        <TextArea rows={4} />
      </Form.Item>

      <Form.Item label="Тип задачи" name="type">
        <Select placeholder="Выберите тип задачи" allowClear>
          {Object.values(TaskTypeEnum).map((type) => (
            <Select.Option key={type} value={type}>
              {type.charAt(0).toUpperCase() + type.slice(1).toLowerCase()}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>

      <Form.Item label="Статус" name="status">
        <Select
          placeholder="Выбирите статус"
          allowClear
          options={taskStatuses.map((status) => ({
            value: status.id,
            label: (
              <Tag color={status.color}>
                <Text style={{ color: "black" }}>{status.name}</Text>
              </Tag>
            ),
          }))}
        />
      </Form.Item>

      <Form.Item label="Приоритет" name="priority">
        <Select
          placeholder="Выберите приоритет"
          allowClear
          options={taskPriorities.map((priority) => ({
            value: priority.id,
            label: (
              <Tag color={priority.color}>
                <Text style={{ color: "black" }}>{priority.name}</Text>
              </Tag>
            ),
          }))}
        />
      </Form.Item>

      <Form.Item label="Прогресс (%)" name="progress">
        <Slider min={0} max={100} step={1} />
      </Form.Item>

      <Form.Item label="Дата начала" name="startDate">
        <DatePicker style={{ width: "100%" }} format="DD.MM.YYYY" />
      </Form.Item>

      <Form.Item label="Дедлайн" name="endDate">
        <DatePicker style={{ width: "100%" }} format="DD.MM.YYYY" />
      </Form.Item>

      <Form.Item label="Сторипоинты" name="storyPoints">
        <InputNumber
          style={{ width: "100%" }}
          min={0}
          placeholder="Введите сторипоинты"
        />
      </Form.Item>

      <Divider />

      <Form.Item>
        <Button type="primary" htmlType="submit">
          Сохранить
        </Button>
      </Form.Item>
    </Form>
  );
};
