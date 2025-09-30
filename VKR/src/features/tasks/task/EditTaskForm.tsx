import {
  Form,
  Input,
  Select,
  DatePicker,
  Button,
  Tag,
  Slider,
  Divider,
  InputNumber,
  Typography,
} from "antd";
import { useEditTaskData, useGetTask } from "../../../api/hooks/tasks";
import { IEditTaskOptions } from "../../../api/options/editOptions/IEditTaskOptions";
import { IWorker } from "../../../interfaces/IWorker";
import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import dayjs, { Dayjs } from "dayjs";
import { taskStatuses } from "../../../sync/taskStatuses";
import { taskPriorities } from "../../../sync/taskPriorities";
import { useGetSprints } from "../../../api/hooks/sprints";
import { PageOwnerEnum } from "../../../enums/ownerEntities/PageOwnerEnum";

const { TextArea } = Input;

interface IFormValues {
  name: string;
  description?: string;
  status: string;
  priority: string;
  type: TaskTypeEnum;
  progress: number;
  sprintId?: number;
  startDate?: Dayjs;
  endDate?: Dayjs;
  storyPoints?: number;
}

interface IEditTaskFormProps {
  taskId: number;
  projectId: number;
  user: IWorker;
  onUpdated?: () => void;
}

export const EditTaskForm = ({
  taskId,
  projectId,
  onUpdated,
}: IEditTaskFormProps) => {
  const [form] = Form.useForm<IFormValues>();
  const { data: task } = useGetTask(taskId);

  const { data: sprints } = useGetSprints(PageOwnerEnum.PROJECT, projectId);
  const editAsync = useEditTaskData();

  if (task) {
    const onFinish = (values: IFormValues) => {
      const status = taskStatuses.find((s) => s.name === values.status);
      const priority = taskPriorities.find((p) => p.name === values.priority);

      if (status && priority) {
        const updatedTask: IEditTaskOptions = {
          projectId: task.project.id,
          name: values.name,
          description: values.description,
          type: values.type,
          progress: values.progress,
          status,
          priority,
          sprintId: values.sprintId,
          startDate: values.startDate,
          endDate: values.endDate,
          workers: task.workers, // Workers managed separately
          storyPoints: values.storyPoints,
        };

        editAsync({ taskId: taskId, options: updatedTask });
        onUpdated?.();
        form.resetFields();
      }
    };

    if (taskStatuses && taskPriorities && sprints) {
      return (
        <Form
          initialValues={{
            name: task.name,
            description: task.description,
            status: task.status.name,
            priority: task.priority.name,
            type: task.type,
            progress: task.progress,
            sprintId: task.sprint?.id,
            startDate: task.startDate ? dayjs(task.startDate) : undefined,
            endDate: task.endDate ? dayjs(task.endDate) : undefined,
            storyPoints: task.storyPoints,
          }}
          form={form}
          onFinish={onFinish}
          style={{ minWidth: "400px" }}
        >
          <Form.Item
            name="name"
            label="Название"
            rules={[{ required: true, message: "Введите название задачи!" }]}
          >
            <Input />
          </Form.Item>

          <Form.Item name="description" label="Описание">
            <TextArea rows={4} />
          </Form.Item>

          <Form.Item
            name="status"
            label="Статус"
            rules={[{ required: true, message: "Выберите статус задачи!" }]}
          >
            <Select
              placeholder="Выберите статус"
              options={taskStatuses.map((status) => ({
                value: status.name,
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
            name="priority"
            label="Приоритет"
            rules={[{ required: true, message: "Выберите приоритет задачи!" }]}
          >
            <Select
              placeholder="Выберите приоритет"
              options={taskPriorities.map((priority) => ({
                value: priority.name,
                label: (
                  <Tag color={priority.color}>
                    <Typography.Text style={{ color: "black" }}>
                      {priority.name}
                    </Typography.Text>
                  </Tag>
                ),
              }))}
            />
          </Form.Item>

          <Form.Item
            name="type"
            label="Тип"
            rules={[{ required: true, message: "Выберите тип задачи!" }]}
          >
            <Select
              placeholder="Выберите тип"
              options={Object.values(TaskTypeEnum).map((type) => ({
                value: type,
                label:
                  type.charAt(0).toUpperCase() + type.slice(1).toLowerCase(),
              }))}
            />
          </Form.Item>

          <Form.Item
            style={{ marginBottom: "0.5rem" }}
            label="Стори поинты"
            name="storyPoints"
          >
            <InputNumber
              style={{ width: "100%" }}
              placeholder="Введите количество сторипоинтов"
            />
          </Form.Item>

          <Form.Item name="progress" label="Прогресс (%)">
            <Slider min={0} max={100} step={1} />
          </Form.Item>

          <Form.Item name="sprintId" label="Спринт">
            <Select
              placeholder="Выберите спринт"
              allowClear
              options={sprints.map((sprint) => ({
                value: sprint.id,
                label: sprint.name,
              }))}
            />
          </Form.Item>

          <Form.Item name="startDate" label="Дата начала">
            <DatePicker style={{ width: "100%" }} format="DD.MM.YYYY" />
          </Form.Item>

          <Form.Item name="endDate" label="Дата окончания">
            <DatePicker style={{ width: "100%" }} format="DD.MM.YYYY" />
          </Form.Item>

          <Divider style={{ marginBottom: "1rem" }} />

          <Form.Item>
            <Button type="primary" htmlType="submit">
              Обновить задачу
            </Button>
          </Form.Item>
        </Form>
      );
    }
    return null;
  }
};
