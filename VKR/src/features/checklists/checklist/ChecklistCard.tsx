import { Card } from "antd";

interface ChecklistCardProps {
  checklistTitle: string;
  children: React.ReactNode;
}

export const ChecklistCard = ({
  checklistTitle,
  children,
}: ChecklistCardProps) => {
  return (
    <Card
      title={checklistTitle}
      style={{
        width: "100%",
        marginBottom: "16px",
        borderRadius: 8,
        boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
      }}
    >
      {children}
    </Card>
  );
};
