import { Button, Form, Input } from "antd";
import { IWorker } from "../../../interfaces/IWorker";
import { useChangePassword } from "../../../api/hooks/workers";

interface IChangeWorkerPasswordFormProps {
  worker: IWorker;
  onSuccess?: () => void; // Optional callback after success
}

interface IFormValues {
  newPassword: string;
  confirmPassword: string;
}

export const ChangeWorkerPasswordForm = ({
  worker,
  onSuccess,
}: IChangeWorkerPasswordFormProps) => {
  console.log(worker, onSuccess);
  const [form] = Form.useForm();
  const changePasswordAsync = useChangePassword();

  const onFormFinish = (values: IFormValues) => {
    changePasswordAsync({
      workerId: worker.id,
      newPassword: values.newPassword,
    });
  };

  return (
    <Form<IFormValues>
      form={form}
      style={{ marginBottom: "1rem", paddingTop: "1rem", minWidth: "450px" }}
      onFinish={onFormFinish}
      initialValues={{
        newPassword: "",
        confirmPassword: "",
      }}
    >
      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Новый пароль"
        name="newPassword"
        rules={[
          { required: true, message: "Введите новый пароль" },
          { min: 6, message: "Пароль должен быть не менее 6 символов" },
          {
            pattern: /^(?=.*[A-Za-z])(?=.*\d)/,
            message: "Пароль должен содержать буквы и цифры",
          },
        ]}
      >
        <Input.Password allowClear placeholder="Введите новый пароль" />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Подтвердите пароль"
        name="confirmPassword"
        dependencies={["newPassword"]}
        rules={[
          { required: true, message: "Подтвердите пароль" },
          ({ getFieldValue }) => ({
            validator(_, value) {
              if (!value || getFieldValue("newPassword") === value) {
                return Promise.resolve();
              }
              return Promise.reject(new Error("Пароли не совпадают"));
            },
          }),
        ]}
      >
        <Input.Password allowClear placeholder="Подтвердите новый пароль" />
      </Form.Item>

      <Form.Item style={{ marginBottom: "1rem" }}>
        <Button htmlType="submit" style={{ width: "100%" }} type="primary">
          Сменить пароль
        </Button>
      </Form.Item>
    </Form>
  );
};
