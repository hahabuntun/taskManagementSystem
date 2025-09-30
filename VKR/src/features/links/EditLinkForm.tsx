import { Button, Form, Input } from "antd";
import { IEditLinkOptions } from "../../api/options/editOptions/IEditLinkOptions";
import { useForm } from "antd/es/form/Form";
import { useEditLink } from "../../api/hooks/links";
import { LinkOwnerEnum } from "../../enums/ownerEntities/LinkOwnerEnum";

interface IEditLinkFormProps {
  linkId: number;
  ownerType: LinkOwnerEnum;
  ownerId: number;
  linkName?: string;
  linkText: string;
}

interface IFormValues {
  name?: string;
  link: string;
}

export const EditLinkForm = ({
  linkText,
  linkId,
  linkName,
  ownerType,
  ownerId,
}: IEditLinkFormProps) => {
  const [form] = useForm();
  const editAsync = useEditLink();

  const onFormFinish = (values: IFormValues) => {
    const options: IEditLinkOptions = {
      name: values.name,
      link: values.link,
    };
    editAsync({
      linkId: linkId,
      ownerId: ownerId,
      ownerType: ownerType,
      options,
    });
  };

  return (
    <Form
      form={form}
      onFinish={onFormFinish}
      initialValues={{ name: linkName, link: linkText }} // Initialize form with current values
    >
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
        <Button style={{ width: "100%" }} type="primary" htmlType="submit">
          Изменить
        </Button>
      </Form.Item>
    </Form>
  );
};
