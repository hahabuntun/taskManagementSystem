import { Checkbox, Popover, Button } from "antd";
import { DownOutlined } from "@ant-design/icons";
import { useState } from "react";
import useApplicationStore from "../stores/applicationStore";

// Перевод названий полей на русский
const fieldLabels: Record<string, string> = {
  name: "Название",
  description: "Описание",
  type: "Тип",
  storyPoints: "Story Points",
  progress: "Прогресс",
  createdAt: "Дата создания",
  startDate: "Дата начала",
  endDate: "Дата окончания",
  status: "Статус",
  priority: "Приоритет",
  project: "Проект",
  sprint: "Спринт",
  creator: "Создатель",
  workers: "Исполнители",
  viewers: "Наблюдатели",
  responsible: "Ответственный",
  tags: "Теги",
};

// Список всех возможных полей
const availableFields = [
  "name",
  "description",
  "type",
  "storyPoints",
  "progress",
  "createdAt",
  "startDate",
  "endDate",
  "status",
  "priority",
  "project",
  "sprint",
  "creator",
  "workers",
  "viewers",
  "responsible",
  "tags",
];

export const FieldsSelector = () => {
  const [isPopoverOpen, setIsPopoverOpen] = useState(false);
  const { fieldsToShow, setFieldsToShow } = useApplicationStore(); // Get state directly from store

  const handleFieldClick = (field: string) => {
    if (field !== "name") {
      setFieldsToShow({
        ...fieldsToShow,
        [field]: !fieldsToShow[field],
      });
    }
  };

  // Контент для Popover
  const popoverContent = (
    <div style={{ display: "flex", flexDirection: "column" }}>
      {availableFields.map((field) => (
        <div
          key={field}
          onClick={() => handleFieldClick(field)}
          style={{
            padding: "8px",
            cursor: field === "name" ? "default" : "pointer",
            backgroundColor:
              field === "name" ? "rgba(255, 255, 255, 0.1)" : "transparent",
            color: "inherit",
          }}
        >
          <Checkbox
            checked={fieldsToShow[field]}
            disabled={field === "name"}
            style={
              {
                color: "inherit",
                "--antd-checkbox-check-color": "white",
              } as React.CSSProperties
            }
          >
            {fieldLabels[field]}
          </Checkbox>
        </div>
      ))}
    </div>
  );

  return (
    <Popover
      content={popoverContent}
      trigger="click"
      open={isPopoverOpen}
      onOpenChange={(open) => setIsPopoverOpen(open)}
      placement="rightTop"
    >
      <Button>
        Отображаемые поля <DownOutlined />
      </Button>
    </Popover>
  );
};
