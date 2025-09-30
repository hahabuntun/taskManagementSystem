import { Flex, Typography, Button, Input, Popover, Spin, Alert } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { ChecklistCard } from "./checklist/ChecklistCard";
import { AddItemForm } from "./checklist/AddItemForm";
import { ChecklistItem } from "./checklist/ChecklistItem";
import { CheckListOwnerEnum } from "../../enums/ownerEntities/CheckListOwnerEnum";
import { useChecklists } from "../../api/hooks/checklists";

interface ChecklistsFeatureProps {
  ownerType: CheckListOwnerEnum;
  ownerId: number;
}

const ChecklistsFeature = ({ ownerType, ownerId }: ChecklistsFeatureProps) => {
  const {
    checklists,
    isLoading,
    error,
    newChecklistTitle,
    newItemTitle,
    setNewChecklistTitle,
    setNewItemTitle,
    handleAddChecklist,
    handleAddItem,
    handleToggleItem,
    handleSaveItem,
    handleDeleteItem,
  } = useChecklists(ownerType, ownerId);

  return (
    <Flex vertical gap="16px">
      {/* Add New Checklist */}
      <Flex align="center" gap="8px">
        <Typography.Title level={5}>Чек-листы</Typography.Title>
        <Input
          value={newChecklistTitle}
          onChange={(e) => setNewChecklistTitle(e.target.value)}
          placeholder="Название нового чек-листа"
          onPressEnter={handleAddChecklist}
          style={{ maxWidth: "300px" }}
        />
        <Button
          type="primary"
          size="small"
          icon={<PlusOutlined />}
          onClick={handleAddChecklist}
          disabled={isLoading}
        />
      </Flex>

      {/* Loading and Error States */}
      {isLoading && <Spin tip="Загрузка чек-листов..." />}
      {error && (
        <Alert
          message="Ошибка загрузки чек-листов"
          description={error.message}
          type="error"
          showIcon
        />
      )}

      {/* Checklists */}
      {checklists.length > 0 && (
        <Flex vertical gap="16px">
          {checklists.map((checklist) => (
            <ChecklistCard key={checklist.id} checklistTitle={checklist.title}>
              <Flex vertical gap="8px">
                {checklist.items.map((item) => (
                  <ChecklistItem
                    key={item.id}
                    item={item}
                    onToggle={() =>
                      handleToggleItem(item.id, !item.isCompleted)
                    }
                    onSave={(newTitle) => handleSaveItem(item.id, newTitle)}
                    onDelete={() => handleDeleteItem(item.id)}
                  />
                ))}
                <Popover
                  trigger="click"
                  content={
                    <AddItemForm
                      onOk={() => handleAddItem(checklist.id)}
                      newItemTitle={newItemTitle}
                      setNewItemTitle={setNewItemTitle}
                    />
                  }
                >
                  <Button
                    type="dashed"
                    size="small"
                    icon={<PlusOutlined />}
                    style={{ width: "100%" }}
                  >
                    Добавить пункт
                  </Button>
                </Popover>
              </Flex>
            </ChecklistCard>
          ))}
        </Flex>
      )}
    </Flex>
  );
};

export default ChecklistsFeature;
