// components/TasksFeature.tsx
import { useState } from "react";
import {
  useAddTask,
  useDeleteTask,
  useEditTaskData,
  useGetTasks,
} from "../../api/hooks/tasks";
import {
  Button,
  Collapse,
  Divider,
  Dropdown,
  MenuProps,
  Modal,
  Space,
  Typography,
} from "antd";
import { FilterTasksForm } from "./FilterTasksForm";
import { AddButton } from "../../components/buttons/AddButton";
import { AddTaskForm } from "./task/AddTaskForm";
import { ITaskFilterOptions } from "../../api/options/filterOptions/ITaskFilterOptions";
import { TasksTable } from "./TasksTable";
import useApplicationStore from "../../stores/applicationStore";
import { IEditTaskOptions } from "../../api/options/editOptions/IEditTaskOptions";
import { IAddTaskOptions } from "../../api/options/createOptions/IAddTaskOptions";
import { PageOwnerEnum } from "../../enums/ownerEntities/PageOwnerEnum";
import { ITask } from "../../interfaces/ITask";
import { GanttChart } from "./taskViews/gantt/Chart";
import { AddTaskToSprintForm } from "../sprints/sprint/AddTaskToSprintForm";
import {
  useAddTaskToSprint,
  useDeleteTaskFromSprint,
} from "../../api/hooks/sprints";
import { AddTaskFormNoProject } from "./task/AddTaskFormNoProject";
import { StatusBoardView } from "./taskViews/board/StatusBoardView";
import { PriorityBoardView } from "./taskViews/board/PriorityBoardView";
import { DateBoardView } from "./taskViews/board/DateBoardView";
import { AssigneeBoardView } from "./taskViews/board/AsigneeBoardView";

interface ITasksFeatureProps {
  entityType: PageOwnerEnum;
  entityId: number;
  howRelatesToTask?: "creator" | "executor" | "responsible" | "viewer";
  projectId?: number;
}

export const TasksFeature = ({
  entityType,
  entityId,
  howRelatesToTask,
  projectId,
}: ITasksFeatureProps) => {
  const { user, isDarkMode } = useApplicationStore.getState();

  const [isAddTaskModalOpen, setIsAddTaskModal] = useState<boolean>(false);
  const [viewType, setViewType] = useState<
    | "list"
    | "statusBoard"
    | "priorityBoard"
    | "dateBoard"
    | "responsibleForTaskBoard"
    | "chart"
  >("list");
  const [currentFilters, setCurrentFilters] =
    useState<ITaskFilterOptions | null>(null);

  const handleViewChange = (key: string) => {
    setViewType(key as typeof viewType);
  };

  let query;
  if (entityType === PageOwnerEnum.SPRINT) {
    query = useGetTasks({
      entityId: entityId,
      pageType: entityType,
      projectId: projectId,
      enabled: !!projectId,
    });
  } else {
    query = useGetTasks({
      entityId,
      pageType: entityType,
      howRelatesToTask,
      enabled: true,
    });
  }

  const { setFilters } = query;

  const addAsync = useAddTask();
  const addTaskToSprint = useAddTaskToSprint();
  const deleteTaskFromSprint = useDeleteTaskFromSprint();
  const deleteAsync = useDeleteTask();
  const editAsync = useEditTaskData();

  const items = query.data || [];

  let Component;

  const handleFilterApply = (filters: ITaskFilterOptions) => {
    setCurrentFilters(filters);
    setFilters(filters);
  };

  switch (viewType) {
    case "list":
      Component = (
        <>
          {entityType === PageOwnerEnum.PROJECT && (
            <TasksTable
              onDelete={(item: ITask) => deleteAsync({ task: item })}
              onEdit={(item: ITask, options: IEditTaskOptions) =>
                editAsync({ taskId: item.id, options: options })
              }
              tasks={items}
            />
          )}

          {entityType === PageOwnerEnum.SPRINT && (
            <TasksTable
              onDelete={(item: ITask) =>
                deleteTaskFromSprint({ sprintId: entityId, task: item })
              }
              deleteText="Удалить из спринта"
              tasks={items}
            />
          )}
          {entityType !== PageOwnerEnum.SPRINT &&
            entityType !== PageOwnerEnum.PROJECT && (
              <TasksTable tasks={items} />
            )}
        </>
      );
      break;
    case "statusBoard":
      Component = <StatusBoardView tasks={items} />;
      break;
    case "priorityBoard":
      Component = <PriorityBoardView tasks={items} />;
      break;
    case "dateBoard":
      Component = <DateBoardView tasks={items} />;
      break;
    case "responsibleForTaskBoard":
      Component = <AssigneeBoardView tasks={items} />;
      break;
    case "chart":
      Component = <GanttChart isDarkMode={isDarkMode} tasks={items} />;
      break;
  }

  const menuItems: MenuProps["items"] = [
    { key: "list", label: "Список" },
    { key: "statusBoard", label: "Доска статусов" },
    { key: "priorityBoard", label: "Доска приоритетов" },
    { key: "dateBoard", label: "Доска дат" },
    { key: "responsibleForTaskBoard", label: "Доска ответственных" },
    { key: "chart", label: "График" },
  ];

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          Задачи
        </Typography.Title>
        <AddButton onClick={() => setIsAddTaskModal(true)} text="Добавить" />

        <Dropdown
          menu={{
            items: menuItems,
            onClick: ({ key }) => handleViewChange(key),
          }}
          trigger={["click"]}
        >
          <Button size="small" style={{ marginLeft: "1rem" }}>
            Переключить вид
          </Button>
        </Dropdown>
      </Space>

      {user && (
        <Modal
          title="Создание задачи"
          style={{ overflowX: "auto" }}
          width={800}
          footer={null}
          onOk={() => setIsAddTaskModal(false)}
          onClose={() => setIsAddTaskModal(false)}
          onCancel={() => setIsAddTaskModal(false)}
          open={isAddTaskModalOpen}
        >
          {entityType === PageOwnerEnum.PROJECT && (
            <AddTaskForm
              onSubmit={(options: IAddTaskOptions) => {
                addAsync({ projectId: entityId, options, creator: user });
              }}
              projectId={entityId}
              user={user}
            />
          )}
          {entityType === PageOwnerEnum.SPRINT && (
            <AddTaskToSprintForm
              onAdded={(task: ITask) =>
                addTaskToSprint({ sprintId: entityId, task: task })
              }
              sprintId={entityId}
            />
          )}
          {entityType === PageOwnerEnum.WORKER && (
            <AddTaskFormNoProject user={user} />
          )}
        </Modal>
      )}

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
                  initialFilters={currentFilters ?? {}}
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
