import { Flex, Input, Button } from "antd";
import { SaveOutlined } from "@ant-design/icons";

interface AddItemFormProps {
  onOk: () => void;
  newItemTitle: string;
  setNewItemTitle: (title: string) => void;
}

export const AddItemForm = ({
  onOk,
  newItemTitle,
  setNewItemTitle,
}: AddItemFormProps) => {
  return (
    <Flex gap="16px" style={{ minWidth: "300px" }}>
      <Input
        value={newItemTitle}
        onChange={(e) => setNewItemTitle(e.target.value)}
        placeholder="Введите название пункта"
        onPressEnter={onOk}
      />
      <Button onClick={onOk} icon={<SaveOutlined />}>
        Сохранить
      </Button>
    </Flex>
  );
};
