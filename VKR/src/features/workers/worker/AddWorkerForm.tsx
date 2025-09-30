import { Button, Checkbox, Form, Input, Select } from "antd";
import { IAddWorkerOptions } from "../../../api/options/createOptions/IAddWorkerOptions";
import { useGetWorkerPositions } from "../../../api/hooks/workerPositions";
import { IWorkerStatus } from "../../../interfaces/IWorkerStatus";
import { IWorkerPosition } from "../../../interfaces/IWorkerPosition";
import { workerStatuses } from "../../../sync/workerStatuses";

interface IAddWorkerFormProps {
  onAddItem?: (options: IAddWorkerOptions) => void;
}

interface IFormValues {
  firstName: string;
  secondName: string;
  thirdName: string;
  email: string;
  status: string;
  workerPosition: string;
  isAdmin: boolean;
  isManager: boolean;
  password: string;
}

export const AddWorkerForm = ({ onAddItem }: IAddWorkerFormProps) => {
  const { data: workerPositions } = useGetWorkerPositions();

  const onFormFinish = (values: IFormValues) => {
    const status: IWorkerStatus | undefined = workerStatuses.find(
      (status) => status.name === values.status
    );
    const position: IWorkerPosition | undefined = workerPositions?.find(
      (position) => position.title === values.workerPosition
    );
    if (status && position) {
      const options: IAddWorkerOptions = {
        firstName: values.firstName,
        secondName: values.secondName,
        thirdName: values.thirdName,
        email: values.email,
        status: status,
        workerPosition: position,
        isAdmin: values.isAdmin,
        isManager: values.isManager,
        password: "123",
      };
      onAddItem?.(options);
    }
  };

  return (
    <Form<IFormValues>
      style={{ marginBottom: "1rem", paddingTop: "2rem" }}
      onFinish={onFormFinish}
      initialValues={{ canManageWorkers: false, canManageProjects: false }}
    >
      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Имя"
        name="firstName"
        rules={[
          { required: true, message: "Введите имя" },
          { min: 2, max: 30, message: "Имя должно быть от 2 до 30 символов" },
        ]}
      >
        <Input allowClear placeholder="Введите имя" />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Фамилия"
        name="secondName"
        rules={[
          { required: true, message: "Введите фамилию" },
          {
            min: 2,
            max: 30,
            message: "Фамилия должна быть от 2 до 30 символов",
          },
        ]}
      >
        <Input allowClear placeholder="Введите фамилию" />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Отчество"
        name="thirdName"
        rules={[
          { max: 30, message: "Отчество не должно превышать 30 символов" },
        ]}
      >
        <Input allowClear placeholder="Введите отчество" />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Email"
        name="email"
        rules={[
          { required: true, message: "Введите email" },
          { type: "email", message: "Введите корректный email" },
        ]}
      >
        <Input allowClear placeholder="Введите Email" />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Пароль"
        name="password"
        rules={[
          { required: true, message: "Введите пароль" },
          { message: "Введите пароль" },
        ]}
      >
        <Input.Password allowClear placeholder="Введите пароль" />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Должность"
        name="workerPosition"
        rules={[{ required: true, message: "Выберите должность" }]}
      >
        <Select
          showSearch
          allowClear
          placeholder="Выберите должность"
          options={workerPositions?.map((position) => ({
            value: position.title,
            label: position.title,
          }))}
        />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Статус"
        name="status"
        rules={[{ required: true, message: "Выберите статус" }]}
      >
        <Select
          showSearch
          allowClear
          placeholder="Выберите статус"
          options={workerStatuses?.map((status) => ({
            value: status.name,
            label: status.name,
          }))}
        />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        name="isAdmin"
        valuePropName="checked"
      >
        <Checkbox>Является администратором</Checkbox>
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        name="isManager"
        valuePropName="checked"
      >
        <Checkbox>Является менеджером</Checkbox>
      </Form.Item>

      <Form.Item style={{ marginBottom: "1rem" }}>
        <Button
          htmlType="submit"
          style={{ background: "green", width: "100%" }}
          type="primary"
        >
          Добавить сотрудника
        </Button>
      </Form.Item>
    </Form>
  );
};
