import { Button, Form, Input } from "antd";
import { IBoard } from "../../../interfaces/IBoard";
import { IEditBoardOptions } from "../../../api/options/editOptions/IEditBoardOptions";
import { useEditBoard } from "../../../api/hooks/boards";

interface IEditBoardFormProps {
  board: IBoard; // Передаем существующую доску для редактирования
}

interface IFormValues {
  name: string;
}

export const EditBoardForm = ({ board }: IEditBoardFormProps) => {
  const [form] = Form.useForm();

  const editAsync = useEditBoard();

  // Инициализируем форму начальными значениями из переданной доски
  const initialValues = {
    name: board.name,
    basis: board.boardBasis,
  };

  const onFormFinish = (values: IFormValues) => {
    const options: IEditBoardOptions = {
      name: values.name,
    };

    editAsync({ boardId: board.id, options });
    form.resetFields();
  };

  return (
    <Form
      form={form}
      onFinish={onFormFinish}
      layout="horizontal"
      initialValues={initialValues}
    >
      <Form.Item
        name="name"
        label="Название доски"
        rules={[{ required: true, message: "Введите название доски" }]}
      >
        <Input placeholder="Введите название" />
      </Form.Item>

      <Form.Item>
        <Button htmlType="submit" style={{ width: "100%" }} type="primary">
          Сохранить
        </Button>
      </Form.Item>
    </Form>
  );
};
