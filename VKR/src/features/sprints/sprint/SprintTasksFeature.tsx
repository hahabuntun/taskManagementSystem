import {
  Collapse,
  Divider,
  Popover,
  Space,
  Typography,
  Dropdown,
  Button,
  MenuProps,
} from "antd";
import { AddButton } from "../../../components/buttons/AddButton";
import { FilterTasksForm } from "../../tasks/FilterTasksForm";
import { TasksTable } from "../../tasks/TasksTable";
import { ITask } from "../../../interfaces/ITask";
import { AddExistingTaskFeature } from "../../tasks/AddExistingTaskFeature";
import { ITaskFilterOptions } from "../../../api/options/filterOptions/ITaskFilterOptions";
import {
  useAddTaskToSprint,
  useDeleteTaskFromSprint,
  useGetAvaiableTasksForSprint,
  useGetSprintTasks,
} from "../../../api/hooks/sprints";
import { useState } from "react";
import { GanttChart } from "../../tasks/taskViews/gantt/Chart";
import useApplicationStore from "../../../stores/applicationStore";
import { ISprint } from "../../../interfaces/ISprint";
import { StatusBoardView } from "../../tasks/taskViews/board/StatusBoardView";
import { PriorityBoardView } from "../../tasks/taskViews/board/PriorityBoardView";
import { DateBoardView } from "../../tasks/taskViews/board/DateBoardView";
import { AssigneeBoardView } from "../../tasks/taskViews/board/AsigneeBoardView";

interface ISprintTasksFeatureProps {
  sprint: ISprint;
}

export const SprintTasksFeature = ({ sprint }: ISprintTasksFeatureProps) => {
  const { isDarkMode } = useApplicationStore();

  const { user } = useApplicationStore.getState();

  const [viewType, setViewType] = useState<
    | "list"
    | "statusBoard"
    | "priorityBoard"
    | "dateBoard"
    | "responsibleForTaskBoard"
    | "chart"
  >("list");

  const handleViewChange = ({ key }: { key: string }) => {
    setViewType(key as typeof viewType);
  };

  // Adjust query based on viewType, similar to TasksFeature
  let query;
  query = useGetSprintTasks(sprint.id, sprint.projectId); // Assuming pagination is supported

  const { data: items, setFilters } = query;

  const {
    data: availableTasks,
    setFilters: setFiltersForAvailableTasks,
    refetch: refetchAvailableTasks,
  } = useGetAvaiableTasksForSprint(sprint.id, sprint.projectId);

  const addAsync = useAddTaskToSprint(() => {
    refetchAvailableTasks();
  });

  const deleteAsync = useDeleteTaskFromSprint(() => {
    refetchAvailableTasks();
  });

  const handleFilterApply = (filters: ITaskFilterOptions) => {
    setFilters(filters);
  };

  const handleApplyFilterForAvailableTasksToAdd = (
    filters: ITaskFilterOptions
  ) => {
    setFiltersForAvailableTasks(filters);
  };

  const menuItems: MenuProps["items"] = [
    { key: "list", label: "Список" },
    { key: "statusBoard", label: "Доска статусов" },
    { key: "priorityBoard", label: "Доска приоритетов" },
    { key: "dateBoard", label: "Доска дат" },
    { key: "responsibleForTaskBoard", label: "Доска ответственных" },
    { key: "chart", label: "График" },
  ];

  let Component;
  switch (viewType) {
    case "list":
      Component = (
        <TasksTable
          onDelete={(item: ITask) =>
            deleteAsync({ sprintId: sprint.id, task: item })
          }
          tasks={items ?? []}
        />
      );
      break;
    case "statusBoard":
      Component = <StatusBoardView tasks={items ?? []} />;
      break;
    case "priorityBoard":
      Component = <PriorityBoardView tasks={items ?? []} />;
      break;
    case "dateBoard":
      Component = <DateBoardView tasks={items ?? []} />;
      break;
    case "responsibleForTaskBoard":
      Component = <AssigneeBoardView tasks={items ?? []} />;
      break;
    case "chart":
      Component = <GanttChart isDarkMode={isDarkMode} tasks={items ?? []} />;
      break;
  }

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          Задачи Стринта
        </Typography.Title>
        <Popover
          trigger="click"
          title="Добавление задачи"
          destroyTooltipOnHide
          content={() => (
            <AddExistingTaskFeature
              handleFilterApply={handleApplyFilterForAvailableTasksToAdd}
              availableTasks={availableTasks || []}
              onAdded={(task: ITask) => {
                addAsync({ sprintId: sprint.id, task: task });
              }}
            />
          )}
        >
          <AddButton text="Добавить" />
        </Popover>

        <Dropdown
          menu={{
            items: menuItems,
            onClick: handleViewChange,
          }}
          trigger={["click"]}
        >
          <Button size="small" style={{ marginLeft: "1rem" }}>
            Переключить вид (Текущий: {viewType})
          </Button>
        </Dropdown>
      </Space>

      {user && (
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
      )}

      <Divider />

      {Component}
    </>
  );
};
