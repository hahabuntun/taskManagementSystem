import { Flex, Checkbox, Button, Popconfirm, Input } from "antd";
import { DeleteOutlined, EditOutlined } from "@ant-design/icons";
import { useState } from "react";
import { ICheckListItem } from "../../../interfaces/ICheckList";

interface ChecklistItemProps {
  item: ICheckListItem;
  onToggle: () => void;
  onSave: (newTitle: string) => void;
  onDelete: () => void;
}

export const ChecklistItem = ({
  item,
  onToggle,
  onSave,
  onDelete,
}: ChecklistItemProps) => {
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [localTitle, setLocalTitle] = useState<string>(item.title);

  return (
    <Flex justify="space-between" align="center">
      <Checkbox checked={item.isCompleted} onChange={onToggle}>
        {isEditing ? (
          <Input
            value={localTitle}
            onChange={(e) => setLocalTitle(e.target.value)}
            autoFocus
            style={{ width: "200px" }}
          />
        ) : (
          <span style={{ cursor: "pointer" }}>{item.title}</span>
        )}
      </Checkbox>
      <Flex gap="8px">
        {isEditing ? (
          <Button
            type="text"
            onClick={() => {
              onSave(localTitle);
              setIsEditing(false);
            }}
          >
            Сохранить
          </Button>
        ) : (
          <Button
            type="text"
            icon={<EditOutlined />}
            onClick={() => {
              setIsEditing(true);
              setLocalTitle(item.title);
            }}
          />
        )}
        <Popconfirm
          title="Удалить элемент?"
          onConfirm={onDelete}
          okText="Да"
          cancelText="Нет"
        >
          <Button type="text" danger icon={<DeleteOutlined />} />
        </Popconfirm>
      </Flex>
    </Flex>
  );
};
