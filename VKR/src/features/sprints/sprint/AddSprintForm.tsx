import { Button, DatePicker, Form, Input, Select, Tag, Typography } from "antd";
import { IAddSprintOptions } from "../../../api/options/createOptions/IAddSprintOptions";
import { ISprintStatus } from "../../../interfaces/ISprintStatus";

import dayjs from "dayjs";
import { sprintStatuses } from "../../../sync/sprintStatuses";

interface IAddSprintFormProps {
  onAdded?: (options: IAddSprintOptions) => void;
}

interface IFormValues {
  name: string;
  status: string;
  startDate: Date;
  endDate: Date;
}

export const AddSprintForm = ({ onAdded }: IAddSprintFormProps) => {
  const [form] = Form.useForm();

  const onFinish = (values: IFormValues) => {
    const status: ISprintStatus | undefined = sprintStatuses.find(
      (status) => status.name === values.status
    );
    if (status) {
      onAdded?.({
        name: values.name,
        status: status,
        startDate: values.startDate ? dayjs(values.startDate) : undefined,
        endDate: values.endDate ? dayjs(values.endDate) : undefined,
      });
    }
  };

  return (
    <Form form={form} onFinish={onFinish} style={{ minWidth: "400px" }}>
      {/* name */}
      <Form.Item
        name="name"
        label="Название"
        rules={[{ required: true, message: "Введите название спринта!" }]}
      >
        <Input />
      </Form.Item>

      {/* status */}
      <Form.Item
        name="status"
        label="Статус"
        rules={[{ required: true, message: "Выберите статус спринта!" }]}
      >
        <Select
          placeholder="Выберите статус"
          options={sprintStatuses?.map((status) => ({
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
      {/* plannned startDate */}
      <Form.Item name="startDate" label="Дата начала">
        <DatePicker style={{ width: "100%" }} />
      </Form.Item>

      <Form.Item name="endDate" label="Дата окончания">
        <DatePicker style={{ width: "100%" }} />
      </Form.Item>

      <Form.Item>
        <Button type="primary" htmlType="submit">
          Создать
        </Button>
      </Form.Item>
    </Form>
  );
};
