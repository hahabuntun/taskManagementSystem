import {
  useGetAvaiableTasksForSprint,
  useGetSprint,
} from "../../../api/hooks/sprints";
import { ITask } from "../../../interfaces/ITask";
import { TasksTable } from "../../tasks/TasksTable";

interface IAddTaskToSprintFormProps {
  sprintId: number;
  onAdded?: (task: ITask) => void;
}

export const AddTaskToSprintForm = ({
  sprintId,
  onAdded,
}: IAddTaskToSprintFormProps) => {
  const { data: sprint } = useGetSprint(sprintId);
  if (sprint) {
    const { data: tasks } = useGetAvaiableTasksForSprint(
      sprint.id,
      sprint.projectId
    );
    return (
      <>
        <TasksTable
          tasks={tasks ?? []}
          onSelect={(item: ITask) => onAdded?.(item)}
        />
      </>
    );
  }
};
