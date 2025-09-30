import { IWorkerPosition } from "../../interfaces/IWorkerPosition";
import { Button, List, Popover, Typography } from "antd";
import { EditButton } from "../../components/buttons/EditButton";
import { DeleteOutlined } from "@ant-design/icons";
import useApplicationStore from "../../stores/applicationStore";
import { LinkButton } from "../../components/LinkButton";
import { EditPositionForm } from "./position/EditPositionForm";
import { useDeleteWorkerPosition } from "../../api/hooks/workerPositions";

const { Text } = Typography;

interface IPositionsListProps {
  items: IWorkerPosition[];
}

export const PositionsList = ({ items }: IPositionsListProps) => {
  const { user } = useApplicationStore.getState();

  const deleteAsync = useDeleteWorkerPosition();
  return (
    <List
      pagination={false}
      dataSource={items}
      renderItem={(item: IWorkerPosition) => (
        <List.Item
          key={item.id}
          style={{ padding: "0.2rem" }}
          actions={
            user?.isAdmin
              ? [
                  <Popover
                    placement="left"
                    destroyTooltipOnHide
                    trigger="click"
                    content={<EditPositionForm position={item} />}
                  >
                    <EditButton itemId={item.title} text="Изменить" />
                  </Popover>,
                  <Button
                    size="small"
                    icon={<DeleteOutlined style={{ color: "#dc4446" }} />}
                    onClick={() => deleteAsync({ workerPositionId: item.id })}
                  >
                    Удалить
                  </Button>,
                ]
              : [
                  <LinkButton
                    handleClicked={function (): void {
                      throw new Error("Function not implemented.");
                    }}
                  >
                    Смотреть разрешения
                  </LinkButton>,
                ]
          }
        >
          <Text>{item.title}</Text>
        </List.Item>
      )}
    />
  );
};
