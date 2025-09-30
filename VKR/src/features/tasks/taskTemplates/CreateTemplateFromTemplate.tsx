import { useState, useEffect } from "react";
import {
  Form,
  Button,
  Space,
  Checkbox,
  Input,
  Select,
  DatePicker,
  Slider,
  InputNumber,
  Tag,
  Typography,
} from "antd";
import { ITaskTemplateOptions } from "../../../api/options/ITaskTemplateOptions";
import { ITag } from "../../../interfaces/ITag";
import { ITaskTemplate } from "../../../interfaces/ITaskTemplate";
import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { useAddTaskTemplate } from "../../../api/hooks/tasks";
import { taskStatuses } from "../../../sync/taskStatuses";
import { taskPriorities } from "../../../sync/taskPriorities";

const { TextArea } = Input;
const { Text } = Typography;

interface CreateTemplateFromTemplateProps {
  template: ITaskTemplate;
  onSubmit?: () => void;
}

export const CreateTemplateFromTemplate = ({
  template,
  onSubmit,
}: CreateTemplateFromTemplateProps) => {
  const addAsync = useAddTaskTemplate();
  const [form] = Form.useForm();
  const [customTags, setCustomTags] = useState<ITag[]>([]);
  const [includeFields, setIncludeFields] = useState({
    taskName: !!template.taskName,
    description: !!template.description,
    type: !!template.type,
    status: !!template.status,
    priority: !!template.priority,
    tags: !!template.tags?.length,
    progress: template.progress !== undefined,
    startDate: !!template.startDate,
    endDate: !!template.endDate,
    storyPoints: template.storyPoints !== undefined,
  });

  useEffect(() => {
    form.setFieldsValue({
      name: `Копия ${template.name}`,
      taskName: includeFields.taskName ? template.taskName : undefined,
      description: includeFields.description ? template.description : undefined,
      type: includeFields.type ? template.type : undefined,
      status: includeFields.status ? template.status?.id : undefined,
      priority: includeFields.priority ? template.priority?.id : undefined,
      progress: includeFields.progress ? template.progress : undefined,
      startDate: includeFields.startDate ? template.startDate : undefined,
      endDate: includeFields.endDate ? template.endDate : undefined,
      storyPoints: includeFields.storyPoints ? template.storyPoints : undefined,
    });
    setCustomTags(includeFields.tags ? template.tags : []);
  }, [template, form, includeFields]);

  const handleFinish = async (values: any) => {
    const newTemplate: ITaskTemplateOptions = {
      name: values.name,
      taskName: includeFields.taskName ? values.taskName : undefined,
      description: includeFields.description ? values.description : undefined,
      type: includeFields.type ? values.type : undefined,
      status: includeFields.status
        ? taskStatuses.find((s) => s.id === values.status) ?? undefined
        : undefined,
      priority: includeFields.priority
        ? taskPriorities.find((p) => p.id === values.priority) ?? undefined
        : undefined,
      tags: includeFields.tags ? customTags : [],
      progress: includeFields.progress ? values.progress : undefined,
      startDate: includeFields.startDate ? values.startDate : undefined,
      endDate: includeFields.endDate ? values.endDate : undefined,
      storyPoints: includeFields.storyPoints ? values.storyPoints : undefined,
      links: [],
    };

    try {
      addAsync({ options: newTemplate });
      onSubmit?.();
      form.resetFields();
      setCustomTags([]);
      setIncludeFields({
        taskName: !!template.taskName,
        description: !!template.description,
        type: !!template.type,
        status: !!template.status,
        priority: !!template.priority,
        tags: !!template.tags?.length,
        progress: template.progress !== undefined,
        startDate: !!template.startDate,
        endDate: !!template.endDate,
        storyPoints: template.storyPoints !== undefined,
      });
    } catch (error) {
      console.error("Error creating template:", error);
    }
  };

  const handleCheckboxChange =
    (field: keyof typeof includeFields) => (e: any) => {
      setIncludeFields((prev) => ({ ...prev, [field]: e.target.checked }));
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
        <Input placeholder={`Копия ${template.name}`} />
      </Form.Item>

      <Form.Item label="Поля для включения">
        <Space direction="vertical">
          {template.taskName && (
            <Checkbox
              checked={includeFields.taskName}
              onChange={handleCheckboxChange("taskName")}
            >
              Название задачи
            </Checkbox>
          )}
          {template.description && (
            <Checkbox
              checked={includeFields.description}
              onChange={handleCheckboxChange("description")}
            >
              Описание
            </Checkbox>
          )}
          {template.type && (
            <Checkbox
              checked={includeFields.type}
              onChange={handleCheckboxChange("type")}
            >
              Тип
            </Checkbox>
          )}
          {template.status && (
            <Checkbox
              checked={includeFields.status}
              onChange={handleCheckboxChange("status")}
            >
              Статус
            </Checkbox>
          )}
          {template.priority && (
            <Checkbox
              checked={includeFields.priority}
              onChange={handleCheckboxChange("priority")}
            >
              Приоритет
            </Checkbox>
          )}
          {template.tags.length > 0 && (
            <Checkbox
              checked={includeFields.tags}
              onChange={handleCheckboxChange("tags")}
            >
              Теги
            </Checkbox>
          )}
          {template.progress !== undefined && (
            <Checkbox
              checked={includeFields.progress}
              onChange={handleCheckboxChange("progress")}
            >
              Прогресс
            </Checkbox>
          )}
          {template.startDate && (
            <Checkbox
              checked={includeFields.startDate}
              onChange={handleCheckboxChange("startDate")}
            >
              Дата начала
            </Checkbox>
          )}
          {template.endDate && (
            <Checkbox
              checked={includeFields.endDate}
              onChange={handleCheckboxChange("endDate")}
            >
              Дата окончания
            </Checkbox>
          )}
          {template.storyPoints !== undefined && (
            <Checkbox
              checked={includeFields.storyPoints}
              onChange={handleCheckboxChange("storyPoints")}
            >
              Сторипоинты
            </Checkbox>
          )}
        </Space>
      </Form.Item>

      {includeFields.taskName && (
        <Form.Item
          label="Название задачи"
          name="taskName"
          rules={[{ required: true, message: "Введите название задачи" }]}
        >
          <Input placeholder={template.taskName} />
        </Form.Item>
      )}
      {includeFields.description && (
        <Form.Item label="Описание" name="description">
          <TextArea rows={4} placeholder={template.description} />
        </Form.Item>
      )}
      {includeFields.type && (
        <Form.Item
          label="Тип"
          name="type"
          rules={[{ required: true, message: "Выберите тип" }]}
        >
          <Select placeholder="Выберите тип">
            {Object.values(TaskTypeEnum).map((type) => (
              <Select.Option key={type} value={type}>
                {type.charAt(0).toUpperCase() + type.slice(1).toLowerCase()}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>
      )}
      {includeFields.status && (
        <Form.Item
          label="Статус"
          name="status"
          rules={[{ required: true, message: "Выберите статус" }]}
        >
          <Select
            placeholder="Выберите статус"
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
      )}
      {includeFields.priority && (
        <Form.Item
          label="Приоритет"
          name="priority"
          rules={[{ required: true, message: "Выберите приоритет" }]}
        >
          <Select
            placeholder="Выберите приоритет"
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
      )}
      {includeFields.progress && (
        <Form.Item label="Прогресс (%)" name="progress">
          <Slider min={0} max={100} step={1} />
        </Form.Item>
      )}
      {includeFields.startDate && (
        <Form.Item label="Дата начала" name="startDate">
          <DatePicker style={{ width: "100%" }} format="DD.MM.YYYY" />
        </Form.Item>
      )}
      {includeFields.endDate && (
        <Form.Item label="Дата окончания" name="endDate">
          <DatePicker style={{ width: "100%" }} format="DD.MM.YYYY" />
        </Form.Item>
      )}
      {includeFields.storyPoints && (
        <Form.Item label="Сторипоинты" name="storyPoints">
          <InputNumber
            style={{ width: "100%" }}
            min={0}
            placeholder="Введите сторипоинты"
          />
        </Form.Item>
      )}

      <Form.Item>
        <Button type="primary" htmlType="submit">
          Создать шаблон
        </Button>
      </Form.Item>
    </Form>
  );
};
