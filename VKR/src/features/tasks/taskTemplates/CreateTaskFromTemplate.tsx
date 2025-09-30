import { useState, useEffect } from "react";
import {
  Form,
  Button,
  Space,
  Checkbox,
  Select,
  Input,
  DatePicker,
  Slider,
  InputNumber,
  Typography,
  Divider,
  Tag,
} from "antd";
import { IAddTaskOptions } from "../../../api/options/createOptions/IAddTaskOptions";
import { IWorker } from "../../../interfaces/IWorker";
import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { useGetProjects } from "../../../api/hooks/projects";
import { PageOwnerEnum } from "../../../enums/ownerEntities/PageOwnerEnum";
import { ITaskTemplate } from "../../../interfaces/ITaskTemplate";
import { taskStatuses } from "../../../sync/taskStatuses";
import { taskPriorities } from "../../../sync/taskPriorities";
import { useAddTask } from "../../../api/hooks/tasks";

const { TextArea } = Input;
const { Text } = Typography;

interface CreateTaskFromTemplateProps {
  user: IWorker;
  template: ITaskTemplate;
  onSubmit?: () => void;
}

export const CreateTaskFromTemplate = ({
  user,
  template,
  onSubmit,
}: CreateTaskFromTemplateProps) => {
  const addAsync = useAddTask();
  const [form] = Form.useForm();
  const [selectedProjectId, setSelectedProjectId] = useState<number | null>(
    null
  );
  const { data: projects } = useGetProjects(user.id, PageOwnerEnum.WORKER);
  const [includeFields, setIncludeFields] = useState({
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
      name: template.taskName ?? template.name,
      description: includeFields.description ? template.description : undefined,
      type: includeFields.type ? template.type : undefined,
      status: includeFields.status ? template.status?.id : undefined,
      priority: includeFields.priority ? template.priority?.id : undefined,
      progress: includeFields.progress ? template.progress : undefined,
      startDate: includeFields.startDate ? template.startDate : undefined,
      endDate: includeFields.endDate ? template.endDate : undefined,
      storyPoints: includeFields.storyPoints ? template.storyPoints : undefined,
    });
  }, [template, form, includeFields]);

  const handleFinish = (values: any) => {
    if (!selectedProjectId) return;

    const task: IAddTaskOptions = {
      name: values.name,
      description: includeFields.description ? values.description : undefined,
      type: includeFields.type ? values.type : undefined,
      status: includeFields.status
        ? taskStatuses.find((s) => s.id === values.status) ?? taskStatuses[0]
        : taskStatuses[0],
      priority: includeFields.priority
        ? taskPriorities.find((p) => p.id === values.priority) ??
          taskPriorities[0]
        : taskPriorities[0],
      progress: includeFields.progress ? values.progress : undefined,
      startDate: includeFields.startDate ? values.startDate : undefined,
      endDate: includeFields.endDate ? values.endDate : undefined,
      storyPoints: includeFields.storyPoints ? values.storyPoints : undefined,
      existingTagIds: includeFields.tags
        ? template.tags.map((tag) => tag.id)
        : [],
      workers: [],
      sprint: undefined,
    };

    onSubmit?.();
    addAsync({ creator: user, options: task, projectId: selectedProjectId });
    form.resetFields();
    setSelectedProjectId(null);
    setIncludeFields({
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
        label="Проект"
        name="projectId"
        rules={[{ required: true, message: "Выберите проект" }]}
      >
        <Select
          placeholder="Выберите проект"
          onChange={setSelectedProjectId}
          value={selectedProjectId}
        >
          {projects?.map((project) => (
            <Select.Option key={project.id} value={project.id}>
              {project.name}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>

      <Form.Item
        label="Название задачи"
        name="name"
        rules={[{ required: true, message: "Введите название задачи" }]}
      >
        <Input placeholder={template.taskName ?? template.name} />
      </Form.Item>

      <Form.Item label="Fields to Include">
        <Space direction="vertical">
          <Checkbox
            checked={includeFields.description}
            onChange={handleCheckboxChange("description")}
            disabled={!template.description}
          >
            Описание
          </Checkbox>
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
          <Checkbox
            checked={includeFields.tags}
            onChange={handleCheckboxChange("tags")}
            disabled={!template.tags?.length}
          >
            Теги
          </Checkbox>
          <Checkbox
            checked={includeFields.progress}
            onChange={handleCheckboxChange("progress")}
            disabled={template.progress === undefined}
          >
            Прогресс
          </Checkbox>
          <Checkbox
            checked={includeFields.startDate}
            onChange={handleCheckboxChange("startDate")}
            disabled={!template.startDate}
          >
            Дата начала
          </Checkbox>
          <Checkbox
            checked={includeFields.endDate}
            onChange={handleCheckboxChange("endDate")}
            disabled={!template.endDate}
          >
            Дата окончания
          </Checkbox>
          <Checkbox
            checked={includeFields.storyPoints}
            onChange={handleCheckboxChange("storyPoints")}
            disabled={template.storyPoints === undefined}
          >
            Стори поинты
          </Checkbox>
        </Space>
      </Form.Item>

      <Divider />

      {includeFields.description && (
        <Form.Item label="Описание" name="description">
          <TextArea rows={4} placeholder={template.description} />
        </Form.Item>
      )}
      {includeFields.type && (
        <Form.Item
          label="Тип"
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
        <Form.Item label="Стори поинты" name="storyPoints">
          <InputNumber
            style={{ width: "100%" }}
            min={0}
            placeholder="Введите количество сторипоинтов"
          />
        </Form.Item>
      )}

      <Divider />

      <Form.Item>
        <Button type="primary" htmlType="submit" disabled={!selectedProjectId}>
          Создать задачу
        </Button>
      </Form.Item>
    </Form>
  );
};
