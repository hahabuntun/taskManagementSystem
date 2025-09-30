import { Button, DatePicker, Form, Input, Select, Tag, Typography } from "antd";
import { IEditSprintOptions } from "../../../api/options/editOptions/IEditSprintOptions";
import { ISprintStatus } from "../../../interfaces/ISprintStatus";
import { ISprint } from "../../../interfaces/ISprint";

import dayjs, { Dayjs } from "dayjs";
import { sprintStatuses } from "../../../sync/sprintStatuses";

interface IEditSprintFormProps {
  sprint: ISprint;
  onEdited?: (options: IEditSprintOptions) => void;
}

interface IFormValues {
  name: string;
  status: string;
  startDate?: Dayjs;
  endDate?: Dayjs;
}

export const EditSprintForm = ({ onEdited, sprint }: IEditSprintFormProps) => {
  const [form] = Form.useForm();

  const onFinish = (values: IFormValues) => {
    const status: ISprintStatus | undefined = sprintStatuses.find(
      (status) => status.name === values.status
    );
    if (status) {
      onEdited?.({
        name: values.name,
        status: status,
        startDate: values.startDate ? dayjs(values.startDate) : undefined,
        endDate: values.endDate ? dayjs(values.endDate) : undefined,
      });
    }
  };

  return (
    <Form
      initialValues={{
        name: sprint.name,
        startDate: sprint.startDate,
        endDate: sprint.endDate,
        status: sprint.status.name,
      }}
      form={form}
      onFinish={onFinish}
      style={{ minWidth: "400px" }}
    >
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
          Изменить
        </Button>
      </Form.Item>
    </Form>
  );
};
