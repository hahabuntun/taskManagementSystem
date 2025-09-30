import { Collapse, Divider, Popover, Space, Typography } from "antd";
import { useGetBoardTasks } from "../../../api/hooks/boards";
import { ITaskFilterOptions } from "../../../api/options/filterOptions/ITaskFilterOptions";
import useApplicationStore from "../../../stores/applicationStore";
import { AddTaskToBoardFeature } from "./AddTaskToBoardFeature";
import { AddButton } from "../../../components/buttons/AddButton";
import { FilterTasksForm } from "../../tasks/FilterTasksForm";
import { BoardBasisEnum } from "../../../enums/BoardBasisEnum";
import { StatusBoard } from "./StatusBoard";
import { PriorityBoard } from "./PriorityBoard";
import { DeadlineBoard } from "./DeadlineBoard";
import { AssigneeBoard } from "./AsigneeBoard";
import { CustomColumnsBoard } from "./CustomColumnsBoard";

interface IBoardTasksFeatureProps {
  boardId: number;
  boardBasic: BoardBasisEnum;
}

export const BoardTasksFeature = ({
  boardId,
  boardBasic,
}: IBoardTasksFeatureProps) => {
  const { data: tasks, isLoading, setFilters } = useGetBoardTasks(boardId);

  const { user } = useApplicationStore.getState();

  const viewType =
    boardBasic === BoardBasisEnum.STATUS_COLUMNS
      ? "statusBoard"
      : boardBasic === BoardBasisEnum.PRIORITY_COLUMNS
      ? "priorityBoard"
      : boardBasic === BoardBasisEnum.DATE
      ? "dateBoard"
      : boardBasic === BoardBasisEnum.ASIGNEE
      ? "responsibleForTaskBoard"
      : "customBoard";

  const handleFilterApply = (filters: ITaskFilterOptions) => {
    setFilters(filters);
  };

  let Component;
  if (tasks) {
    switch (viewType) {
      case "statusBoard":
        Component = <StatusBoard tasks={tasks} boardId={boardId} />;
        break;
      case "priorityBoard":
        Component = <PriorityBoard boardId={boardId} />;
        break;
      case "dateBoard":
        Component = <DeadlineBoard boardId={boardId} />;
        break;
      case "responsibleForTaskBoard":
        Component = <AssigneeBoard boardId={boardId} />;
        break;
      case "customBoard":
        Component = <CustomColumnsBoard boardId={boardId} />;
        break;
    }

    if (isLoading) return <div>Загрузка задач...</div>;
    if (!user) return <div>Необходима авторизация</div>;

    return (
      <>
        <Space align="baseline" wrap>
          <Typography.Title
            level={3}
            style={{ margin: 0, alignSelf: "center" }}
          >
            Задачи
          </Typography.Title>
          <Popover
            trigger="click"
            title="Добавить задачу"
            placement="bottomRight"
            content={() => (
              <AddTaskToBoardFeature workerId={user.id} boardId={boardId} />
            )}
          >
            <AddButton text="Добавить задачу" />
          </Popover>
        </Space>

        <Collapse
          style={{ margin: "1rem auto" }}
          items={[
            {
              key: "filters",
              label: "Фильтры",
              children: (
                <FilterTasksForm
                  workerId={user.id}
                  onFilterApply={handleFilterApply}
                />
              ),
            },
          ]}
        />

        <Divider />

        {Component}
      </>
    );
  }
};
