import {
  Card,
  Collapse,
  Divider,
  Flex,
  Popover,
  Space,
  Typography,
} from "antd";
import {
  useAddBoard,
  useDeleteBoard,
  useGetBoards,
} from "../../api/hooks/boards";
import { BoardBasisEnum } from "../../enums/BoardBasisEnum";
import { LinkButton } from "../../components/LinkButton";
import { useNavigate } from "react-router-dom";
import { FilterBoardsForm } from "./FilterBoardsForm";
import { AddBoardForm } from "./board/AddBoardForm";
import { AddButton } from "../../components/buttons/AddButton";
import { DeleteButton } from "../../components/buttons/DeleteButton";
import { EditButton } from "../../components/buttons/EditButton";
import { EditBoardForm } from "./board/EditBoardForm";
import { IAddBoardOptions } from "../../api/options/createOptions/IAddBoardOptions";
import useApplicationStore from "../../stores/applicationStore";
import { IBoardFilterOptopns } from "../../api/options/filterOptions/IBoardFilterOptions";
import { BoardOwnerEnum } from "../../enums/ownerEntities/BoardOwnerEnum";

interface IBoardsFeatureProps {
  ownerType: BoardOwnerEnum;
  ownerId: number;
  type?: "personal" | "project";
}

const { Text } = Typography;

// Компонент для отображения иконки/заголовка в зависимости от типа доски
const getBoardTypeDisplay = (basis: BoardBasisEnum) => {
  switch (basis) {
    case BoardBasisEnum.STATUS_COLUMNS:
      return { type: "Статус задач", icon: "📊" };
    case BoardBasisEnum.PRIORITY_COLUMNS:
      return { type: "Приоритет задач", icon: "⭐" };
    case BoardBasisEnum.CUSTOM_COLUMNS:
      return { type: "Кастомные колонки", icon: "🎨" };
    case BoardBasisEnum.DATE:
      return { type: "Дедлайны задач", icon: "📅" };
    case BoardBasisEnum.ASIGNEE:
      return { type: "Ответственные за задачи", icon: "👤" };
    default:
      return { type: "Неизвестный тип", icon: "❓" };
  }
};

export const BoardsFeature = ({
  ownerType,
  ownerId,
  type,
}: IBoardsFeatureProps) => {
  const {
    data: boards,
    isLoading,
    setFilters,
  } = useGetBoards(ownerType, ownerId, type);

  const { user } = useApplicationStore.getState();

  const addAsync = useAddBoard();

  const deleteAsync = useDeleteBoard();

  const navigate = useNavigate();

  const handleFilterApply = (filters: IBoardFilterOptopns) => {
    setFilters(filters);
  };

  if (isLoading) {
    return (
      <div style={{ textAlign: "center", padding: "20px" }}>
        <Text>Загрузка досок...</Text>
      </div>
    );
  }

  return (
    <div style={{ padding: "24px" }}>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          Доски
        </Typography.Title>
        {ownerType === BoardOwnerEnum.WORKER && type === "project" ? null : (
          <Popover
            trigger="click"
            title="Добавление доски"
            content={() => (
              <AddBoardForm
                onAdded={(options: IAddBoardOptions) => {
                  addAsync({
                    entityType: ownerType,
                    entityId: ownerId,
                    options: options,
                    creator: user,
                  });
                }}
              />
            )}
          >
            <AddButton text="Добавить" />
          </Popover>
        )}
      </Space>

      <Collapse
        style={{ margin: "1rem auto" }}
        items={[
          {
            key: "filters",
            label: "Фильтры",
            children: <FilterBoardsForm onFilterApply={handleFilterApply} />,
          },
        ]}
      />

      <Divider />

      <Flex wrap="wrap" gap={16} justify="flex-start">
        {boards?.map((board) => {
          const { type, icon } = getBoardTypeDisplay(board.boardBasis);

          return (
            <Flex
              key={board.id}
              style={{
                width: "100%",
                maxWidth: "350px",
                flex: "0 0 auto",
                marginBottom: "16px",
              }}
              vertical
            >
              <Card
                hoverable
                title={
                  <LinkButton
                    icon={icon}
                    handleClicked={() => {
                      navigate(`/boards/${board.id}`);
                    }}
                  >
                    {board.name}
                  </LinkButton>
                }
                style={{
                  width: "100%",
                  height: "100%",
                  wordBreak: "break-word",
                }}
              >
                <Space
                  direction="vertical"
                  size="small"
                  style={{ width: "100%" }}
                >
                  <Space>
                    <Text type="secondary">Тип:</Text>
                    <Text>{type}</Text>
                  </Space>
                  <Text type="secondary">
                    Создано: {board.createdAt.format("DD.MM.YYYY")}
                  </Text>
                  {board.owner.type === BoardOwnerEnum.PROJECT && (
                    <Space>
                      <Text type="secondary">Проект:</Text>
                      <LinkButton
                        handleClicked={() =>
                          navigate(`/projects/${board.owner.id}`)
                        }
                      >
                        {board.owner.name}
                      </LinkButton>
                    </Space>
                  )}
                </Space>
                <Divider style={{ margin: "8px 0" }} />
                <Flex justify="end" align="center" gap="1rem">
                  <Popover
                    trigger="click"
                    content={<EditBoardForm board={board} />}
                  >
                    <EditButton text="Изменить" itemId={board.id} />
                  </Popover>
                  <DeleteButton
                    text="Удалить"
                    itemId={board.id}
                    onClick={() => {
                      deleteAsync({ boardId: board.id });
                    }}
                  />
                </Flex>
              </Card>
            </Flex>
          );
        })}
      </Flex>
    </div>
  );
};
