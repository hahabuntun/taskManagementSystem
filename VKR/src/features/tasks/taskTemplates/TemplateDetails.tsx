import { Flex, Card, Typography } from "antd";
import { useGetTaskTemplate } from "../../../api/hooks/tasks";
import { DeleteButton } from "../../../components/buttons/DeleteButton";
import { EditButton } from "../../../components/buttons/EditButton";
import { TaskTemplateTagManager } from "./TemplateTagManager";

interface TemplateDetailsProps {
  templateId: number;
  onDelete?: (templateId: number) => void;
}

export const TemplateDetails = ({
  templateId,
  onDelete,
}: TemplateDetailsProps) => {
  const { data: template } = useGetTaskTemplate(templateId);

  if (!template) return null;

  // Формируем массив характеристик, исключая undefined/null
  const details = [
    { label: "Тип:", value: template.type },
    {
      label: "Прогресс:",
      value:
        template.progress !== undefined ? `${template.progress}%` : undefined,
    },
    { label: "Создано:", value: template.createdAt?.format("DD.MM.YYYY") }, // Обязательное поле
    { label: "Начало:", value: template.startDate?.format("DD.MM.YYYY") },
    { label: "Окончание:", value: template.endDate?.format("DD.MM.YYYY") },
    { label: "Статус:", value: template.status?.name },
    { label: "Приоритет:", value: template.priority?.name },
  ].filter((item) => item.value !== undefined && item.value !== null); // Исключаем undefined и null

  return (
    <Flex
      vertical
      gap="24px"
      style={{ padding: "24px", margin: "0 auto", maxWidth: "1200px" }}
    >
      <Flex
        gap="24px"
        justify="space-around"
        wrap="wrap"
        style={{ width: "100%" }}
      >
        <Card
          style={{
            minWidth: "770px",
            borderRadius: "8px",
            boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
          }}
        >
          <Flex vertical gap="8px">
            <Typography.Title level={4}>{template.name}</Typography.Title>
            {template.description && (
              <Typography.Text
                type="secondary"
                style={{ fontSize: "16px", lineHeight: "1.6" }}
              >
                {template.description}
              </Typography.Text>
            )}
            {details.map((item, index) => (
              <Flex key={index} justify="space-between" align="center">
                <Typography.Text style={{ minWidth: "120px" }}>
                  {item.label}
                </Typography.Text>
                <Typography.Text type="secondary">{item.value}</Typography.Text>
              </Flex>
            ))}
          </Flex>
        </Card>
      </Flex>
      <Card
        title="Теги"
        style={{
          minWidth: "770px",
          borderRadius: "8px",
          boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
        }}
      >
        <TaskTemplateTagManager templateId={template.id} />
      </Card>
      <Flex justify="end">
        {onDelete && (
          <DeleteButton
            style={{ marginRight: "1rem" }}
            onClick={() => onDelete(template.id)}
            text="Удалить шаблон"
            itemId={template.id}
          />
        )}
        <EditButton text="Изменить шаблон" itemId={template.id} />
      </Flex>
    </Flex>
  );
};
