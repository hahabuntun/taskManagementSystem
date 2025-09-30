import { Form, Input, Select, Button, Tag, DatePicker, Typography } from "antd";
import { ITaskTemplateOptions } from "../../../api/options/ITaskTemplateOptions";
import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { taskStatuses } from "../../../sync/taskStatuses";
import { taskPriorities } from "../../../sync/taskPriorities";
import { useAddTaskTemplate } from "../../../api/hooks/tasks";

interface ICreateTemplateProps {
  onSubmit?: () => void;
}
export const CreateTemplate = ({ onSubmit }: ICreateTemplateProps) => {
  const addAsync = useAddTaskTemplate();
  const [form] = Form.useForm();

  const handleFinish = (values: any) => {
    const selectedStatus = taskStatuses.find((s) => s.name === values.status);
    const selectedPriority = taskPriorities.find(
      (p) => p.name === values.priority
    );

    const template: ITaskTemplateOptions = {
      name: values.name,
      taskName: values.taskName,
      description: values.description,
      type: values.type,
      priority: selectedPriority, // Используем данные из хука или заглушку
      status: selectedStatus, // Используем данные из хука или заглушку
      tags: [],
      progress: values.progress || 0,
      startDate: values.startDate,
      endDate: values.endDate,
      links: [],
    };
    addAsync({ options: template });
    onSubmit?.();
    form.resetFields();
  };

  return (
    <Form form={form} onFinish={handleFinish} layout="horizontal">
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
        <Input.TextArea />
      </Form.Item>

      <Form.Item label="Тип" name="type">
        <Select placeholder="Выберите тип">
          {Object.values(TaskTypeEnum).map((type) => (
            <Select.Option key={type} value={type}>
              {type.charAt(0).toUpperCase() + type.slice(1).toLowerCase()}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>

      <Form.Item label="Статус" name="status">
        {taskStatuses ? (
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
        ) : (
          <Input placeholder="Введите статус" />
        )}
      </Form.Item>

      <Form.Item label="Приоритет" name="priority">
        {taskPriorities ? (
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
        ) : (
          <Input placeholder="Введите приоритет" />
        )}
      </Form.Item>

      <Form.Item label="Дата начала" name="startDate">
        <DatePicker style={{ width: "100%" }} format="DD.MM.YYYY" />
      </Form.Item>

      <Form.Item label="Дата окончания" name="endDate">
        <DatePicker style={{ width: "100%" }} format="DD.MM.YYYY" />
      </Form.Item>

      <Form.Item>
        <Button type="primary" htmlType="submit">
          Создать шаблон
        </Button>
      </Form.Item>
    </Form>
  );
};
