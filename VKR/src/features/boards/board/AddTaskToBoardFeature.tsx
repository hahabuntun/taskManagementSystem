import {
  useAddTaskToBoard,
  useGetAvailableBoardTasks,
} from "../../../api/hooks/boards";
import { Collapse, Divider } from "antd";
import { FilterTasksForm } from "../../tasks/FilterTasksForm";
import { ITaskFilterOptions } from "../../../api/options/filterOptions/ITaskFilterOptions";
import { TasksTable } from "../../tasks/TasksTable";
import { ITask } from "../../../interfaces/ITask";

interface IAddTaskToBoardFeatureProps {
  boardId: number;
  workerId: number;
}

export const AddTaskToBoardFeature = ({
  boardId,
  workerId,
}: IAddTaskToBoardFeatureProps) => {
  const { data: items, setFilters } = useGetAvailableBoardTasks(
    boardId,
    workerId
  );

  const addAsync = useAddTaskToBoard();

  const handleFilterApply = (filters: ITaskFilterOptions) => {
    setFilters(filters);
  };
  return (
    <>
      <Collapse
        style={{ margin: "1rem auto" }}
        items={[
          {
            key: "filters",
            label: "Фильтры",
            children: (
              <FilterTasksForm
                workerId={workerId}
                onFilterApply={handleFilterApply}
              />
            ),
          },
        ]}
      />

      <Divider />

      <div style={{ maxHeight: "60vh" }}></div>
      <TasksTable
        onSelect={(task: ITask) =>
          addAsync({ boardId, options: { taskId: task.id } })
        }
        tasks={items ?? []}
      />
    </>
  );
};
