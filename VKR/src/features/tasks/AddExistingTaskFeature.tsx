import { Collapse, Divider } from "antd";
import { ITask } from "../../interfaces/ITask";
import { FilterTasksForm } from "./FilterTasksForm";
import { TasksTable } from "./TasksTable";
import { ITaskFilterOptions } from "../../api/options/filterOptions/ITaskFilterOptions";
import useApplicationStore from "../../stores/applicationStore";

interface IAddExistingTaskFeatureProps {
  onAdded?: (task: ITask) => void;
  availableTasks: ITask[];
  handleFilterApply?: (filters: ITaskFilterOptions) => void;
}

export const AddExistingTaskFeature = ({
  onAdded,
  availableTasks,
  handleFilterApply,
}: IAddExistingTaskFeatureProps) => {
  const { user } = useApplicationStore.getState();
  if (user) {
    return (
      <div style={{ overflow: "auto" }}>
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

        <TasksTable
          onSelect={(item: ITask) => {
            onAdded?.(item);
          }}
          tasks={availableTasks}
        />
      </div>
    );
  }
};
