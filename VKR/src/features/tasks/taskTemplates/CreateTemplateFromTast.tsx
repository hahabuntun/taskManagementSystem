import { useEffect, useState } from "react";
import { Form, Input, Button, Space, Checkbox } from "antd";
import { ITask } from "../../../interfaces/ITask";
import { ITaskTemplateOptions } from "../../../api/options/ITaskTemplateOptions";
import { ITag } from "../../../interfaces/ITag";
import { useAddTaskTemplate } from "../../../api/hooks/tasks";

interface CreateTemplateFromTaskProps {
  task: ITask;
  onSubmit?: () => void;
}

export const CreateTemplateFromTask = ({
  task,
  onSubmit,
}: CreateTemplateFromTaskProps) => {
  const addAsync = useAddTaskTemplate();
  const [form] = Form.useForm();
  const [customTags, setCustomTags] = useState<ITag[]>([]);
  const [includeFields, setIncludeFields] = useState({
    taskName: true,
    description: !!task.description,
    type: true,
    status: true,
    priority: true,
    tags: !!task.tags.length,
    progress: task.progress > 0,
    startDate: !!task.startDate,
    endDate: !!task.endDate,
  });

  useEffect(() => {
    form.setFieldsValue({
      name: `Шаблон для ${task.name}`,
    });
    setCustomTags(includeFields.tags ? task.tags : []);
  }, [task, form, includeFields]);

  const handleFinish = (values: any) => {
    const status = includeFields.status ? task.status : undefined;
    const priority = includeFields.priority ? task.priority : undefined;

    const template: ITaskTemplateOptions = {
      name: values.name,
      taskName: includeFields.taskName ? task.name : undefined,
      description: includeFields.description ? task.description : undefined,
      type: includeFields.type ? task.type : undefined,
      priority,
      status,
      tags: includeFields.tags ? customTags : [],
      progress: includeFields.progress ? task.progress : 0,
      startDate: includeFields.startDate ? task.startDate : undefined,
      endDate: includeFields.endDate ? task.endDate : undefined,
      links: [],
    };
    addAsync({ options: template });
    onSubmit?.();
    form.resetFields();
    setCustomTags([]);
  };

  const handleCheckboxChange =
    (field: keyof typeof includeFields) => (e: any) => {
      setIncludeFields((prev) => ({ ...prev, [field]: e.target.checked }));
    };

  return (
    <Form form={form} onFinish={handleFinish} layout="vertical">
      <Form.Item
        label="Название шаблона"
        name="name"
        rules={[{ required: true, message: "Введите название шаблона" }]}
      >
        <Input />
      </Form.Item>

      <Form.Item label="Поля для включения">
        <Space direction="vertical">
          <Checkbox
            checked={includeFields.taskName}
            onChange={handleCheckboxChange("taskName")}
          >
            Название задачи
          </Checkbox>
          {task.description && (
            <Checkbox
              checked={includeFields.description}
              onChange={handleCheckboxChange("description")}
            >
              Описание
            </Checkbox>
          )}
          <Checkbox
            checked={includeFields.type}
            onChange={handleCheckboxChange("type")}
          >
            Тип
          </Checkbox>
          <Checkbox
            checked={includeFields.status}
            onChange={handleCheckboxChange("status")}
          >
            Статус
          </Checkbox>
          <Checkbox
            checked={includeFields.priority}
            onChange={handleCheckboxChange("priority")}
          >
            Приоритет
          </Checkbox>
          {task.tags.length > 0 && (
            <Checkbox
              checked={includeFields.tags}
              onChange={handleCheckboxChange("tags")}
            >
              Теги
            </Checkbox>
          )}
          {task.progress > 0 && (
            <Checkbox
              checked={includeFields.progress}
              onChange={handleCheckboxChange("progress")}
            >
              Прогресс
            </Checkbox>
          )}
          {task.startDate && (
            <Checkbox
              checked={includeFields.startDate}
              onChange={handleCheckboxChange("startDate")}
            >
              Дата начала
            </Checkbox>
          )}
          {task.endDate && (
            <Checkbox
              checked={includeFields.endDate}
              onChange={handleCheckboxChange("endDate")}
            >
              Дата окончания
            </Checkbox>
          )}
        </Space>
      </Form.Item>

      <Form.Item>
        <Button type="primary" htmlType="submit">
          Создать шаблон
        </Button>
      </Form.Item>
    </Form>
  );
};
