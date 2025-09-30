import { Button, Form, Input } from "antd";
import { IAddLinkOptions } from "../../api/options/createOptions/IAddLinkOptions";
import { useForm } from "antd/es/form/Form";

interface IAddLinkFormProps {
  onAddLink: (options: IAddLinkOptions) => void;
}

interface IFormValues {
  name?: string;
  link: string;
}

export const AddLinkForm = ({ onAddLink }: IAddLinkFormProps) => {
  const [form] = useForm();

  const onFormFinish = (values: IFormValues) => {
    const options: IAddLinkOptions = {
      name: values.name,
      link: values.link,
    };
    onAddLink(options);
  };

  return (
    <Form form={form} layout="horizontal" onFinish={onFormFinish}>
      <Form.Item label="Описание" name="name">
        <Input placeholder="Введите описание" />
      </Form.Item>
      <Form.Item
        label="Ссылка"
        name="link"
        rules={[{ required: true, message: "Пожалуйста, введите ссылку" }]}
      >
        <Input placeholder="Введите ссылку" />
      </Form.Item>
      <Form.Item>
        <Button
          style={{ width: "100%", background: "green" }}
          type="primary"
          htmlType="submit"
        >
          Добавить
        </Button>
      </Form.Item>
    </Form>
  );
};
