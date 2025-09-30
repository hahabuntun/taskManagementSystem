import { Button, Form, Input, Select } from "antd";
import { BoardBasisEnum } from "../../../enums/BoardBasisEnum";
import { IAddBoardOptions } from "../../../api/options/createOptions/IAddBoardOptions";

interface IAddBoardFormProps {
  onAdded?: (options: IAddBoardOptions) => void;
}

interface IFormValues {
  name: string;
  basis: BoardBasisEnum;
}

export const AddBoardForm = ({ onAdded }: IAddBoardFormProps) => {
  const [form] = Form.useForm();

  const onFormFinish = (values: IFormValues) => {
    const options: IAddBoardOptions = {
      name: values.name,
      basis: values.basis,
    };

    onAdded?.(options);
    form.resetFields();
  };

  // Используем ключи enum как значения и добавляем читаемые метки
  const boardBasisOptions = [
    { value: BoardBasisEnum.STATUS_COLUMNS, label: "Статус задач" },
    { value: BoardBasisEnum.PRIORITY_COLUMNS, label: "Приоритет задач" },
    { value: BoardBasisEnum.CUSTOM_COLUMNS, label: "Кастомные колонки" },
    { value: BoardBasisEnum.DATE, label: "Дедлайны задач" },
    { value: BoardBasisEnum.ASIGNEE, label: "Ответственные за задачи" },
  ];

  return (
    <Form form={form} onFinish={onFormFinish} layout="horizontal">
      <Form.Item
        name="name"
        label="Название доски"
        rules={[{ required: true, message: "Введите название доски" }]}
      >
        <Input placeholder="Введите название" />
      </Form.Item>

      <Form.Item
        name="basis"
        label="Тип колонок"
        rules={[{ required: true, message: "Выберите тип колонок!" }]}
      >
        <Select
          placeholder="Выберите тип колонок"
          options={boardBasisOptions}
        />
      </Form.Item>

      <Form.Item>
        <Button
          htmlType="submit"
          style={{ width: "100%", background: "green" }}
          type="primary"
        >
          Добавить
        </Button>
      </Form.Item>
    </Form>
  );
};
