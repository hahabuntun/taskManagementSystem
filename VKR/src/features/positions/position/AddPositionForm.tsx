import { IAddWorkerPositionOptions } from "../../../api/options/createOptions/IAddWorkerPositionOptions";
import { Button, Form, Input } from "antd";

interface IAddPositionFormProps {
  onAdded?: (options: IAddWorkerPositionOptions) => void;
}

interface IFormValues {
  title: string;
}

export const AddPositionForm = ({ onAdded }: IAddPositionFormProps) => {
  const [form] = Form.useForm();

  const onFormFinish = (values: IFormValues) => {
    const options: IAddWorkerPositionOptions = {
      title: values.title,
    };
    onAdded?.(options);
  };

  return (
    <Form form={form} onFinish={onFormFinish}>
      <Form.Item
        label="Название должности"
        name="title"
        rules={[
          {
            required: true,
            message: "Пожалуйста, введите название должности!",
          },
        ]} // Make it required
      >
        <Input placeholder="Введите должность" />
      </Form.Item>
      <Form.Item style={{ marginBottom: "1rem" }}>
        <Button
          type="primary"
          style={{ width: "100%", background: "green" }}
          htmlType="submit"
        >
          Создать
        </Button>
      </Form.Item>
    </Form>
  );
};
