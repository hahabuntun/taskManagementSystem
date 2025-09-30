import { Button, Checkbox, Form, Input, Select } from "antd";
import { IWorker } from "../../../interfaces/IWorker";
import { useGetWorkerPositions } from "../../../api/hooks/workerPositions";
import { IEditWorkerOptions } from "../../../api/options/editOptions/IEditWorkerOptions";
import { workerStatuses } from "../../../sync/workerStatuses";
import { EyeInvisibleOutlined, EyeTwoTone } from "@ant-design/icons";

interface IEditWorkerFormProps {
  worker: IWorker;
  onEditItem?: (itemId: number, options: IEditWorkerOptions) => void;
}

interface IFormValues {
  firstName: string;
  secondName: string;
  thirdName: string;
  email: string;
  statusId: number;
  workerPositionId: number;
  isAdmin: boolean;
  isManager: boolean;
  password: string;
}

export const EditWorkerForm = ({
  worker,
  onEditItem,
}: IEditWorkerFormProps) => {
  const { data: workerPositions } = useGetWorkerPositions();

  const onFormFinish = (values: IFormValues) => {
    const options: IEditWorkerOptions = {
      firstName: values.firstName,
      secondName: values.secondName,
      thirdName: values.thirdName,
      password: values.password,
      email: values.email,
      statusId: values.statusId,
      workerPositionId: values.workerPositionId,
      isAdmin: values.isAdmin,
      isManager: values.isManager,
    };

    onEditItem?.(worker.id, options);
  };

  return (
    <Form<IFormValues>
      style={{ marginBottom: "1rem", paddingTop: "2rem" }}
      onFinish={onFormFinish}
      initialValues={{
        firstName: worker.firstName,
        secondName: worker.secondName,
        thirdName: worker.thirdName,
        email: worker.email,
        workerPositionId: worker.workerPosition.id,
        statusId: worker.status.id,
        isAdmin: worker.isAdmin,
        isManager: worker.isManager,
      }}
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
        label="Должность"
        name="workerPositionId"
        rules={[{ required: true, message: "Выберите должность" }]}
      >
        <Select
          showSearch
          allowClear
          placeholder="Выберите должность"
          options={workerPositions?.map((position) => ({
            value: position.id,
            label: position.title,
          }))}
        />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Статус"
        name="statusId"
        rules={[{ required: true, message: "Выберите статус" }]}
      >
        <Select
          showSearch
          allowClear
          placeholder="Выберите статус"
          options={workerStatuses?.map((status) => ({
            value: status.id,
            label: status.name,
          }))}
        />
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        name="isAdmin"
        valuePropName="checked"
      >
        <Checkbox checked={worker.isAdmin}>Является администратором</Checkbox>
      </Form.Item>

      <Form.Item
        style={{ marginBottom: "1rem" }}
        name="isManager"
        valuePropName="checked"
      >
        <Checkbox checked={worker.isManager}>Является менеджером</Checkbox>
      </Form.Item>
      <Form.Item
        style={{ marginBottom: "1rem" }}
        label="Пароль"
        name="password"
        rules={[{ required: true, message: "Введите пароль" }]}
      >
        <Input.Password
          placeholder="Введите пароль"
          iconRender={(visible) =>
            visible ? (
              <EyeTwoTone data-testid="password-toggle-icon" />
            ) : (
              <EyeInvisibleOutlined data-testid="password-toggle-icon" />
            )
          }
        />
      </Form.Item>

      <Form.Item style={{ marginBottom: "1rem" }}>
        <Button htmlType="submit" style={{ width: "100%" }} type="primary">
          Сохранить изменения
        </Button>
      </Form.Item>
    </Form>
  );
};
